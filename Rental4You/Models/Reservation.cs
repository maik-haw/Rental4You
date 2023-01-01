using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
        // TODO: Change status for enum
        public string Status { get; set; }
        // Foreign Keys:
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int PickupId { get; set; }
        public Pickup? Pickup { get; set; }
        public int DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }
    }
}
