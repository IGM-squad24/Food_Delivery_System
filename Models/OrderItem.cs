using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliverySystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        // Foreign keys
        [Display(Name = "Order")]
        public int OrderId { get; set; }

        [Display(Name = "Menu Item")]
        public int MenuItemId { get; set; }

        // Navigation properties
        public Order Order { get; set; } = null!;
        public MenuItem MenuItem { get; set; } = null!;
    }
}
