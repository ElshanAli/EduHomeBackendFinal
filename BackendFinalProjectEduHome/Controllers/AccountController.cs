using BackendFinalProjectEduHome.DAL.Entities;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string? returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var activeUser = await _userManager.FindByNameAsync(model.UserName);

            if (activeUser is null)
            {
                ModelState.AddModelError("", "Email or Username is incorrect");
                return View(model);
            }

            var signResult = await _signInManager.PasswordSignInAsync(activeUser, model.Password, model.RememberMe, true);

            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Username is incorrect");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl)) return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
