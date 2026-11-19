using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        [Display(Name = "Delivery Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Date Registered")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
