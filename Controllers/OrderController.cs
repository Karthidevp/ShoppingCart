using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using ShoppingCart.Interface;
using ShoppingCart.Models;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {
        private readonly Iorderservices _orderservices;
        private readonly ICartService _cartService;
        public OrderController(Iorderservices iorderservices, ICartService cartService)
        {
            _orderservices = iorderservices;
            _cartService = cartService;
        }
        public async Task<IActionResult> GetOrderByUser(int Id)
        {
            var OrderDetails = await _orderservices.GetUserOrderDetailsAsync(Id);
            return View("UserOrderDetails",OrderDetails);
        }

        public async Task<IActionResult> GetAllOrderDetails()
        {
            var OrderDetails = await _orderservices.GetAllOrderDetailsAsync();
            return View( OrderDetails);
                       
        }
        public async Task<ActionResult> ExportOrdersToPdf()
        {
            var orderDetails = await _orderservices.GetAllOrderDetailsAsync();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 40f, 40f, 30f, 30f); // Adjust margins if needed
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add title with better styling
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph("Order Details", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f // Add spacing below the title
                };
                document.Add(title);

                // Create a table with 7 columns (removed the image column)
                var table = new PdfPTable(7)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 2, 2, 3, 2, 2, 2, 3 });

                // Add headers with background color and bold font
                AddHeaderCell(table, "Order NO");
                AddHeaderCell(table, "Username");
                AddHeaderCell(table, "Product Name");
                AddHeaderCell(table, "Price");
                AddHeaderCell(table, "Quantity");
                AddHeaderCell(table, "Total Amount");
                AddHeaderCell(table, "Ordered Date");

                // Add rows with improved formatting
                foreach (var item in orderDetails)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.OrderNO.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.Username)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(item.ProductName)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(item.Price.ToString("C"))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(item.quantity.ToString())) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(item.TotalAmount.ToString("C"))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(item.OrderedDatetime.ToString("g"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                document.Add(table);
                document.Close();

                var fileBytes = memoryStream.ToArray();
                var fileName = "OrderDetails.pdf";
                return File(fileBytes, "application/pdf", fileName);
            }
        }

        // Helper method to add header cells
        private void AddHeaderCell(PdfPTable table, string text)
        {
            var cell = new PdfPCell(new Phrase(text, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
            {
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5f
            };
            table.AddCell(cell);
        }

        public IActionResult GeneratePDF()
        {
            return null;
        }

        //public IActionResult GeneratePDF()
        //{
        //    var orders = GetOrders();  // Fetch data for the PDF (same as used in the view)
        //    return new ViewAsPdf("Index", orders)  // "Index" is the name of the view to convert to PDF
        //    {
        //        FileName = "OrderDetails.pdf"
        //    };
        //}

        //private List<Order> GetOrders()
        //{
        //    // Replace this with your actual data fetching logic
        //    return new List<Order>
        //{
        //    new Order { OrderNO = "001", Username = "JohnDoe", ProductName = "Product1", Price = 100, Quantity = 2, TotalAmount = 200, OrderedDatetime = DateTime.Now },
        //    // Add more sample orders here
        //};
        //}

        public async Task<IActionResult> UserOrderDetails()
        {
            var UserId =Convert.ToInt32( HttpContext.Session.GetInt32("UserId"));
            var OrderDetails = await _orderservices.GetUserOrderDetailsAsync(UserId);
            return View(OrderDetails);
        }
       
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] List<CartItem> cartItems)
        {
            try
            {
                var UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                foreach (var item in cartItems)
                {
                    var Orders = new Orders();
                    Orders.Userid = UserId;
                   Orders.cartid   =item.CartId;
                    Orders.Quantity=item.Quantity;
                   Orders.Totalamout = item.TotalAmount;
                 var product = await _cartService.GetProductByCartIdAsync(Orders.cartid);
                    //var Product = new Product();
                   Orders.productid= product.ProductId;
                    await _orderservices.PlaceOrderAsync(Orders);
                }
                                               
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}
