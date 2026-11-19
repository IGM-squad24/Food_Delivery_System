using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Data;
using FoodDeliverySystem.Models;

namespace FoodDeliverySystem.Controllers
{
    public class DeliveryDriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryDriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryDrivers
        public async Task<IActionResult> Index(string searchString)
        {
            var drivers = from d in _context.DeliveryDrivers select d;

            if (!string.IsNullOrEmpty(searchString))
            {
                drivers = drivers.Where(d => d.Name.Contains(searchString) || d.LicenseNumber.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await drivers.ToListAsync());
        }

        // GET: DeliveryDrivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var driver = await _context.DeliveryDrivers
                .Include(d => d.Orders)
                    .ThenInclude(o => o.Customer)
                .Include(d => d.Orders)
                    .ThenInclude(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (driver == null) return NotFound();
            return View(driver);
        }

        // GET: DeliveryDrivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryDrivers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,VehicleType,LicenseNumber,IsAvailable")] DeliveryDriver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Driver registered successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: DeliveryDrivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var driver = await _context.DeliveryDrivers.FindAsync(id);
            if (driver == null) return NotFound();
            return View(driver);
        }

        // POST: DeliveryDrivers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,VehicleType,LicenseNumber,IsAvailable")] DeliveryDriver driver)
        {
            if (id != driver.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Driver updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: DeliveryDrivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var driver = await _context.DeliveryDrivers.FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null) return NotFound();
            return View(driver);
        }

        // POST: DeliveryDrivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.DeliveryDrivers.FindAsync(id);
            if (driver != null) _context.DeliveryDrivers.Remove(driver);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Driver removed successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.DeliveryDrivers.Any(e => e.Id == id);
        }
    }
}
