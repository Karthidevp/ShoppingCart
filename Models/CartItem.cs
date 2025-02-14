namespace ShoppingCart.Models
{
    public class CartItem
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? ProductImage { get; set; }
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int QNTY { get;set; }

        public decimal TotalAmount { get; set; } 

      
    }
}
