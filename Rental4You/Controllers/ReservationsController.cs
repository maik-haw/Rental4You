using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using Rental4You.ViewModels;

namespace Rental4You.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
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

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var user = GetCurrentUser();
            List<Reservation> reservations;
            // If Employee: only show reservations associated to his/her company
            if (User.IsInRole("Employee") && user.CompanyId != null)
            {
                reservations = await _context.Reservations
                    .Include(r => r.Delivery)
                    .Include(r => r.Pickup)
                    .Include(r => r.Vehicle)
                    .Where(r => r.Vehicle.CompanyId == user.CompanyId)
                    .ToListAsync();
            }
            // If Client: show his/her own reservations
            else if (User.IsInRole("Client"))
            {
                reservations = await _context.Reservations
                    .Include(r => r.Delivery)
                    .Include(r => r.Pickup)
                    .Include(r => r.Vehicle)
                    .Where(r => r.ClientId == user.Id)
                    .ToListAsync();
            }
            // Else: show all reservations
            else
            {
                reservations = await _context.Reservations
                    .Include(r => r.Delivery)
                    .Include(r => r.Pickup)
                    .Include(r => r.Vehicle)
                    .ToListAsync();
            }

            var model = new ReservationsSearch()
            {
                SearchResults = reservations,
                NumberResults = reservations.Count
            };
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model");
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "UserName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(ReservationsSearch search)
        {
            List<Reservation> reservations;
            reservations = await _context.Reservations
                .Include(r => r.Delivery)
                .Include(r => r.Pickup)
                .Include(r => r.Vehicle)
                .Where(r => r.ClientId == search.ClientId)
                .Where(r => r.VehicleId == search.VehicleId)
                .Where(r => r.Vehicle.VehicleCategoryId == search.CategoryId)
                .ToListAsync();

            search.SearchResults = reservations;
            search.NumberResults = reservations.Count;

            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model");
            ViewData["VehicleCategoryId"] = new SelectList(_context.VehicleCategories, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "UserName");
            return View("Index", search);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Delivery)
                .Include(r => r.Pickup)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["DeliveryId"] = new SelectList(_context.Deliveries, "Id", "Id");
            ViewData["PickupId"] = new SelectList(_context.Pickups, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model");
            ViewData["ReservationStatus"] = new SelectList(Enum.GetValues(typeof(ReservationStatus))
                .Cast<ReservationStatus>().ToList());
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,Status,VehicleId,PickupId,DeliveryId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryId"] = new SelectList(_context.Deliveries, "Id", "Id", reservation.DeliveryId);
            ViewData["PickupId"] = new SelectList(_context.Pickups, "Id", "Id", reservation.PickupId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", reservation.VehicleId);
            ViewData["ReservationStatus"] = new SelectList(Enum.GetValues(typeof(ReservationStatus))
                .Cast<string>().ToList());
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["DeliveryId"] = new SelectList(_context.Deliveries, "Id", "Id", reservation.DeliveryId);
            ViewData["PickupId"] = new SelectList(_context.Pickups, "Id", "Id", reservation.PickupId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", reservation.VehicleId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,Status,VehicleId,PickupId,DeliveryId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["DeliveryId"] = new SelectList(_context.Deliveries, "Id", "Id", reservation.DeliveryId);
            ViewData["PickupId"] = new SelectList(_context.Pickups, "Id", "Id", reservation.PickupId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", reservation.VehicleId);
            ViewData["ReservationStatus"] = new SelectList(Enum.GetValues(typeof(ReservationStatus))
                .Cast<string>().ToList());
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Delivery)
                .Include(r => r.Pickup)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
