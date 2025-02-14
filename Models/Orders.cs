namespace ShoppingCart.Models
{
    public class Orders
    {
        public int cartid { get; set; }

        public int Userid { get; set; }

        public int productid { get; set; }

        public int Quantity { get; set; }

        public decimal Totalamout { get; set; }


    }
}
