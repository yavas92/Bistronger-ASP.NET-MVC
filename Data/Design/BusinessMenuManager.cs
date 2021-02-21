using Bistronger.Areas.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public class BusinessMenuManager: BaseManager, IBusinessMenuManager
    {
        public BusinessMenuManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {

        }

        public BusinessMenu CreateBusinessMenu(BusinessMenu businessMenu)
        {
            if (businessMenu == null)
                throw new ArgumentNullException(nameof(businessMenu));

            _context.BusinessMenus.Add(businessMenu);
            _context.SaveChanges();

            return businessMenu;
        }

        public void DeleteBusinessMenu(int id)
        {
            var p = GetBusinessMenu(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public BusinessMenu EditBusinessMenu(BusinessMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _context.Entry(menu).State = EntityState.Modified;
            _context.SaveChanges();

            return menu;
        }

        public BusinessMenu GetBusinessMenu(int id)
        {
            var qry = from t in _context.BusinessMenus.Include("Menu")
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        public DataSet<BusinessMenu> GetBusinessMenus(int id = -1)
        {
            var datasSet = new DataSet<BusinessMenu>();

            var qryBase = _context.BusinessMenus.Include("Menu");
            var qry = qryBase.Select(x => x);

            //filter to only shows menus from specific businessID
            if (id > -1)
            {
                qry = from t in _context.BusinessMenus.Include("Menu")
                      where t.BusinessID == id
                      select t;
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public BusinessMenu GetBusinessMenuForBusiness(int id)
        {
            var qry = from t in _context.BusinessMenus.Include("Business").Include("Menu")
                      where t.BusinessID == id
                      select t;

            return qry.SingleOrDefault();
        }
    }
}
