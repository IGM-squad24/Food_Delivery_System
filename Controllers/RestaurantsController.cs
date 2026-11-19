using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Data;
using FoodDeliverySystem.Models;

namespace FoodDeliverySystem.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index(string searchString)
        {
            var restaurants = from r in _context.Restaurants select r;

            if (!string.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(r => r.Name.Contains(searchString) || r.Address.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await restaurants.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.MenuItems)
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (restaurant == null) return NotFound();
            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone,IsActive,OpeningTime,ClosingTime")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Restaurant created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Phone,IsActive,OpeningTime,ClosingTime")] Restaurant restaurant)
        {
            if (id != restaurant.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Restaurant updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null) return NotFound();
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null) _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Restaurant deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
