using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class Topping
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Pizza>? Pizzas { get; set; }
    }
}
