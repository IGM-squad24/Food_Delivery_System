using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliverySystem.Models
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Preparing,
        [Display(Name = "Out for Delivery")]
        OutForDelivery,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [StringLength(300)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        [Display(Name = "Estimated Delivery")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedDeliveryTime { get; set; }

        [Display(Name = "Actual Delivery")]
        [DataType(DataType.DateTime)]
        public DateTime? ActualDeliveryTime { get; set; }

        // Foreign keys
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        [Display(Name = "Driver")]
        public int? DeliveryDriverId { get; set; }

        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public DeliveryDriver? DeliveryDriver { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
