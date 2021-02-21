using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Models.MenuItems;
using Bistronger.Data.Design;

namespace Bistronger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMenuItemManager _menuItemManager;

        public MenuItemsController(ApplicationDbContext context, IMenuItemManager menuItemManager)
        {
            _context = context;
            _menuItemManager = menuItemManager;
        }

        // GET: api/MenuItems
        [HttpGet]
        public ActionResult<IEnumerable<MenuItem>> GetMenuItems()
        {
            return _menuItemManager.GetMenuItems().Data;
        }

        // GET: api/MenuItems/5
        [HttpGet("{id}")]
        public ActionResult<MenuItem> GetMenuItem(int id)
        {
            var menuItem = _menuItemManager.GetMenuItem(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return menuItem;
        }

        [HttpPost]
        public ActionResult<MenuItem> PostMenuItem(MenuItemCreateViewModel model)
        {
            string[] itemIDS = model.MenuItemString.Split(',');

            for (int i = 0; i < itemIDS.Length; i++)
            {
                MenuItem newMenuItem = new MenuItem
                {
                    ItemID = Int32.Parse(itemIDS[i]),
                    MenuID = model.MenuID
                };

                _menuItemManager.CreateMenuItem(newMenuItem);
            }

            return CreatedAtAction("GetMenuItem", new { id = Int32.Parse(itemIDS[0]) }, model);
        }

        [HttpDelete]
        public IActionResult DeleteMenuItem(MenuItemCreateViewModel model)
        {
            string[] itemIDS = model.MenuItemString.Split(',');

            for (int i = 0; i < itemIDS.Length; i++)
            {
                var menuItem = _menuItemManager.GetMenuItemByMenuIdAndItemId(model.MenuID, Int32.Parse(itemIDS[i]));

                if (menuItem == null)
                {
                    return NotFound();
                }

                _menuItemManager.DeleteMenuItem(menuItem.ID);
            }

            return NoContent();
        }

    }
}
