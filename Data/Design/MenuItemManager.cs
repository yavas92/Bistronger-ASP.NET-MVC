using Bistronger.Areas.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public class MenuItemManager : BaseManager, IMenuItemManager
    {
        public MenuItemManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {

        }

        public MenuItem CreateMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            return menuItem;
        }

        public void DeleteMenuItem(int id)
        {
            var p = GetMenuItem(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public MenuItem EditMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            _context.Entry(menuItem).State = EntityState.Modified;
            _context.SaveChanges();

            return menuItem;
        }

        public MenuItem GetMenuItem(int id)
        {
            var qry = from t in _context.MenuItems.Include("Item")
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        public MenuItem GetMenuItemByMenuIdAndItemId(int menuId, int itemId)
        {
            var qry = from t in _context.MenuItems
                      where t.MenuID == menuId && t.ItemID == itemId
                      select t;

            return qry.SingleOrDefault();
        }

        public DataSet<MenuItem> GetMenuItems(int id = -1)
        {
            var datasSet = new DataSet<MenuItem>();

            var qryBase = _context.MenuItems.Include("Item");
            var qry = qryBase.Select(x => x);


            //filter to only shows menuItems from specific menu
            if (id > -1)
            {
                qry = from t in _context.MenuItems.Include("Item")
                      where t.MenuID == id
                      select t;
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }
    }
}
