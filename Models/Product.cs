namespace ShoppingCart.Models
{
    public class Product
    {

        public int Iscart { get; set; }
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public decimal PricePerQnt { get; set; } 
        
        public int AvailableQnty { get; set; }

        public string? ProductImage { get; set; }

        public IFormFile? ProductImagefile { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool status {get; set; }

        public bool Isdeleted { get; set; }

        public string? Description { get; set; }

        public int SoldQnt {  get; set; }
    }
}
