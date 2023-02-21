using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Models;

namespace PizzaWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<PizzaCategory> Categories { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<Pizza>("Pizza")
                .HasValue<Drink>("Drink");

            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.Toppings)
                .WithMany(t => t.Pizzas);
            modelBuilder.Entity<Topping>()
                .HasMany(t => t.Pizzas)
                .WithMany(p => p.Toppings);
        }
        public DbSet<PizzaWebsite.Models.PizzaCategory> PizzaCategory { get; set; }
    }
}