using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers
{
    public class StatInfo
    {
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<StatInfo> DelProductFromCart(int productId)
        {
            var currentUser = await _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).FirstAsync();

            var currentUserCartItem = await _context.CartItems
                .Include(x => x.ApplicationUser)
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(x => x.ApplicationUser.UserName == User.Identity.Name && x.ProductId == productId)
                .FirstAsync();

            if (currentUserCartItem.Quantity == 1)
                _context.CartItems.Remove(currentUserCartItem);
            else if(currentUserCartItem.Quantity > 1)
            {
                currentUserCartItem.Quantity -= 1;
                _context.Entry(currentUserCartItem).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return await GetStatInfo();
        }

        [HttpPost]
        public async Task<StatInfo> AddProductToCart(int productId)
        {
            var currentUser = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            //err
            var currentUserCartItem = await _context.CartItems 
                .Include(x => x.ApplicationUser)
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(x => x.ApplicationUser.UserName == User.Identity.Name && x.ProductId == productId)
                .FirstOrDefaultAsync();

            if (currentUserCartItem == null)
                _context.CartItems.Add(new CartItem()
                {
                    Id = 0,
                    Quantity = 1,
                    ProductId = productId,
                    UserId = currentUser.Id
                });
            else
            {
                currentUserCartItem.Quantity += 1;
                _context.Entry(currentUserCartItem).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return await GetStatInfo();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<StatInfo> GetStatInfo()
        {
            var currentUser = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return new StatInfo() { Count = 0, Amount = 0.0M };
            else
            {
                int count = 0;
                decimal amount = 0.0M;

                var currentUserCartItems = await _context.CartItems
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Product)
                    .Where(x => x.ApplicationUser.UserName == User.Identity.Name)
                    .ToListAsync();

                foreach (var cartItem in currentUserCartItems)
                {
                    count += cartItem.Quantity;
                    amount += (cartItem.Product.Price) * (cartItem.Quantity);
                }

                return new StatInfo() { Count = count, Amount = amount };
            }
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CartItems.Include(c => c.ApplicationUser).Include(c => c.Order).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.ApplicationUser)
                .Include(c => c.Order)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "OrderNumber");
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Name");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,OrderId,ProductId,UserId")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "OrderNumber", cartItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "OrderNumber", cartItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,OrderId,ProductId,UserId")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "OrderNumber", cartItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.ApplicationUser)
                .Include(c => c.Order)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartItems'  is null.");
            }
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
          return _context.CartItems.Any(e => e.Id == id);
        }
    }
}
