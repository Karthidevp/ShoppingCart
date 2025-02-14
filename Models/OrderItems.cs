namespace ShoppingCart.Models
{
    public class OrderItems
    {
        public int OrderNO   { get; set; }
        public string? Username { get; set; }

        public string? ProductName { get; set; }

        public decimal Price { get; set; }

        public string? ProductImage { get; set; }

        public int quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderedDatetime { get; set; }
    }
}
