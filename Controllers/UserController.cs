using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interface;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class UserController : Controller
    {


        #region Common Extension

        private readonly IUserservice _userservice;
        public UserController(IUserservice userservice)
        {
            _userservice = userservice;

        }

        #endregion

        #region Get User List
        public async Task<IActionResult> UserList()
        {
            var Userlist = await _userservice.GetUserListAsync();
            return View(Userlist);
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> Profile(int Id)
        {
            var UserDetail = await _userservice.GetUserByIdAsync(Id);

            return View(UserDetail);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(Users users)
        {
            if (ModelState.IsValid)
            {

                var (success, message) = await _userservice.CreteUser(users);
                if (success)
                {

                    TempData["success"] = message;
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong";
                    return View();
                }

            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(Users users)
        {
            await _userservice.UpdateUserAsync(users);

            return View();
        }

    }


}

