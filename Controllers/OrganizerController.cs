using ChessPlatform.Data;
using ChessPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessPlatform.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ChessPlatformContext _context;

        public OrganizerController(UserManager<ApplicationUser> userManager, ChessPlatformContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /*public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tournaments = await _context.Tournaments.Where(t => t.CreatedById == userId).ToListAsync();

            return View(tournaments);
        }*/
    }
}
