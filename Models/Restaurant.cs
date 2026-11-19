using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySystem.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Opening Time")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningTime { get; set; } = new TimeSpan(9, 0, 0);

        [Display(Name = "Closing Time")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingTime { get; set; } = new TimeSpan(22, 0, 0);

        // Navigation properties
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
