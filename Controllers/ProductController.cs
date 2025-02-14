using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interface;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private string Imagename = "";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductServices _productServices;
        public ProductController(IWebHostEnvironment webHostEnvironment, IProductServices productServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _productServices = productServices;
        }

        public async Task<IActionResult> ProductList()
        {
            var products = await _productServices.GetAllProductsAsync();
            return View(products); // Make sure products is of type IEnumerable<Product>
        }
        public async Task<IActionResult> ViewProductDetails(int id)
        {
            var product =await _productServices.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            string StringFileName = UploadFile(product);
            // Optionally, store the filename in the product model or database
            product.ProductImage = StringFileName; // Ensure your Product model has this property

            // Save the product to the database (this is a placeholder; implement as needed)
            await _productServices.InsertOrUpdateProductAsync(product);
            // await _context.SaveChangesAsync();

            TempData["success"] = "Product created successfully!";
            return View(); // Redirect to a page showing all products or a confirmation page
        }
        private string UploadFile(Product product)
        {
            string FileName = "";
            if (product.ProductImagefile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                FileName = Guid.NewGuid().ToString() + "_" + product.ProductImagefile.FileName;
                string filepath = Path.Combine(uploadDir, FileName);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    product.ProductImagefile.CopyTo(filestream);
                }
            }
            return FileName;

        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);
            Imagename = product.ProductImage;
            HttpContext.Session.SetString("IMGSRC", Imagename);
            return View(product);
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                string ImageFileName = HttpContext.Session.GetString("IMGSRC");
                if ( product.ProductImagefile != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string FileName = ImageFileName;
                    string filepath = Path.Combine(uploadDir, FileName);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);

                    }
                    string StringFileName = UpdateImageFile(product);

                    product.ProductImage = StringFileName;
                    await _productServices.InsertOrUpdateProductAsync(product);

                    return RedirectToAction("ProductList","Product");
                }
                else
                {
                    product.ProductImage = ImageFileName;
                    await _productServices.InsertOrUpdateProductAsync(product);
                    return RedirectToAction("ProductList", "Product");
                }
            }

            return View();
        }
            private string UpdateImageFile(Product product)
            {
                string FileName = string.Empty;
                if (product.ProductImagefile != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    FileName = Guid.NewGuid().ToString() + "_" + product.ProductImagefile.FileName;
                    string filepath = Path.Combine(uploadDir, FileName);
                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                    product.ProductImagefile.CopyTo(filestream);
                    }
                }
                return FileName;
            }
        
        public async Task<IActionResult> DeleteProduct(int id)
        {
           var result= await _productServices.DeleteProductAsync(id);
            return RedirectToAction("ProductList");

        }
        public async Task<IActionResult> ActivateProduct(int id)
        {

            await _productServices.ActivateProductAsync(id);

            TempData["success"] = "The Product was Activated successfully";
            return RedirectToAction("ProductList");

        }
        public async Task<IActionResult> InActivateProduct(int id)
        {

            await _productServices.InActivateProductAsync(id);

            TempData["success"] = "The Product was DeActivated successfully";
            return RedirectToAction("ProductList");

        }

        public async Task<IActionResult> AddToCart(int id)
        {

            await _productServices.AddToCartProductAsync(id);

            TempData["success"] = "The Product was DeActivated successfully";
            return RedirectToAction("ProductList");

        }



    }
    
 }

