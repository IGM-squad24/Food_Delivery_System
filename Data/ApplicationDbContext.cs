using Microsoft.EntityFrameworkCore;
using FoodDeliverySystem.Models;

namespace FoodDeliverySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Restaurant configuration
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // MenuItem configuration
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasOne(m => m.Restaurant)
                      .WithMany(r => r.MenuItems)
                      .HasForeignKey(m => m.RestaurantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // DeliveryDriver configuration
            modelBuilder.Entity<DeliveryDriver>(entity =>
            {
                entity.HasIndex(e => e.LicenseNumber).IsUnique();
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.OrderDate);
                entity.HasIndex(e => e.Status);
                
                entity.HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Restaurant)
                      .WithMany(r => r.Orders)
                      .HasForeignKey(o => o.RestaurantId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.DeliveryDriver)
                      .WithMany(d => d.Orders)
                      .HasForeignKey(o => o.DeliveryDriverId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.MenuItem)
                      .WithMany(mi => mi.OrderItems)
                      .HasForeignKey(oi => oi.MenuItemId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Restaurants
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 1, Name = "Pizza Palace", Address = "123 Main St, Downtown", Phone = "555-0101", IsActive = true, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(23, 0, 0) },
                new Restaurant { Id = 2, Name = "Burger Barn", Address = "456 Oak Ave, Midtown", Phone = "555-0102", IsActive = true, OpeningTime = new TimeSpan(11, 0, 0), ClosingTime = new TimeSpan(22, 0, 0) },
                new Restaurant { Id = 3, Name = "Sushi Spot", Address = "789 Pine Rd, Uptown", Phone = "555-0103", IsActive = true, OpeningTime = new TimeSpan(12, 0, 0), ClosingTime = new TimeSpan(21, 30, 0) }
            );

            // Seed Menu Items
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Name = "Margherita Pizza", Description = "Classic tomato, mozzarella, and basil", Price = 12.99m, Category = "Pizza", IsAvailable = true, RestaurantId = 1 },
                new MenuItem { Id = 2, Name = "Pepperoni Pizza", Description = "Loaded with pepperoni and cheese", Price = 14.99m, Category = "Pizza", IsAvailable = true, RestaurantId = 1 },
                new MenuItem { Id = 3, Name = "Garlic Bread", Description = "Crispy garlic bread with herbs", Price = 5.99m, Category = "Sides", IsAvailable = true, RestaurantId = 1 },
                new MenuItem { Id = 4, Name = "Classic Burger", Description = "Beef patty with lettuce, tomato, onion", Price = 10.99m, Category = "Burgers", IsAvailable = true, RestaurantId = 2 },
                new MenuItem { Id = 5, Name = "Cheese Burger", Description = "Classic burger with cheddar cheese", Price = 12.49m, Category = "Burgers", IsAvailable = true, RestaurantId = 2 },
                new MenuItem { Id = 6, Name = "French Fries", Description = "Golden crispy fries", Price = 4.99m, Category = "Sides", IsAvailable = true, RestaurantId = 2 },
                new MenuItem { Id = 7, Name = "California Roll", Description = "Crab, avocado, cucumber roll", Price = 8.99m, Category = "Sushi Rolls", IsAvailable = true, RestaurantId = 3 },
                new MenuItem { Id = 8, Name = "Salmon Nigiri", Description = "Fresh salmon over seasoned rice", Price = 6.99m, Category = "Nigiri", IsAvailable = true, RestaurantId = 3 },
                new MenuItem { Id = 9, Name = "Miso Soup", Description = "Traditional Japanese miso soup", Price = 3.99m, Category = "Soups", IsAvailable = true, RestaurantId = 3 }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "John Smith", Email = "john@example.com", Phone = "555-1001", Address = "100 Elm St, Apt 4A", CreatedAt = new DateTime(2024, 1, 15) },
                new Customer { Id = 2, Name = "Jane Doe", Email = "jane@example.com", Phone = "555-1002", Address = "200 Maple Dr, Suite 2B", CreatedAt = new DateTime(2024, 2, 20) },
                new Customer { Id = 3, Name = "Bob Wilson", Email = "bob@example.com", Phone = "555-1003", Address = "300 Cedar Ln", CreatedAt = new DateTime(2024, 3, 10) }
            );

            // Seed Delivery Drivers
            modelBuilder.Entity<DeliveryDriver>().HasData(
                new DeliveryDriver { Id = 1, Name = "Mike Johnson", Phone = "555-2001", VehicleType = "Motorcycle", LicenseNumber = "DL-001", IsAvailable = true },
                new DeliveryDriver { Id = 2, Name = "Sarah Lee", Phone = "555-2002", VehicleType = "Car", LicenseNumber = "DL-002", IsAvailable = true },
                new DeliveryDriver { Id = 3, Name = "Tom Brown", Phone = "555-2003", VehicleType = "Bicycle", LicenseNumber = "DL-003", IsAvailable = false }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderDate = new DateTime(2024, 6, 1, 12, 30, 0), Status = OrderStatus.Delivered, TotalAmount = 33.97m, DeliveryAddress = "100 Elm St, Apt 4A", CustomerId = 1, RestaurantId = 1, DeliveryDriverId = 1, EstimatedDeliveryTime = new DateTime(2024, 6, 1, 13, 15, 0), ActualDeliveryTime = new DateTime(2024, 6, 1, 13, 10, 0) },
                new Order { Id = 2, OrderDate = new DateTime(2024, 6, 2, 18, 0, 0), Status = OrderStatus.Preparing, TotalAmount = 27.47m, DeliveryAddress = "200 Maple Dr, Suite 2B", CustomerId = 2, RestaurantId = 2, DeliveryDriverId = null, EstimatedDeliveryTime = new DateTime(2024, 6, 2, 18, 45, 0) },
                new Order { Id = 3, OrderDate = new DateTime(2024, 6, 3, 19, 30, 0), Status = OrderStatus.Pending, TotalAmount = 19.97m, DeliveryAddress = "300 Cedar Ln", CustomerId = 3, RestaurantId = 3, DeliveryDriverId = null }
            );

            // Seed Order Items
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, Quantity = 1, UnitPrice = 12.99m, Subtotal = 12.99m, OrderId = 1, MenuItemId = 1 },
                new OrderItem { Id = 2, Quantity = 1, UnitPrice = 14.99m, Subtotal = 14.99m, OrderId = 1, MenuItemId = 2 },
                new OrderItem { Id = 3, Quantity = 1, UnitPrice = 5.99m, Subtotal = 5.99m, OrderId = 1, MenuItemId = 3 },
                new OrderItem { Id = 4, Quantity = 2, UnitPrice = 10.99m, Subtotal = 21.98m, OrderId = 2, MenuItemId = 4 },
                new OrderItem { Id = 5, Quantity = 1, UnitPrice = 4.99m, Subtotal = 4.99m, OrderId = 2, MenuItemId = 6 },
                new OrderItem { Id = 6, Quantity = 1, UnitPrice = 8.99m, Subtotal = 8.99m, OrderId = 3, MenuItemId = 7 },
                new OrderItem { Id = 7, Quantity = 1, UnitPrice = 6.99m, Subtotal = 6.99m, OrderId = 3, MenuItemId = 8 },
                new OrderItem { Id = 8, Quantity = 1, UnitPrice = 3.99m, Subtotal = 3.99m, OrderId = 3, MenuItemId = 9 }
            );
        }
    }
}
