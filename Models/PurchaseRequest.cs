namespace ShoppingCart.Models
{
    public class PurchaseRequest
    {

        public int CartId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
