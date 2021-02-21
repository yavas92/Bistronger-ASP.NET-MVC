using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Authorization;
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
    public class BusinessManager : BaseManager, IBusinessManager
    {
        public BusinessManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public Business CreateBusiness(Business business)
        {
            if (business == null)
                throw new ArgumentNullException(nameof(business));

            _context.Businesses.Add(business);
            _context.SaveChanges();

            return business;
        }

        public void DeleteBusiness(int id)
        {
            var p = GetBusiness(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Business EditBusiness(Business business)
        {
            if (business == null)
                throw new ArgumentNullException(nameof(business));

            _context.Entry(business).State = EntityState.Modified;
            _context.SaveChanges();

            return business;
        }

        public Business GetBusiness(int id)
        {
            var qry = from t in _context.Businesses.Include("Owner")
                      where t.ID == id
                      select t;

            //Enkel businesses ophalen die van ons zijn tenzij we admin rol hebben
            if (!_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
            {
                qry = from t in qry
                      where t.OwnerID == _user.Id
                      select t;
            }

            return qry.SingleOrDefault();
        }

        public Business GetBusinessForDetails(int id)
        {
            var qry = from t in _context.Businesses
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        public DataSet<Business> GetBusinesses()
        {
            var datasSet = new DataSet<Business>();

            var qryBase = _context.Businesses.Include("Owner");
            var qry = qryBase.Select(x => x);

            if (_user != null)
            {
                //Enkel businesses ophalen die van ons zijn tenzij we admin rol hebben
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    qry = from t in qry
                          where t.OwnerID == _user.Id
                          select t;
                }
            }
            qry = from t in qry
                  orderby t.Name
                  select t;
            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public DataSet<Business> GetBusinesses(BusinessFilter filter)
        {
            var datasSet = new DataSet<Business>();
            if (filter == null)
                return GetBusinesses();
            else
            {
                var qryBase = _context.Businesses.Include("Owner");
                var qry = qryBase.Select(x => x);

                if (_user != null)
                {
                    //Enkel businesses ophalen die van ons zijn tenzij we admin rol hebben
                    if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                    {
                        qry = from t in qry
                              where t.OwnerID == _user.Id
                              select t;
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    qry = from t in qry
                          where t.Name.ToLower().Contains(filter.Name.ToLower())
                          select t;
                }

                if (filter.Zipcode.HasValue)
                {
                    qry = from t in qry
                          where t.Zipcode == filter.Zipcode
                          select t;
                }

                if (filter.Type.HasValue)
                {
                    qry = from t in qry
                          where t.Type == filter.Type
                          select t;
                }

                if (filter.ShowOpenOnly)
                {
                    var ToD = DateTime.Now.TimeOfDay;
                    var DoW = DateTime.Today.DayOfWeek;
                    List<Business> tempList = new List<Business>();
                    foreach (Business business in qry.ToList())
                    {
                        if ((TimeSpan.Compare(GetBusinessHourToday(business.ID, DoW).OpeningsHour.TimeOfDay, ToD) == -1)
                                    && (TimeSpan.Compare(GetBusinessHourToday(business.ID, DoW).ClosingHour.TimeOfDay, ToD) == 1))
                        {
                            tempList.Add(business);
                        }
                    }
                    datasSet.Data = tempList;
                }
                else datasSet.Data = qry.ToList();

                return datasSet;
            }
        }

        public BusinessHour AddBusinessHours(BusinessHour businessHour)
        {
            if (businessHour == null)
                throw new ArgumentNullException(nameof(businessHour));

            _context.BusinessHours.Add(businessHour);
            _context.SaveChanges();

            return businessHour;
        }

        public DataSet<BusinessHour> GetBusinessHours(int businessID)
        {
            var datasSet = new DataSet<BusinessHour>();

            var qryBase = _context.BusinessHours;
            var qry = qryBase.Select(x => x);

            qry = from t in qry
                  where t.BusinessID == businessID
                  select t;

            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public BusinessHour GetBusinessHourToday(int businessID, DayOfWeek day)
        {
            var qry = GetBusinessHours(businessID).Data.Select(x => x);
            qry = from t in qry
                  where t.Day == day
                  select t;
            return qry.ToList().FirstOrDefault();
        }

        public BusinessHour EditBusinessHour(BusinessHour businessHour)
        {
            if (businessHour == null)
                throw new ArgumentNullException(nameof(businessHour));

            _context.Entry(businessHour).State = EntityState.Modified;
            _context.SaveChanges();

            return businessHour;
        }

        public DataSet<BusinessHour> GetAllBusinessHours()
        {
            var datasSet = new DataSet<BusinessHour>();
            var qry = _context.BusinessHours.Select(x => x);
            datasSet.Data = qry.ToList();
            return datasSet;
        }

        public BusinessOpenStatus GetBusinessOpenStatus(int businessID)
        {
            TimeSpan openingHour = GetBusinessHourToday(businessID, DateTime.Today.DayOfWeek).OpeningsHour.TimeOfDay;
            TimeSpan closingHour = GetBusinessHourToday(businessID, DateTime.Today.DayOfWeek).ClosingHour.TimeOfDay;

            bool open = (TimeSpan.Compare(openingHour, DateTime.Now.TimeOfDay) == -1) && (TimeSpan.Compare(closingHour, DateTime.Now.TimeOfDay) == 1);
            bool closesSoon = false;
            bool opensSoon = false;
            if (open)
            {
                TimeSpan diff = closingHour - DateTime.Now.TimeOfDay;
                closesSoon = diff.TotalMinutes > 0 && diff.TotalMinutes <= 30;
                
                if (closesSoon)
                    return BusinessOpenStatus.ClosesSoon;
                else
                    return BusinessOpenStatus.Open;
            }
            else
            {
                TimeSpan diff = openingHour - DateTime.Now.TimeOfDay;
                opensSoon = diff.TotalMinutes > 0 && diff.TotalMinutes <= 30;

                if (opensSoon)
                    return BusinessOpenStatus.OpenSoon;
                else
                    return BusinessOpenStatus.Closed;
            }
        }
    }
}