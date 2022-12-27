using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental4You.Data;
using Rental4You.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Rental4You.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var manager = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            if (manager == null)
            {
                return RedirectToAction("Index");
            }
            var users = await _context.Users
                .Where(u => u.CompanyId != null && u.CompanyId == manager.CompanyId)
                .ToListAsync();
            return View(users);
        }

        // GET: UserManager/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UserManager/Create
        public IActionResult Create()
        {
            var manager = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (manager == null || manager.CompanyId == null)
            {
                return NotFound();
            }
            var newEmployee = new CreateEmployeeViewModel
            {
                CompanyId = (int) manager.CompanyId,
                Role = Roles.Employee.ToString()
            };
            ViewData["Roles"] = new SelectList(new String[] { "Employee", "Manager" });
            return View(newEmployee);
        }

        // POST: UserManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,EMail,FirstName,LastName,Password,Role,BirthDate,UserAvatar,CompanyId")] CreateEmployeeViewModel createUser)
        {
            if (ModelState.IsValid)
            {
                var manager = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
                if (createUser.UserAvatar != null && createUser.UserAvatar.FormFile.Length > (200 * 1024))
                {
                    ModelState.AddModelError("UserAvatar.FormFile", "Error: File too big");
                    ViewData["Roles"] = new SelectList(new String[] { "Employee", "Manager" });
                    return View(createUser);
                }

                var user = new ApplicationUser
                {
                    UserName = createUser.EMail,
                    Email = createUser.EMail,
                    FirstName = createUser.FirstName,
                    LastName = createUser.LastName,
                    BirthDate = createUser.BirthDate,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CompanyId = createUser.CompanyId
                };

                if (createUser.UserAvatar.FormFile != null)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await createUser.UserAvatar.FormFile.CopyToAsync(dataStream);
                        user.UserAvatar = dataStream.ToArray();
                    }
                }
                var res = await _userManager.CreateAsync(user, createUser.Password);
                if(!res.Succeeded)
                {
                    foreach(var err in res.Errors)
                    {
                        if(err.Code.Contains("Password"))
                        {
                            ModelState.AddModelError("Password", err.Description);
                        }
                    }
                    ViewData["Roles"] = new SelectList(new String[] { "Employee", "Manager" });
                    return View(createUser);
                }
                await _userManager.AddToRoleAsync(user, createUser.Role);
/*                var userId = await _userManager.GetUserIdAsync(user);
                user.Id = userId;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();*/
                TempData["Info"] = String.Format(
                    "Profile for employee '{0} {1}' was created. " +
                    "He/she can now login with Username '{2}' and Password '{3}'.",
                    user.FirstName, user.LastName, user.UserName, createUser.Password);
                return RedirectToAction(nameof(Index));
            }
            return View(createUser);
        }

        // GET: UserManager/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new EditEmployeeViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive
            };
            return View(model);
        }

        // POST: UserManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,FirstName,LastName,BirthDate,IsActive")] EditEmployeeViewModel editUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || (editUser != null && id != editUser.Id))
            {
                return NotFound();
            }

            try
            {
                user.IsActive = editUser.IsActive;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                TempData["Error"] = "Error while editing User: " + e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserManager/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName == User.Identity.Name)
            {
                TempData["Error"] = "You cannot delete your own profile!";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                var reservations = await _context.Reservations.Where(r => r.ClientId == user.Id).ToListAsync();
                if (reservations.Count() > 0)
                {
                    TempData["Error"] = "User has associated reservations. Cannot delete.";
                    return RedirectToAction("Index");
                }
                else
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
