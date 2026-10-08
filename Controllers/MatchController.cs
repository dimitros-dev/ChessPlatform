using ChessPlatform.Data;
using ChessPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessPlatform.Controllers
{
    public class MatchController : Controller
    {
        private readonly ChessPlatformContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MatchController(ChessPlatformContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var match = await _context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    (m.Player1Id == userId || m.Player2Id == userId));

            if (match == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(t => t.Id == match.TournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            ViewBag.TimeControlMinutes = tournament.TimeControlMinutes;

            return View(match);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SetResult(int id, string result)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }
            if (result == "WhiteWin")
            {
                match.WinnerId = match.Player1Id;
            }
            else if (result == "BlackWin")
            {
                match.WinnerId = match.Player2Id;
            }
            else if (result == "Draw")
            {
                match.WinnerId = null;
            }
            match.Result = result;
            match.IsFinished = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }
    }
}
