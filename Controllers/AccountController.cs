using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcLoginTemplate.Models;

namespace MvcLoginTemplate.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;  // Store the ReturnUrl to pass it back to the view
            return View();
        }

        [HttpPost]
        public async  Task<ActionResult> Login(string Username, string Password,string returnUrl=null)
        {
            // Check if the username and password are "Sneh"
            if (Username == "Sneh" && Password == "Sneh@123")
            {
                var user = await _userManager.FindByNameAsync(Username);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // If credentials are incorrect, display an error message
            ViewBag.Error = "Invalid login attempt. Please try again.";
            ViewData["ReturnUrl"] = returnUrl;  // Pass the ReturnUrl back to the view in case of failure
            return View("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
