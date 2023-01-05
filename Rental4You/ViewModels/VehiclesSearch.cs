using Microsoft.AspNetCore.Mvc.Rendering;
using Rental4You.Models;
using Rental4You.Validations;
using System.ComponentModel.DataAnnotations;

namespace Rental4You.ViewModels
{
    public class VehiclesSearch
    {
        [Display(Name = "Search Results")]
        public List<Vehicle> VehiclesList { get; set; }

        [Display(Name = "Number of search results")]
        public int NumberResults { get; set; }

        [Display(Name = "Search by Location")]
        public string? LocationToSearch { get; set; }

        [Display(Name = "Search by Category")]
        public SelectList? CategoriesToSearch { get; set; }

        [Display(Name = "Selected Categories")]
        public List<String>? SelectedCategories { get; set; }

        [Display(Name = "Selected Category")]
        public string? SelectedCategory { get; set; }

        [DataType(DataType.Date)]
        [FutureDate]
        [Display(Name = "Pickup Date")]
        public DateTime? PickupDateToSearch { get; set; }

        [DataType(DataType.Date)]
        [FutureDate]
        [Display(Name = "Delivery Date")]
        [DeliveryDateAttribute("PickupDateToSearch")]
        public DateTime? DeliveryDateToSearch { get; set; }
        public double? Hours { get; set; }
    }
}
