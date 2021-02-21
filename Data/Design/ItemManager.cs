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
/// Stijn + Vic
/// </summary>
namespace Bistronger.Data.Design
{
    public class ItemManager : BaseManager, IItemManager
    {
        public ItemManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {

        }
        public Item CreateItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _context.Items.Add(item);
            _context.SaveChanges();

            return item;
        }

        public void DeleteItem(int id)
        {
            var p = GetItem(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Item EditItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            return item;
        }

        public Item GetItem(int id)
        {
            var qry = from t in _context.Items.Include("Owner")
                      where t.ID == id
                      select t;

            //Een klant wil ook de details zien
            if (!_userManager.IsInRoleAsync(_user, UserRoleType.Customer.ToString()).Result)
            {
                //Enkel items ophalen die van ons zijn
                if (!_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == _user.Id
                          select t;
                }
            }

            return qry.SingleOrDefault();
        }

        public DataSet<Item> GetItems()
        {
            var datasSet = new DataSet<Item>();

            var qryBase = _context.Items.Include("Owner");
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                //Enkel items ophalen die van ons zijn
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == _user.Id
                          select t;
                }
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public DataSet<Item> GetItems(List<MenuItem> menuItems, string ownerID = null)
        {
            var datasSet = new DataSet<Item>();

            var qryBase = _context.Items.Include("Owner");
            var qry = qryBase.Select(x => x);

            List<int> ids = new List<int>();

            foreach (var item in menuItems)
            {
                ids.Add(item.ItemID);
            }

            //Enkel items ophalen die van ons zijn
            if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
            {
                qry = from t in qry
                      where t.OwnerID == _user.Id && !ids.Contains(t.ID)
                      select t;
            }
            else if (_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
            {
                if (ownerID != null)
                {
                    qry = from t in qry
                          where t.OwnerID == ownerID && !ids.Contains(t.ID)
                          select t;
                }
            }


            datasSet.Data = qry.ToList();

            return datasSet;
        }
    }
}
