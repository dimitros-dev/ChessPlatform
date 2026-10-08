using System.Text.Json.Serialization.Metadata;
using ChessPlatform.Data;
using ChessPlatform.Models;
using ChessPlatform.Repositories;
using ChessPlatform.Services;
using ChessPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessPlatform.Controllers
{
    
    public class TournamentController : Controller
    {
        private readonly ChessPlatformContext _context;
        private readonly ITournamentRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITournamentService _tournamentService;
        public TournamentController(ChessPlatformContext context,UserManager<ApplicationUser> userManager,
                                    ITournamentRepository repository, ITournamentService tournamentService)
        {
            _context = context;
            _userManager = userManager;
            _repository = repository;
            _tournamentService = tournamentService;
        }
        [Authorize(Roles = "Organizer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TournamentCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var tournament = new Tournament
            {
                Name = vm.Name,
                Description = vm.Description,
                TournamentType = vm.TournamentType,
                TimeControlMinutes = vm.TimeControlMinutes,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                CreatedById = _userManager.GetUserId(User)
            };

            var created = await _tournamentService.CreateTournamentAsync(tournament);
            if (!created)
            {
                ModelState.AddModelError("", "End date must be after the start date");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tournaments = await _context.Tournaments.Where(t => t.CreatedById == userId).ToListAsync();

            return View(tournaments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Player")]
        public async Task<IActionResult> Join(int id)
        {
            var userId = _userManager.GetUserId(User);

            var joined = await _tournamentService.JoinTournamentAsync(id, userId);

            if (!joined)
            {
                return Content("You cannot join this tournament");
            }

            return RedirectToAction("Index", "Player");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Player")]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = _userManager.GetUserId(User);

            await _tournamentService.LeaveTournamentAsync(id, userId);

            return RedirectToAction("Tournaments", "Player");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            await _tournamentService.DeleteTournamentAsync(id, userId);

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Start(int id)
        {
            var userId = _userManager.GetUserId(User);

            //await _tournamentService.StartTournamentAsync(id, userId);
            var started = await _tournamentService.StartTournamentAsync(id, userId);

            if (!started)
            {
                return Content("Tournament could not be started.");
            }

            return RedirectToAction("Index");

            //return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Standings(int id)
        {
            var standings = await _tournamentService.GetStandingsAsync(id);

            return View(standings);
        }

    }
}
