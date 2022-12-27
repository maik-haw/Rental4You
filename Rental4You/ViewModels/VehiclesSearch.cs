using Microsoft.AspNetCore.Mvc.Rendering;
using Rental4You.Models;
using Rental4You.Validations;
using System.ComponentModel.DataAnnotations;

namespace Rental4You.ViewModels
{
    public class VehiclesSearch
    {
        public List<Vehicle> VehiclesList { get; set; }
        public int NumberResults { get; set; }
        public string? LocationToSearch { get; set; }
        public SelectList? CategoriesToSearch { get; set; }
        public List<String>? SelectedCategories { get; set; }
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime? PickupDateToSearch { get; set; }
        [DataType(DataType.Date)]
        [FutureDate]
        [DeliveryDateAttribute("PickupDateToSearch")]
        public DateTime? DeliveryDateToSearch { get; set; }
    }
}
