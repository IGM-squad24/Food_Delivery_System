using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Data;
using FoodDeliverySystem.Models;

namespace FoodDeliverySystem.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(string searchString, string category, int? restaurantId)
        {
            var menuItems = _context.MenuItems.Include(m => m.Restaurant).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                menuItems = menuItems.Where(m => m.Name.Contains(searchString) || m.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                menuItems = menuItems.Where(m => m.Category == category);
            }

            if (restaurantId.HasValue)
            {
                menuItems = menuItems.Where(m => m.RestaurantId == restaurantId.Value);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentRestaurant"] = restaurantId;
            ViewData["Categories"] = new SelectList(await _context.MenuItems.Select(m => m.Category).Distinct().ToListAsync());
            ViewData["Restaurants"] = new SelectList(await _context.Restaurants.ToListAsync(), "Id", "Name", restaurantId);

            return View(await menuItems.OrderBy(m => m.Restaurant.Name).ThenBy(m => m.Category).ToListAsync());
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null) return NotFound();
            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name");
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Category,IsAvailable,ImageUrl,RestaurantId")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Menu item created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", menuItem.RestaurantId);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", menuItem.RestaurantId);
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Category,IsAvailable,ImageUrl,RestaurantId")] MenuItem menuItem)
        {
            if (id != menuItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Menu item updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", menuItem.RestaurantId);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var menuItem = await _context.MenuItems.Include(m => m.Restaurant).FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null) return NotFound();
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null) _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Menu item deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
