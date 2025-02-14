using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ShoppingCart.Interface;
using ShoppingCart.Models;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices _productServices;
        public HomeController(ILogger<HomeController> logger, IProductServices productServices)
        {
            _logger = logger;
            _productServices = productServices;
        }
        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Product()
        {
            //HttpContext.Session.SetString("Email", "Cart");
            var products = await _productServices.GetAllProductsAsync();
            return View(products); // Make sure products is of type IEnumerable<Product>
        }
        //public IActionResult Product()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
