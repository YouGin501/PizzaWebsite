using Microsoft.AspNetCore.Mvc;
using PizzaWebsite.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Models
{
    public class Pizza : Product
    {
        [Required]
        public int Weight { get; set; }

        [Required]
        public virtual List<Topping>? Toppings { get; set; }

        [Required]
        public int PizzaCategoryId { get; set; }
        [ForeignKey("PizzaCategoryId")]
        public virtual PizzaCategory? PizzaCategory { get; set; }
    }
}
