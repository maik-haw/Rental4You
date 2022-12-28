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
        public IActionResult Search(VehiclesSearch search)
        {
            List<Vehicle> vehicles;

            // TODO: Select vehicles, dependent on search (currently only by location)
            vehicles = _context.Vehicles
                    .Where(v => v.Location.Contains(search.LocationToSearch == null ? "" : search.LocationToSearch))
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToList();
            search.VehiclesList = vehicles;
            search.NumberResults = vehicles.Count;
            // TODO: This SelectList will be rendered as "multiple select", but I don't know why!
            search.CategoriesToSearch = new SelectList(_context.VehicleCategories, "Id", "Name");

            return View("Index", search);
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