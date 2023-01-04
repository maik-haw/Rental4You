using Rental4You.Models;
using Rental4You.Validations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Rental4You.ViewModels
{
    public class ReservationsVM
    {

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        [DataType(DataType.Date)]
        [FutureDate]
        [Display(Name = "Pickup Date")]
        public DateTime PickupDate { get; set; }
        [DataType(DataType.Date)]
        [FutureDate]
        [Display(Name = "Delivery Date")]
        [DeliveryDateAttribute("PickupDate")]
        public DateTime DeliveryDate { get; set; }
    }
}
