using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Models.Menus;
using System.Security.Claims;
using Bistronger.Data.Design;
using Microsoft.AspNetCore.Authorization;
using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bistronger.Controllers
{
    [Authorize]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMenuManager _menuManager;
        private readonly IMenuItemManager _menuItemManager;
        private readonly IItemManager _itemManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenusController(ApplicationDbContext context, IMenuManager menuManager, IMenuItemManager menuItemManager, IItemManager itemManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _menuManager = menuManager;
            _menuItemManager = menuItemManager;
            _itemManager = itemManager;
            _userManager = userManager;
        }

        // GET: Menus
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Index()
        {
            MenuIndexViewModel model = new MenuIndexViewModel
            {
                Menus = _menuManager.GetMenus()
            };
            return View(model);
        }

        // GET: Menus/Details/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = _menuManager.GetMenu(id.Value);

            if (menu == null)
            {
                return NotFound();
            }

            MenuDetailViewModel vm = new MenuDetailViewModel
            {
                ID = menu.ID,
                Email = menu.Owner.Email,
                Naam = menu.Naam,
                menuItems = _menuItemManager.GetMenuItems(menu.ID)
            };

            return View(vm);
        }

        // GET: Menus/Create
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Create()
        {
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            MenuCreateViewModel vm = new MenuCreateViewModel();

            return View(vm);
        }

        // POST: Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Create(MenuCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu
                {
                    Naam = model.Name,
                    OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                _menuManager.CreateMenu(menu);

                return RedirectToAction(nameof(Edit), new { id = menu.ID });
            }
            return View(model);
        }

        // GET: Menus/Edit/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var menu = _menuManager.GetMenu(id.Value);

            if (menu == null)
            {
                return NotFound();
            }

            List<MenuItem> menuItems = _menuItemManager.GetMenuItems(menu.ID).Data;

            MenuEditViewModel vm = new MenuEditViewModel
            {
                ID = menu.ID,
                Name = menu.Naam,
                Email = menu.Owner.Email,
                MenuItems = menuItems,
                Items = _itemManager.GetItems(menuItems,menu.OwnerID).Data,
                OwnerID = menu.OwnerID
            };

            return View(vm);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int id, MenuEditViewModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Menu menu = new Menu
                    {
                        ID = model.ID,
                        Naam = model.Name,
                        OwnerID = model.OwnerID
                    };
                    _menuManager.EditMenu(menu);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(model.ID))
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
            return View(model);
        }

        // GET: Menus/Delete/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = _menuManager.GetMenu(id.Value);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult DeleteConfirmed(int id)
        {
            _menuManager.DeleteMenu(id);

            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.ID == id);
        }
    }
}
