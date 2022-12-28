using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;

namespace Rental4You.Controllers
{
    public class VehicleCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehicleCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleCategories.ToListAsync());
        }

        // GET: VehicleCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VehicleCategories == null)
            {
                return NotFound();
            }

            var vehicleCategory = await _context.VehicleCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleCategory == null)
            {
                return NotFound();
            }

            return View(vehicleCategory);
        }

        // GET: VehicleCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] VehicleCategory vehicleCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleCategory);
        }

        // GET: VehicleCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VehicleCategories == null)
            {
                return NotFound();
            }

            var vehicleCategory = await _context.VehicleCategories.FindAsync(id);
            if (vehicleCategory == null)
            {
                return NotFound();
            }
            return View(vehicleCategory);
        }

        // POST: VehicleCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] VehicleCategory vehicleCategory)
        {
            if (id != vehicleCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleCategoryExists(vehicleCategory.Id))
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
            return View(vehicleCategory);
        }

        // GET: VehicleCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VehicleCategories == null)
            {
                return NotFound();
            }

            var vehicleCategory = await _context.VehicleCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleCategory == null)
            {
                return NotFound();
            }

            return View(vehicleCategory);
        }

        // POST: VehicleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VehicleCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.VehicleCategories'  is null.");
            }
            var vehicleCategory = await _context.VehicleCategories.FindAsync(id);
            if (vehicleCategory != null)
            {
                _context.VehicleCategories.Remove(vehicleCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleCategoryExists(int id)
        {
            return _context.VehicleCategories.Any(e => e.Id == id);
        }
    }
}
