using System.Threading.Tasks;
using ChessPlatform.Data;
using ChessPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ChessPlatformContext _context;
        public AdminController(UserManager<ApplicationUser> userManager, ChessPlatformContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<(ApplicationUser User, IList<string> Roles)>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add((user, roles));
            }
            
            return View(userList);
        }
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            var currentUser = await _userManager.GetUserAsync(User);

           
            if (currentUser.Id == userId)
            {
                return BadRequest("You cannot change your role.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction("Users");
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (id == currentUserId)
            {
                return BadRequest("You cannot delete your own account.");
            }
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var tournamentPlayers = _context.TournamentPlayers
                .Where(x => x.PlayerId == id);

            _context.TournamentPlayers.RemoveRange(tournamentPlayers);

            var matches = _context.Matches
                .Where(x => x.Player1Id == id || x.Player2Id == id);

            _context.Matches.RemoveRange(matches);

            var tournaments = _context.Tournaments
                .Where(x => x.CreatedById == id);

            _context.Tournaments.RemoveRange(tournaments);

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return RedirectToAction("Users");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Tournaments()
        {
            var tournaments = await _context.Tournaments.Include(t => t.CreatedBy).ToListAsync();

            return View(tournaments);
        }
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentPlayers = _context.TournamentPlayers
                .Where(tp => tp.TournamentId == id);

            _context.TournamentPlayers.RemoveRange(tournamentPlayers);

            var matches = _context.Matches.Where(m => m.TournamentId == id);

            _context.Matches.RemoveRange(matches);

            _context.Tournaments.Remove(tournament);

            await _context.SaveChangesAsync();

            return RedirectToAction("Tournaments");
        }
    }
}
