using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using Rental4You.ViewModels;

namespace Rental4You.Controllers
{
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
              return View(await _context.Deliveries.ToListAsync());
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: Deliveries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeliveryDate,Kms,Damage,Remarks")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.Where(p => p.Id == id)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync();
            if (delivery == null)
            {
                return NotFound();
            }
            var deliveryVM = new DeliveryViewModel
            {
                Id = delivery.Id,
                Kms = delivery.Kms,
                Damage = delivery.Damage,
                DeliveryDate = delivery.DeliveryDate,
                Employee = delivery.Employee,
                EmployeeId = delivery.EmployeeId,
                Remarks = delivery.Remarks,
                Reservation = delivery.Reservation,
                ReservationId = delivery.ReservationId
            };
            return View(deliveryVM);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeliveryDate,Kms,Damage,Remarks,ReservationId,Reservation,DeliveryImages")] DeliveryViewModel deliveryVM)
        {
            if (id != deliveryVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var delivery = new Delivery
                    {
                        Id = deliveryVM.Id,
                        Kms = deliveryVM.Kms,
                        Damage = deliveryVM.Damage,
                        DeliveryDate = deliveryVM.DeliveryDate,
                        Employee = deliveryVM.Employee,
                        EmployeeId = deliveryVM.EmployeeId,
                        Remarks = deliveryVM.Remarks,
                        Reservation = deliveryVM.Reservation,
                        ReservationId = deliveryVM.ReservationId
                    };
                    if (deliveryVM.DeliveryImages != null)
                    {
                        foreach(var img in deliveryVM.DeliveryImages)
                        {
                            if (img.Length > (1024 * 1024))
                            {
                                ModelState.AddModelError("DeliveryImages", "Error: File too big");
                                return View(deliveryVM);
                            }
                            using (var dataStream = new MemoryStream())
                            {
                                await img.CopyToAsync(dataStream);
                                var deliveryImg = new DeliveryImage
                                {
                                    ImageData = dataStream.ToArray(),
                                    DeliveryId = delivery.Id,
                                    Delivery = delivery
                                };
                                _context.Add(deliveryImg);
                                delivery.DeliveryImages.Add(deliveryImg);
                            }
                        }
                    }
                    delivery.EmployeeId = _userManager.GetUserId(User);
                    var reservation = await _context.Reservations.FindAsync(delivery.ReservationId);
                    reservation.Status = ReservationStatus.delivered;
                    _context.Update(delivery);
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(deliveryVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Reservations");
            }
            return View(deliveryVM);
        }

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deliveries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Deliveries'  is null.");
            }
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(int id)
        {
          return _context.Deliveries.Any(e => e.Id == id);
        }
    }
}
