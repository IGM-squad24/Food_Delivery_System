using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliverySystem.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Item Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Image URL")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Foreign key
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        // Navigation properties
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
