using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        public string? Name { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Display(Name = "Rating")]
        public double Rating { get; set; }
        // Foreign Keys:
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<ApplicationUser>? Employees { get; set; }
    }
}
