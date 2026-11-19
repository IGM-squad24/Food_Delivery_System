using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Data;
using FoodDeliverySystem.Models;

namespace FoodDeliverySystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(OrderStatus? status, string searchString)
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryDriver)
                .AsQueryable();

            if (status.HasValue)
            {
                orders = orders.Where(o => o.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Customer.Name.Contains(searchString)
                    || o.Restaurant.Name.Contains(searchString)
                    || o.Id.ToString().Contains(searchString));
            }

            ViewData["CurrentStatus"] = status;
            ViewData["CurrentFilter"] = searchString;
            ViewData["Statuses"] = new SelectList(Enum.GetValues(typeof(OrderStatus)));

            return View(await orders.OrderByDescending(o => o.OrderDate).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryDriver)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants.Where(r => r.IsActive), "Id", "Name");
            ViewData["DeliveryDriverId"] = new SelectList(_context.DeliveryDrivers.Where(d => d.IsAvailable), "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Status,TotalAmount,DeliveryAddress,Notes,EstimatedDeliveryTime,CustomerId,RestaurantId,DeliveryDriverId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.Status = OrderStatus.Pending;
                _context.Add(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants.Where(r => r.IsActive), "Id", "Name", order.RestaurantId);
            ViewData["DeliveryDriverId"] = new SelectList(_context.DeliveryDrivers.Where(d => d.IsAvailable), "Id", "Name", order.DeliveryDriverId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", order.RestaurantId);
            ViewData["DeliveryDriverId"] = new SelectList(_context.DeliveryDrivers, "Id", "Name", order.DeliveryDriverId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Status,TotalAmount,DeliveryAddress,Notes,EstimatedDeliveryTime,ActualDeliveryTime,CustomerId,RestaurantId,DeliveryDriverId")] Order order)
        {
            if (id != order.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.Status == OrderStatus.Delivered && order.ActualDeliveryTime == null)
                    {
                        order.ActualDeliveryTime = DateTime.Now;
                    }
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Order updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", order.RestaurantId);
            ViewData["DeliveryDriverId"] = new SelectList(_context.DeliveryDrivers, "Id", "Name", order.DeliveryDriverId);
            return View(order);
        }

        // POST: Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = newStatus;
            if (newStatus == OrderStatus.Delivered)
            {
                order.ActualDeliveryTime = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Order #{id} status updated to {newStatus}!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Order deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
