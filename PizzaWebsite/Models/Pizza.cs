using Microsoft.AspNetCore.Mvc;
using PizzaWebsite.Models;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class Pizza : Product
    {
        [Required]
        public int Weight { get; set; }

        [Required]
        public virtual List<Topping> Toppings { get; set; }
    }
}
