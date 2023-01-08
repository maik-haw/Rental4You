using Rental4You.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Rental4You.ViewModels
{
    public class DeliveryViewModel
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
        public IEnumerable<IFormFile>? DeliveryImages { get; set; }
        public string? EmployeeId { get; set; }
        public ApplicationUser? Employee { get; set; }
    }
}
