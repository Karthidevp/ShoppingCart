namespace ShoppingCart.Models
{
    public class Users
    {
        public int UserId { get; set; }

        public string? Name { get; set; }


        public string? EmailId { get; set; }


        public string? Address { get; set; }


        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        //public string? status { get; set; }  
        
        public bool status { get; set; }

             
        public string? PhoneNo { get; set; }

        public bool Isdeleted { get; set; }
        public bool HasOrder { get; set; } // New property

    }
}
