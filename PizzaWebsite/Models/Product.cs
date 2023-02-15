using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Models
{
    public abstract class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string? Image { get; set; }

        public virtual List<CartItem>? CartItems { get; set; } 
    }
}
