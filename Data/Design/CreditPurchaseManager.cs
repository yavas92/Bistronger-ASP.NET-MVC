using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Vic & Scott
    /// </summary>
    public class CreditPurchaseManager : BaseManager, ICreditPurchaseManager
    {
        public CreditPurchaseManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public CreditPurchase CreateCreditPurchase(CreditPurchase creditPurchase)
        {
            if (creditPurchase == null)
                throw new ArgumentNullException(nameof(creditPurchase));

            _context.CreditPurchases.Add(creditPurchase);
            if (_context.SaveChanges() > 0)
                return creditPurchase;
            return null;
        }

        public void DeleteCreditPurchase(int id)
        {
            var p = GetCreditPurchase(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public CreditPurchase EditCreditPurchase(CreditPurchase creditPurchase)
        {
            if (creditPurchase == null)
                throw new ArgumentNullException(nameof(creditPurchase));

            _context.Entry(creditPurchase).State = EntityState.Modified;
            _context.SaveChanges();

            return creditPurchase;
        }

        public CreditPurchase GetCreditPurchase(int id)
        {
            var qry = from t in _context.CreditPurchases
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        public DataSet<CreditPurchase> GetCreditPurchases()
        {
            // check for role
            var datasSet = new DataSet<CreditPurchase>();

            var qryBase = _context.CreditPurchases;
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                //Enkel CreditPurchase ophalen die van ons zijn tenzij we admin rol hebben
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result ||
                    _userManager.IsInRoleAsync(_user, UserRoleType.Customer.ToString()).Result)
                {
                    qry = from t in qry
                          where t.UserID == _user.Id
                          select t;
                }
            }
            qry = from t in qry
                  orderby t.CreatedOn descending
                  select t;
            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public DataSet<CreditPurchase> GetCreditPurchases(CreditFilter filter)
        {
            var datasSet = new DataSet<CreditPurchase>();
            if (filter == null)
                return GetCreditPurchases();
            else
            {
                var qryBase = _context.CreditPurchases;
                var qry = qryBase.Select(x => x);

                if (_user != null)
                {
                    //Enkel CreditPurchase ophalen die van ons zijn tenzij we admin rol hebben
                    if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result ||
                        _userManager.IsInRoleAsync(_user, UserRoleType.Customer.ToString()).Result)
                    {
                        qry = from t in qry
                              where t.UserID == _user.Id
                              select t;
                    }
                }

                if (filter.From.HasValue)
                {
                    qry = from t in qry
                          where t.CreatedOn.Date >= filter.From.Value.Date
                          select t;
                }

                if (filter.To.HasValue)
                {
                    qry = from t in qry
                          where t.CreatedOn.Date <= filter.To.Value.Date
                          select t;
                }

                if (filter.Status.HasValue)
                {
                    qry = from t in qry
                          where t.Status == filter.Status.Value
                          select t;
                }

                if (!String.IsNullOrWhiteSpace(filter.UserID))
                {
                    qry = from t in qry
                          where t.UserID.ToLower().Contains(filter.UserID.ToLower())
                          select t;
                }

                if (filter.PurchaseID.HasValue)
                {
                    qry = from t in qry
                          where t.ID == filter.PurchaseID.Value
                          select t;
                }

                qry = from t in qry
                      orderby t.CreatedOn descending
                      select t;
                datasSet.Data = qry.ToList();
                return datasSet;
            }
        }
    }
}