using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string? Description { get; set; }
        public int Seats { get; set; }

        [Display(Name = "Kilometres")]
        public double Kms { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Display(Name = "Price per hour")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Cost { get; set; }

        // Foreign Keys:
        [Display(Name = "Category")]
        public int VehicleCategoryId { get; set; }
        [Display(Name = "Category")]
        public VehicleCategory? VehicleCategory { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Company")]
        public Company? Company { get; set; }
        public IEnumerable<Reservation>? Reservations { get; set; }
    }
}
