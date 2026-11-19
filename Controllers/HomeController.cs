using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Data;
using FoodDeliverySystem.Models;
using System.Diagnostics;

namespace FoodDeliverySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["TotalOrders"] = await _context.Orders.CountAsync();
            ViewData["PendingOrders"] = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.Preparing);
            ViewData["TotalRestaurants"] = await _context.Restaurants.CountAsync(r => r.IsActive);
            ViewData["TotalCustomers"] = await _context.Customers.CountAsync();
            ViewData["TotalDrivers"] = await _context.DeliveryDrivers.CountAsync(d => d.IsAvailable);
            ViewData["TotalRevenue"] = await _context.Orders.Where(o => o.Status == OrderStatus.Delivered).SumAsync(o => o.TotalAmount);

            var recentOrders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            return View(recentOrders);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
