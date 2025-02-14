using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Login
    {

        public int Id { get; set; }

        public string? EmailId { get; set; }


        public string? Password { get; set; }

       
    }
}
