using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
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
    public class MenuManager : BaseManager, IMenuManager
    {
        public MenuManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccesor)
            : base(context, userManager, httpContextAccesor)
        {

        }

        public Menu CreateMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _context.Menus.Add(menu);
            _context.SaveChanges();

            return menu;
        }

        public void DeleteMenu(int id)
        {
            var p = GetMenu(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Menu EditMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _context.Entry(menu).State = EntityState.Modified;
            _context.SaveChanges();

            return menu;
        }

        public Menu GetMenu(int id)
        {
            var qry = from t in _context.Menus.Include("Owner")
                      where t.ID == id
                      select t;

            if (_user != null)
            {
                //Enkel menu ophalen als die van ons is tenzij we admin rol hebben
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == _user.Id
                          select t;
                }
            }

            return qry.SingleOrDefault();
        }

        public DataSet<Menu> GetMenus(string ownerID = null)
        {
            var datasSet = new DataSet<Menu>();

            var qryBase = _context.Menus.Include("Owner");
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                //Enkel menus ophalen die van ons zijn tenzij we admin rol hebben
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == _user.Id
                          select t;
                }
                else if (_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == ownerID
                          select t;
                }
            }

            qry = from t in qry
                  orderby t.Naam
                  select t;

            datasSet.Data = qry.ToList();

            return datasSet;
        }

    }
}
