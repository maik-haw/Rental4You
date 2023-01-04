using Microsoft.AspNetCore.Mvc.Rendering;
using Rental4You.Models;
using Rental4You.Validations;
using System.ComponentModel.DataAnnotations;

namespace Rental4You.ViewModels
{
    public class ReservationsSearch
    {
        [Display(Name = "Search Results")]
        public List<Reservation>? SearchResults { get; set; }

        [Display(Name = "Number of search results")]
        public int NumberResults { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Search by Pickup Date")]
        public DateTime? PickupDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Search by Delivery Date")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Search by Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Search by Vehicle")]
        public int? VehicleId { get; set; }

        [Display(Name = "Search by Client")]
        public string? ClientId { get; set; }
    }
}
