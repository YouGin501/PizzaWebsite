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
    public class PizzaCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PizzaCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PizzaCategories
        public async Task<IActionResult> Index()
        {
              return View(await _context.PizzaCategory.ToListAsync());
        }

        // GET: PizzaCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PizzaCategory == null)
            {
                return NotFound();
            }

            var pizzaCategory = await _context.PizzaCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaCategory == null)
            {
                return NotFound();
            }

            return View(pizzaCategory);
        }

        // GET: PizzaCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PizzaCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] PizzaCategory pizzaCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaCategory);
        }

        // GET: PizzaCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PizzaCategory == null)
            {
                return NotFound();
            }

            var pizzaCategory = await _context.PizzaCategory.FindAsync(id);
            if (pizzaCategory == null)
            {
                return NotFound();
            }
            return View(pizzaCategory);
        }

        // POST: PizzaCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] PizzaCategory pizzaCategory)
        {
            if (id != pizzaCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaCategoryExists(pizzaCategory.Id))
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
            return View(pizzaCategory);
        }

        // GET: PizzaCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PizzaCategory == null)
            {
                return NotFound();
            }

            var pizzaCategory = await _context.PizzaCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaCategory == null)
            {
                return NotFound();
            }

            return View(pizzaCategory);
        }

        // POST: PizzaCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PizzaCategory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PizzaCategory'  is null.");
            }
            var pizzaCategory = await _context.PizzaCategory.FindAsync(id);
            if (pizzaCategory != null)
            {
                _context.PizzaCategory.Remove(pizzaCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaCategoryExists(int id)
        {
          return _context.PizzaCategory.Any(e => e.Id == id);
        }
    }
}
