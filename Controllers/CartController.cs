using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ShoppingCart.Interface;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        //public iactionresult index()
        //{
        //    return view();
        //}
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var EmaiId = HttpContext.Session.GetString("Email"); // Replace with actual user ID from your authentication system
            var carts = await _cartService.GetCartsByEmailIdAsync(EmaiId);
            return View(carts);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId")); // Replace with actual user ID retrieval
           
            if (userId!= null)
            {
                var cart = new Cart
                {
                    UserId = userId,
                    ProductId = id,
                };
              await _cartService.AddToCartAsync(cart); // Ensure your service method is designed to handle CartItem
                TempData["success"] = "Product added to cart!";
                return RedirectToAction("Product", "Home");
            }
            return View();
           
        }


        public async Task<IActionResult> RemoveFromCart(int id)
        {
            
            await _cartService.RemoveFromCartAsync(id);
            TempData["success"] = "Product removed from cart!";
            return RedirectToAction("Index");
        }
    }
}
