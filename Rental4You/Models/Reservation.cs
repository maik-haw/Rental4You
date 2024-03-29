﻿using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public enum ReservationStatus
    {
        open,
        confirmed,
        rejected,
        closed,
        pickedUp,
        delivered
    }

    public class Reservation
    {
        public int Id { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.open;
        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
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
