using ChessPlatform.Models;
using ChessPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChessPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
               UserName = model.Username,
               Email = model.Email,
               FirstName = model.FirstName,
               LastName = model.LastName,
               DateOfBirth = model.DateOfBirth,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
   
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Player");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Player");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent:false,
                                                                        lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                if(await _userManager.IsInRoleAsync(user, "Organizer"))
                {
                    return RedirectToAction("Index", "Tournament");
                }
                return RedirectToAction("Index", "Player");
            }
           

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }
    }
}
