﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using Rental4You.ViewModels;

namespace Rental4You.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            List<Reservation> reservations = null;
            // If Employee: only show reservations associated to his/her company
            if ((User.IsInRole("Employee") || User.IsInRole("Manager")) && user.CompanyId != null)
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

            List<ReservationVM> reservationsVM = new List<ReservationVM>();
            foreach (var reservation in reservations){
                ReservationVM reservationVM = new ReservationVM();
                reservationVM.VehicleId = reservation.VehicleId;
                reservationVM.Vehicle = reservation.Vehicle;
                reservationVM.ReservationId = reservation.Id;
                reservationVM.CreatedAt = reservation.CreatedAt;
                reservationVM.Status = reservation.Status;
                reservationVM.PickupDate = reservation.Pickup.PickupDate;
                reservationVM.DeliveryDate = reservation.Delivery.DeliveryDate;
                reservationsVM.Add(reservationVM);
            }
            return View(reservationsVM);
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

            ReservationVM reservationVM = new ReservationVM();
            reservationVM.Vehicle = reservation.Vehicle;
            reservationVM.CreatedAt = reservation.CreatedAt;
            reservationVM.Status = reservation.Status;
            reservationVM.PickupDate = reservation.Pickup.PickupDate;
            reservationVM.DeliveryDate = reservation.Delivery.DeliveryDate;

            return View(reservationVM);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create(int? id)
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

            ReservationVM reservationVM = new ReservationVM();
            reservationVM.VehicleId = vehicle.Id;
            reservationVM.Vehicle = vehicle;

            return View(reservationVM);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,PickupDate,DeliveryDate")] ReservationVM reservationVM)
        {
            ModelState.Remove(nameof(ReservationVM.Vehicle));
            if (ModelState.IsValid)
            {
                var reservations = _context.Reservations
                    .Include(x => x.Pickup)
                    .Include(x => x.Delivery)
                    .Where(x => x.VehicleId == reservationVM.VehicleId)
                    .ToList();

                bool isAvailable = !reservations.Any(r => r.VehicleId == reservationVM.VehicleId &&
                        (r.Pickup.PickupDate < reservationVM.DeliveryDate &&
                        r.Pickup.PickupDate >= reservationVM.PickupDate) ||
                        (r.Delivery.DeliveryDate <= reservationVM.DeliveryDate &&
                        r.Delivery.DeliveryDate > reservationVM.PickupDate) ||
                        (r.Pickup.PickupDate <= reservationVM.PickupDate &&
                        r.Delivery.DeliveryDate >= reservationVM.DeliveryDate));

                if (isAvailable == true)
                {
                    Pickup pickup = new Pickup();
                    pickup.PickupDate = reservationVM.PickupDate;

                    Delivery delivery = new Delivery();
                    delivery.DeliveryDate = reservationVM.DeliveryDate;

                    Reservation reservation = new Reservation();
                    reservation.CreatedAt = DateTime.Now;
                    reservation.Status = ReservationStatus.open;
                    reservation.VehicleId = reservationVM.VehicleId;
                    reservation.ClientId = _userManager.GetUserId(User);
                    reservation.Client = GetCurrentUser();
                    reservation.Pickup = pickup;
                    reservation.PickupId = pickup.Id;
                    reservation.Delivery = delivery;
                    reservation.DeliveryId = delivery.Id;

                    pickup.Reservation = reservation;
                    pickup.ReservationId = reservation.Id;

                    delivery.Reservation = reservation;
                    delivery.ReservationId = reservation.Id;

                    _context.Add(delivery);
                    _context.Add(pickup);
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();

                    // update models' foreign keys
                    reservation.PickupId = pickup.Id;
                    reservation.DeliveryId = delivery.Id;
                    pickup.ReservationId = reservation.Id;
                    delivery.ReservationId = reservation.Id;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                ViewData["ErrorMessage"] = "Selected vehicle isn't available during selected period of time";
                return View(reservationVM);
            }
            return View(reservationVM);
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
            var pickup = await _context.Pickups.FindAsync(reservation.PickupId);
            var delivery = await _context.Deliveries.FindAsync(reservation.DeliveryId);
            if (pickup == null || delivery == null)
            {
                return NotFound();
            }

            ReservationVM reservationVM = new ReservationVM();
            reservationVM.VehicleId = reservation.VehicleId;
            reservationVM.ReservationId = reservation.Id;
            reservationVM.CreatedAt = reservation.CreatedAt;
            reservationVM.Status = reservation.Status;
            reservationVM.PickupDate = pickup.PickupDate;
            reservationVM.DeliveryDate = delivery.DeliveryDate;
            return View(reservationVM);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,VehicleId,Status,PickupDate,DeliveryDate")] ReservationVM reservationVM)
        {
            if (id != reservationVM.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reservations = _context.Reservations
                        .Include(x => x.Pickup)
                        .Include(x => x.Delivery)
                        .Where(x => x.VehicleId == reservationVM.VehicleId)
                        .ToList();

                    bool isAvailable = !reservations.Any(r => r.VehicleId == reservationVM.VehicleId &&
                       (r.Pickup.PickupDate < reservationVM.DeliveryDate &&
                       r.Pickup.PickupDate >= reservationVM.PickupDate) ||
                       (r.Delivery.DeliveryDate <= reservationVM.DeliveryDate &&
                       r.Delivery.DeliveryDate > reservationVM.PickupDate) ||
                       (r.Pickup.PickupDate <= reservationVM.PickupDate &&
                       r.Delivery.DeliveryDate >= reservationVM.DeliveryDate));

                    if (isAvailable == true)
                    {

                        var reservation = await _context.Reservations.FindAsync(reservationVM.ReservationId);
                        if (reservation != null && reservation.Status == ReservationStatus.open)
                        {
                            var pickup = await _context.Pickups.FindAsync(reservation.PickupId);
                            if (pickup != null)
                            {
                                pickup.PickupDate = reservationVM.PickupDate;
                                _context.Update(pickup);
                                await _context.SaveChangesAsync();
                            }
                            var delivery = await _context.Deliveries.FindAsync(reservation.DeliveryId);
                            if (delivery != null)
                            {
                                delivery.DeliveryDate = reservationVM.DeliveryDate;
                                _context.Update(delivery);
                                await _context.SaveChangesAsync();
                            }

                            reservation.Status = reservationVM.Status;
                        }

                        _context.Update(reservation);
                        await _context.SaveChangesAsync();
                    } else
                    {
                        ViewData["ErrorMessage"] = "Selected vehicle isn't available during selected period of time";
                        return View(reservationVM);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservationVM.ReservationId))
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
            return View(reservationVM);
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

            ReservationVM reservationVM = new ReservationVM();
            reservationVM.ReservationId = reservation.Id;
            reservationVM.CreatedAt = reservation.CreatedAt;
            reservationVM.Status = reservation.Status;
            reservationVM.VehicleId = reservation.VehicleId;
            reservationVM.Vehicle = reservation.Vehicle;
            reservationVM.PickupDate = reservation.Pickup.PickupDate;
            reservationVM.DeliveryDate = reservation.Delivery.DeliveryDate;

            return View(reservationVM);
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
            var pickup = await _context.Pickups.FindAsync(reservation.PickupId);
            if (pickup != null)
            {
                _context.Pickups.Remove(pickup);
            }
            var delivery = await _context.Deliveries.FindAsync(reservation.DeliveryId);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
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
