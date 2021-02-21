using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using System.Security.Claims;
using Bistronger.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Bistronger.Data.Design;
using Bistronger.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Bistronger.Data.Enums;
using System.IO;

namespace Bistronger.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemManager _itemManager;
        public readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(ApplicationDbContext context, IItemManager itemManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _itemManager = itemManager;
            _userManager = userManager;
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        // GET: Items
        public IActionResult Index()
        {
            //var applicationDbContext = _context.Items.Include(i => i.Owner);

            var indexVM = new ItemIndexViewModel
            {
                Items = _itemManager.GetItems()
            };
            return View(indexVM);
        }


        // GET: Items/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemManager.GetItem(id.Value);

            var vm = new ItemDetailViewModel
            {
                ID = item.ID,
                Email = item.Owner.Email,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type,
                DisplayPicture = item.DisplayPicture
            };

            if (item == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ItemCreateViewModel vm = new ItemCreateViewModel();
            return View(vm);
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Create(ItemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new Item
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    Type = model.Type,
                    OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                if (model.DisplayPicture != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.DisplayPicture.CopyTo(ms);
                        item.DisplayPicture = ms.ToArray();
                    }
                }

                _itemManager.CreateItem(item);

                return RedirectToAction(nameof(Index));
            }

            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(model);
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        // GET: Items/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemManager.GetItem(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vm = new ItemEditViewModel
            {
                ID = item.ID,
                OwnerID = item.OwnerID,
                Email = item.Owner.Email,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type,
                DisplayPictureBytes = item.DisplayPicture
            };

            return View(vm);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int id, ItemEditViewModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var item = new Item
                    {
                        ID = model.ID,
                        OwnerID = model.OwnerID,
                        Description = model.Description,
                        Name = model.Name,
                        Price = model.Price,
                        Type = model.Type,
                        DisplayPicture = model.DisplayPictureBytes
                    };

                    //If new image we need to overwrite it
                    if (model.DisplayPictureUpload != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            model.DisplayPictureUpload.CopyTo(ms);
                            item.DisplayPicture = ms.ToArray();
                        }
                    }

                    _itemManager.EditItem(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(model.ID))
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
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(model);
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemManager.GetItem(id.Value);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public ActionResult DeleteConfirmed(int id)
        {
            _itemManager.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}