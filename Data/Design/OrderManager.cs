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
using Microsoft.AspNetCore.Authorization;


/// <summary>
/// Vic
/// </summary>
namespace Bistronger.Data.Design
{
    public class OrderManager : BaseManager, IOrderManager
    {
        private readonly IBusinessManager _businessManager;
        public OrderManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IBusinessManager businessManager)
            : base(context, userManager, httpContextAccessor)
        {
            _businessManager = businessManager;
        }
        public Order CreateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public void DeleteOrder(int id)
        {
            var p = GetOrder(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        public Order EditOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return order;
        }
        public Order GetOrder(int id)
        {
            var qry = from t in _context.Orders
                      where t.ID == id
                      select t;

            //Enkel orders ophalen die van ons zijn tenzij we admin rol hebben
            if (!_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
            {
                qry = from t in qry
                      where t.UserID == _user.Id
                      select t;
            }            

            return qry.SingleOrDefault();
        }

        public Order GetOrderDetailsAsBistro(int id)
        {
            var qry = from t in _context.Orders
                      where t.ID == id
                      select t;

            var business = _context.Businesses.Where(b => b.ID == qry.SingleOrDefault().BusinessID).FirstOrDefault();

            if (business.OwnerID == _user.Id)
                return qry.SingleOrDefault();
            else
                return null;
        }

        public DataSet<Order> GetOrders()
        {
            var datasSet = new DataSet<Order>();

            var qryBase = _context.Orders.Include("User");
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                //Enkel orders ophalen die van ons zijn tenzij we admin rol hebben
                if (!_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
                {
                    qry = from t in qry
                          where t.UserID == _user.Id
                          select t;
                }
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }
        public DataSet<Order> GetOrdersForBistros()
        {
            var datasSet = new DataSet<Order>();

            int[] businesses = _context.Businesses.Where(b => b.OwnerID == _user.Id).Select(b => b.ID).ToArray();

            var qryBase = _context.Orders;
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                // alle bestellingen bij onze bistros weergeven
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    qry = qry.Where(o => businesses.Contains((int)o.BusinessID));

                }
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }
    }
}
