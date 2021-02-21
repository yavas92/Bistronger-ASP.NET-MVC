using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Models.Packages;
using Bistronger.Data.Design;
using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;

namespace Bistronger.Controllers
{
    public class PurchasePackagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPackageManager _packageManager;

        public PurchasePackagesController(ApplicationDbContext context, IPackageManager packageManager)
        {
            _context = context;
            _packageManager = packageManager;
        }

        // GET: Packages
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Index(PackageFilter filter)
        {
            if (filter == null)
                filter = new PackageFilter();

            var VM = new PackageIndexViewModel
            {
                Packages = _packageManager.GetPackages(filter),
                Filter = filter
            };

            return View(VM);
        }

        // GET: Packages/Create
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Create()
        {
            PackageCreateViewModel vm = new PackageCreateViewModel();
            return View(vm);
        }

        // POST: Packages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Create(PackageCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Package package = new Package
                {
                    Type = vm.Type,
                    Amount = vm.Amount,
                    Price = vm.Price
                };
                _packageManager.CreatePackage(package);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Packages/Edit/5
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Edit(int? id)
        {
            if (id == null || !id.HasValue)
            {
                return NotFound();
            }

            var package = _packageManager.GetPackage(id.Value);
            if (package == null)
            {
                return NotFound();
            }

            PackageEditViewModel vm = new PackageEditViewModel()
            {
                ID = package.ID,
                Type = package.Type,
                Amount = package.Amount,
                Price = package.Price
            };
            return View(vm);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Edit(int id, PackageEditViewModel vm)
        {
            if (id != vm.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Package package = new Package
                {
                    ID = vm.ID,
                    Type = vm.Type,
                    Amount = vm.Amount,
                    Price = vm.Price
                };
                try
                {
                    _packageManager.EditPackage(package);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Packages/Delete/5
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null || !id.HasValue)
            {
                return NotFound();
            }

            var package = _packageManager.GetPackage(id.Value);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult DeleteConfirmed(int id)
        {
            _packageManager.DeletePackage(id);
            return RedirectToAction(nameof(Index));
        }
    }
}