using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers
{
    public class PizzasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PizzasController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost]
        public List<Topping> getSelectedValues(List<Topping> toppings)
        {
            List<Topping> selectedValues = toppings;
            return selectedValues;
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {
            var pizzas = _context.Pizzas.Include(p => p.Toppings);
            return View(await pizzas.ToListAsync());
        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pizzas == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // GET: Pizzas/Create
        public IActionResult Create()
        {
            List<Topping> toppings = _context.Toppings.Include(t => t.Pizzas).ToList();
            ViewData["Toppings"] = toppings;

            return View();
        }

        // POST: Pizzas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Weight,Id,Name,Price,Image,Toppings")] Pizza pizza,
            IFormFile image, int[] Toppings)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    if (!System.IO.File.Exists(name))
                        await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    pizza.Image = "Images/" + image.FileName;
                }
                else
                {
                    pizza.Image = "Images/default.png";
                }

                var allToppings = _context.Toppings.ToList();
                List<Topping> forPizza= new List<Topping>();
                foreach (var item in Toppings)
                    forPizza.Add(allToppings.First(t => t.Id == item));
                pizza.Toppings = forPizza;

                _context.Add(pizza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            }
            List<Topping> topps = _context.Toppings.Include(t => t.Pizzas).ToList();
            ViewData["Toppings"] = topps;
            return View(pizza);
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pizzas == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }
            List<Topping> toppings = _context.Toppings.Include(t => t.Pizzas).ToList();
            ViewData["Toppings"] = toppings;
            return View(pizza);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Weight,Id,Name,Price,Image,Toppings")] Pizza pizza)
        {
            if (id != pizza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaExists(pizza.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<Topping> toppings = _context.Toppings.Include(t => t.Pizzas).ToList();
            ViewData["Toppings"] = toppings;
            return View(pizza);
        }

        // GET: Pizzas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pizzas == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pizzas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pizzas'  is null.");
            }
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza != null)
            {
                _context.Pizzas.Remove(pizza);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaExists(int id)
        {
          return _context.Pizzas.Any(e => e.Id == id);
        }
    }
}
