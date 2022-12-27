using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Rental4You.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private ApplicationUser GetCurrentUser()
        {
            var user = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.Company)
                .FirstOrDefault();
            return user;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(bool? active, int? category)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name");

            if(category == null && active != null)
                return View(await _context.Vehicles
                    .Where(v => v.CompanyId == user.CompanyId && v.IsActive == active)
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToListAsync());
            else if(category != null && active == null)
                return View(await _context.Vehicles
                    .Where(v => v.CompanyId == user.CompanyId && v.VehicleCategoryId == category)
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToListAsync());
            else if (category != null && active != null)
                return View(await _context.Vehicles
                    .Where(v => v.CompanyId == user.CompanyId && v.VehicleCategoryId == category && v.IsActive == active)
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToListAsync());
            else
                return View(await _context.Vehicles
                    .Where(v => v.CompanyId == user.CompanyId)
                    .Include(v => v.Company)
                    .Include(v => v.VehicleCategory)
                    .ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Company)
                .Include(v => v.VehicleCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model,Description,Seats,Kms,IsActive,Location,Cost,VehicleCategoryId,CompanyId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var employee = GetCurrentUser();
                if(employee.CompanyId == null)
                {
                    TempData["Error"] = "Current user is not associated to a company. Cannot create vehicle.";
                    return RedirectToAction("Index");
                }
                vehicle.CompanyId = (int) employee.CompanyId;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Id", vehicle.VehicleCategoryId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name", vehicle.VehicleCategoryId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Description,Seats,Kms,IsActive,Location,Cost,VehicleCategoryId,CompanyId")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name", vehicle.VehicleCategoryId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Company)
                .Include(v => v.VehicleCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
            }

            var existingReservations = from r in _context.Reservations
                                       where r.VehicleId == id
                                       select r;
            if (existingReservations != null && existingReservations.Count() > 0)
            {
                // Reservations associated to vehicle exist -> do not delete!
                TempData["Error"] = "Vehicle cannot be deleted, because Reservations exist.";
                return RedirectToAction("Index");
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
          return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
