using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;

namespace Rental4You.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Companies
        public async Task<IActionResult> Index(bool? active)
        {
            if (active == null)
                return View(await _context.Companies
                    .ToListAsync());
            else
                return View(await _context.Companies
                    .Where(c => c.IsActive == active)
                    .ToListAsync());
        }

        // GET: Companies/Search
        [HttpGet]
        public async Task<IActionResult> Search(string? searchText)
        {
            if (searchText == null)
            {
                var allCompanies = await _context.Companies
                    .ToListAsync();
                return View("Index", allCompanies);
            }
            else
            {
                var searchResults = await _context.Companies
                    .Where(c => c.Name.Contains(searchText))
                    .ToListAsync();
                return View("Index", searchResults);
            }
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EMail,IsActive,Rating")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                var savedCompany = await _context.Companies
                    .Where(c => c.Name == company.Name && c.EMail == company.EMail)
                    .FirstOrDefaultAsync();
                var companyManager = new ApplicationUser
                {
                    UserName = company.EMail,
                    Email = company.EMail,
                    FirstName = "Manager",
                    LastName = company.Name,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CompanyId = savedCompany.Id,
                    Company = savedCompany
                };
                await _userManager.CreateAsync(companyManager, "Manager...123");
                await _userManager.AddToRoleAsync(companyManager, Roles.Manager.ToString());
                TempData["Info"] = String.Format(
                    "Company '{0}' was created. " +
                    "The Manager can now login with Username '{1}' and Password 'Manager...123'. " +
                    "Please consider changing the default password!",
                    company.Name, company.EMail);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EMail,IsActive,Rating")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if(company != null)
            {
                var existingVehicles = from c in _context.Companies
                        where c.Id == id
                        join v in _context.Vehicles on c.Id equals v.CompanyId
                        select c;
                if (existingVehicles != null && existingVehicles.Count() > 0)
                {
                    // Vehicles associated to company exist -> do not delete!
                    TempData["Error"] = "Company cannot be deleted, because vehicles are registered. Please delete them first!";
                    return View(company);
                } else
                {
                    // Remove all Users associated with the company
                    var users = await _context.Users.Where(u => u.CompanyId == company.Id).ToListAsync();
                    foreach (var user in users) {
                        _context.Users.Remove(user);
                    }
                    // No vehicles associated to company exist -> delete!
                    _context.Companies.Remove(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return _context.Companies.Any(e => e.Id == id);
        }
    }
}
