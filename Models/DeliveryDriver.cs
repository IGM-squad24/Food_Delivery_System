using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySystem.Models
{
    public class DeliveryDriver
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Driver Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
