using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class Drink : Product
    {
        [Required]
        public int Volume { get; set; }
    }
}
