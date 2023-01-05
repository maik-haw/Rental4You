using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }
        public double? Kms { get; set; }
        public string? Damage { get; set; }
        public string? Remarks { get; set; }
        // Foreign Keys:
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public IEnumerable<DeliveryImage>? DeliveryImages { get; set; }
        public string? EmployeeId { get; set; }
        public ApplicationUser? Employee { get; set; }
    }
}
