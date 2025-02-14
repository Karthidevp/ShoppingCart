using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interface;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        #region Common Extension

        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;

        }
        public IActionResult SessionExpired()
        {
            TempData["SessionExpiredMessage"] = "Your session has expired. Please log in again.";
            return RedirectToAction("Login");
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Return the login view
            ViewBag.SessionExpiredMessage = TempData["SessionExpiredMessage"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            //User Login 
            var (success, message, role, Userid) = await _accountServices.LoginUserAsync(login);
           
            if (success) {
                HttpContext.Session.SetString("Email", login.EmailId);
                if (role != null && Userid != null)
                {
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetInt32("UserId", Userid);
                    return RedirectToAction("Product", "Home");
                }
            }
            if (message != null)
            {
                ModelState.AddModelError("", message);
                return View();
            }
            // Super Admin Login 
            var (Success, Message, Role) = await _accountServices.AdminLoginAsync(login);
            if (Success) 
            {
                if (Role != null)
                {
                    HttpContext.Session.SetString("Email", login.EmailId);
                    HttpContext.Session.SetString("Role", Role);
                    return RedirectToAction("Index", "Home");
                }

             }

    
            ModelState.AddModelError("", Message);
                return View();
     
        }
        [HttpPost]
        public IActionResult LogOut(int id)
        {
            // Clear session
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View(); // Create a corresponding view to show the access denied message
        }

    }
}
