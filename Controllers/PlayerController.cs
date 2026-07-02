using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessPlatform.Data;
using ChessPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChessPlatform.Controllers
{
    [Authorize(Roles = "Player")]
    public class PlayerController : Controller
    {
        private readonly ChessPlatformContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerController(ChessPlatformContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var myTournaments = await _context.TournamentPlayers.Where(tp => tp.PlayerId == userId)
                .Select(tp => tp.Tournament).ToListAsync();

            var wins = await _context.Matches.CountAsync(m =>(m.Player1Id == userId && m.Result == "WhiteWin") ||
                                                        (m.Player2Id == userId && m.Result == "BlackWin"));

            var draws = await _context.Matches.CountAsync(m => (m.Player1Id == userId || m.Player2Id == userId) &&
                m.Result == "Draw");

            var played = await _context.Matches.CountAsync(m => (m.Player1Id == userId || m.Player2Id == userId) &&
                                                            m.IsFinished);

            var losses = played - wins - draws;
            var winRate = played == 0 ? 0 : (double)wins / played * 100;

            ViewBag.Wins = wins;
            ViewBag.Losses = losses;
            ViewBag.Draws = draws;
            ViewBag.Played = played;
            ViewBag.WinRate = winRate;

            return View(myTournaments);
        }
        public async Task<IActionResult> Tournaments()
        {
            var userId = _userManager.GetUserId(User);
            var tournaments = await _context.Tournaments.ToListAsync();

            var joinedIds = await _context.TournamentPlayers.Where(tp => tp.PlayerId == userId)
                .Select(tp => tp.TournamentId).ToListAsync();

            ViewBag.JoinedIds = joinedIds;
            
            return View(tournaments);
        }
        public async Task<IActionResult> MyMatches()
        {
            var userId = _userManager.GetUserId(User);

            var matches = await _context.Matches.Include(m => m.Player1).Include(m => m.Player2)
            .Where(m => m.Player1Id == userId || m.Player2Id == userId)
            .ToListAsync();

            ViewBag.UserId = userId;

            return View(matches);
        }
    }
}
