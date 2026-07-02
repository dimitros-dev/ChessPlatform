using System.Text.Json.Serialization.Metadata;
using ChessPlatform.Data;
using ChessPlatform.Models;
using ChessPlatform.Repositories;
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
        public TournamentController(ChessPlatformContext context,UserManager<ApplicationUser> userManager,
                                    ITournamentRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _repository = repository;
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
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                CreatedById = _userManager.GetUserId(User)
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tournaments = await _context.Tournaments.Where(t => t.CreatedById == userId).ToListAsync();

            return View(tournaments);
        }

        [Authorize(Roles = "Player")]
        public async Task<IActionResult> Join(int id)
        {
            var userId = _userManager.GetUserId(User);
            var alreadyJoined = await _context.TournamentPlayers
                                .AnyAsync(tp => tp.TournamentId == id && tp.PlayerId == userId);

            if (!alreadyJoined)
            {
                var tournamentPlayer = new TournamentPlayer
                {
                    TournamentId = id,
                    PlayerId = userId
                };

                _context.TournamentPlayers.Add(tournamentPlayer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }

        [Authorize(Roles = "Player")]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = _userManager.GetUserId(User);

            var record = await _context.TournamentPlayers
                .FirstOrDefaultAsync(tp => tp.TournamentId == id && tp.PlayerId == userId);

            if (record != null)
            {
                _context.TournamentPlayers.Remove(record);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Tournaments", "Player");
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(t => t.Id == id && t.CreatedById == userId);

            if (tournament != null)
            {
                var players = _context.TournamentPlayers
                    .Where(x => x.TournamentId == id);

                _context.TournamentPlayers.RemoveRange(players);

                var matches = _context.Matches
                    .Where(x => x.TournamentId == id);

                _context.Matches.RemoveRange(matches);
                _context.Tournaments.Remove(tournament);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Start(int id)
        {
            var userId = _userManager.GetUserId(User);

            var tournament = await _context.Tournaments.
                            FirstOrDefaultAsync(t => t.Id == id && t.CreatedById == userId);

            var playerCount = await _context.TournamentPlayers.CountAsync(tp => tp.TournamentId == id);

            var players = await _context.TournamentPlayers.Where(tp => tp.TournamentId == id)
                                                          .Select(tp => tp.PlayerId).ToListAsync();

            if (tournament == null)
            {
                return NotFound();
            }

            if (playerCount < 2)
            {
                return Content("At least 2 players are required");
            }

            tournament.IsStarted = true;

            _context.Tournaments.Update(tournament);

            for (int i = 0; i < players.Count; i += 2)
            {
                var match = new Match
                {
                    TournamentId = id,
                    Player1Id = players[i],
                    Player2Id = players[i + 1],
                    IsFinished = false
                };

                _context.Matches.Add(match);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
