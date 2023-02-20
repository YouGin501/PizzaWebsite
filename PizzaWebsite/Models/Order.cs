using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string OrderNumber { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Required]
        public string PersonName { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }
}
