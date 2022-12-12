namespace Rental4You.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        // TODO: status probably should be an enum?
        public string? Status { get; set; }
        // Foreign Keys:
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        // TODO: Add Customer ID
        // public string ApplicationUserId { get; set; }
        // public ApplicationUser ApplicationUser { get; set; }
        public int? PickupId { get; set; }
        public Pickup? Pickup { get; set; }
        public int? DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
    }
}
