using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using Rental4You.ViewModels;
using System.Diagnostics;

namespace Rental4You.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var vehicles = _context.Vehicles
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToList();

            var vehiclesSearch = new VehiclesSearch()
            {
                VehiclesList = vehicles,
                NumberResults = vehicles.Count,
                CategoriesToSearch = new SelectList(_context.VehicleCategories, "Id", "Name")
            };

            return View(vehiclesSearch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(
            [Bind("LocationToSearch, SelectedCategories, DeliveryDateToSearch, PickupDateToSearch")]
            VehiclesSearch vehiclesSearch)
        {
            var vehicleList = await _context.Vehicles.Include(x => x.Company).Include(x => x.VehicleCategory).ToListAsync();
            ModelState.Remove(nameof(VehiclesSearch.VehiclesList));
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(vehiclesSearch.LocationToSearch))
                {
                    vehicleList = vehicleList
                        .Where(x => x.Location.Contains(vehiclesSearch.LocationToSearch, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (vehiclesSearch.SelectedCategories != null && vehiclesSearch.SelectedCategories.Count() > 0)
                {
                    vehicleList = vehicleList
                        .Where(x => x.VehicleCategory == null || vehiclesSearch.SelectedCategories.Contains(x.VehicleCategoryId.ToString()))
                        .ToList();
                }

                if (vehiclesSearch.PickupDateToSearch.HasValue && vehiclesSearch.DeliveryDateToSearch.HasValue)
                {
                    var reservations = _context.Reservations.Include(x => x.Pickup).Include(x => x.Delivery).ToList();
                    vehicleList = vehicleList.Where(v =>
                        !reservations.Any(r =>
                            r.VehicleId == v.Id &&
                            (r.Pickup.PickupDate < vehiclesSearch.DeliveryDateToSearch &&
                            r.Pickup.PickupDate >= vehiclesSearch.PickupDateToSearch) ||
                            (r.Delivery.DeliveryDate <= vehiclesSearch.DeliveryDateToSearch &&
                            r.Delivery.DeliveryDate > vehiclesSearch.PickupDateToSearch) ||
                            (r.Pickup.PickupDate <= vehiclesSearch.PickupDateToSearch &&
                            r.Delivery.DeliveryDate >= vehiclesSearch.DeliveryDateToSearch)))
                        .ToList();
                }
            }

            vehiclesSearch.VehiclesList = vehicleList;
            vehiclesSearch.NumberResults = vehiclesSearch.VehiclesList.Count;
            vehiclesSearch.CategoriesToSearch = new SelectList(_context.VehicleCategories.ToList(), "Id", "Name");


            return View("Index", vehiclesSearch);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}