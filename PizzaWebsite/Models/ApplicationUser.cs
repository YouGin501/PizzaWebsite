using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public List<Order>? Orders { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
    }
}
