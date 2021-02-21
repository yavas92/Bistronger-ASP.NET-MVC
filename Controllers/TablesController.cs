using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Models.Tables;
using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using System.Security.Claims;

namespace Bistronger.Controllers
{
    /// <summary>
    /// Joren
    /// </summary>
    public class TablesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBusinessManager _businessManager;
        private readonly ITableManager _tableManager;

        public TablesController(ApplicationDbContext context,
            IBusinessManager businessManager, ITableManager tableManager)
        {
            _context = context;
            _businessManager = businessManager;
            _tableManager = tableManager;
        }

        // GET: Tables/businessID
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Index(int? businessID)
        {
            if (businessID == null || !businessID.HasValue)
            {
                return NotFound();
            }
            _tableManager.UpdateTablesAvailability(businessID.Value);
            var indexVM = new TableIndexViewModel
            {
                Business = _businessManager.GetBusiness(businessID.Value),
                Tables = _tableManager.GetTables(businessID.Value)
            };
            return View(indexVM);
        }

        // GET: Tables/Add
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Add(int? businessID)
        {
            if (businessID == null)
            {
                return NotFound();
            }
            var addVM = new TableAddViewModel
            {
                BusinessID = businessID.Value
            };
            return View(addVM);
        }

        // POST: Tables/ADD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Add(TableAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Table newTable = new Table
                {
                    BusinessID = vm.BusinessID,
                    Seats = vm.Seats,
                    Available = vm.Available
                };

                _tableManager.CreateTable(newTable);
                return RedirectToAction(nameof(Index), new { businessID = vm.BusinessID });
            }
            return View(vm);
        }

        // GET: Tables/Edit/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = _tableManager.GetTable(id.Value);
            if (table == null)
            {
                return NotFound();
            }

            var vm = new TableEditViewModel
            {
                ID = table.ID,
                BusinessID = table.BusinessID,
                Seats = table.Seats,
                Available = table.Available
            };
            return View(vm);
        }

        // POST: Tables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int id, TableEditViewModel vm)
        {
            if (id != vm.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var table = new Table
                {
                    ID = vm.ID,
                    BusinessID = vm.BusinessID,
                    Seats = vm.Seats,
                    Available = vm.Available
                };

                try
                {
                    _tableManager.EditTable(table);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { businessID = vm.BusinessID });
            }
            return View(vm);
        }

        // POST: Tables/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Delete(int? id, int businessID)
        {
            if (id == null)
            {
                return NotFound();
            }
            var table = _tableManager.GetTable(id.Value);
            if (table == null)
            {
                return NotFound();
            }

            _tableManager.DeleteTable(id.Value);
            return RedirectToAction(nameof(Index), new { businessID = businessID });
        }

        private bool TableExists(int id)
        {
            return _tableManager.GetTable(id) != null;
        }
    }
}