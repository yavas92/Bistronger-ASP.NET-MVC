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
/// Vic
/// </summary>
namespace Bistronger.Data.Design
{
    public class AdvertManager : BaseManager, IAdvertManager
    {
        protected readonly IBusinessManager _businessManager;

        protected readonly IAdvertLineManager _advertLineManager;

        #region Constructor

        public AdvertManager(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IBusinessManager businessManager, IAdvertLineManager advertLineManager)
            : base(context, userManager, httpContextAccessor)
        {
            _businessManager = businessManager;
            _advertLineManager = advertLineManager;
        }

        #endregion Constructor

        #region CreateAdvert

        public Advert CreateAdvert(Advert advert)
        {
            if (advert == null)
                throw new ArgumentNullException(nameof(advert));

            _context.Adverts.Add(advert);
            _context.SaveChanges();

            return advert;
        }

        #endregion CreateAdvert

        #region DeleteAdvert

        public void DeleteAdvert(int id)
        {
            var p = GetAdvert(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            //_context.Entry(p).State = EntityState.Deleted;
            //_context.SaveChanges();
        }

        #endregion DeleteAdvert

        #region EditAdvert

        public Advert EditAdvert(Advert advert)
        {
            if (advert == null)
                throw new ArgumentNullException(nameof(advert));

            _context.Entry(advert).State = EntityState.Modified;
            _context.SaveChanges();

            return advert;
        }

        #endregion EditAdvert

        #region GetAdvert

        public Advert GetAdvert(int id)
        {
            var qry = from t in _context.Adverts.Include("Business")
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        #endregion GetAdvert

        #region GetAdverts

        public DataSet<Advert> GetAdverts(bool checkForOwnership = false)
        {
            var datasSet = new DataSet<Advert>();

            var qryBase = _context.Adverts;
            var qry = qryBase.Select(x => x);

            if (_user != null && checkForOwnership)
            {
                //Enkel Adverts ophalen die van ons zijn als we owner rol hebben
                /*
                      qry = from t in _context.BusinessMenus.Include("Menu")
                      where t.BusinessID == id
                      select t;
                 */
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    List<int> ownedBusinesses = _businessManager.GetBusinesses().Data.Select(business => business.ID).ToList();
                    qry = from t in qry
                          where ownedBusinesses.Contains(t.BusinessID)
                          select t;
                }
            }

            qry = from t in qry.Include("Business")
                  orderby t.CreatedOn descending
                  select t;
            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public DataSet<Advert> GetAdverts(AdvertFilter filter)
        {
            var datasSet = new DataSet<Advert>();
            if (filter == null)
                return GetAdverts(true);
            else
            {
                var qryBase = _context.Adverts;
                var qry = qryBase.Select(x => x);

                if (_user != null)
                {
                    //Enkel Adverts ophalen die van ons zijn als we owner rol hebben
                    if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                    {
                        List<int> ownedBusinesses = _businessManager.GetBusinesses().Data.Select(business => business.ID).ToList();
                        qry = from t in qry
                              where ownedBusinesses.Contains(t.BusinessID)
                              select t;
                    }
                }

                if (filter.BusinessID.HasValue)
                {
                    qry = from t in qry
                          where t.BusinessID == filter.BusinessID.Value
                          select t;
                }

                qry = from t in qry.Include("Business")
                      orderby t.CreatedOn descending
                      select t;
                datasSet.Data = qry.ToList();

                return datasSet;
            }
        }

        #endregion GetAdverts

        #region Update Adverts

        public void ReduceAdvertViewCount()
        {
            foreach (Advert advert in GetAdverts().Data)
            {

                if (advert.Shows > 1)
                {
                    advert.Shows--;
                    EditAdvert(advert);
                }
                else
                {
                    AdvertLine linkedLine = _advertLineManager.GetAdvertLineByAdvertID(advert.ID);
                    if (linkedLine != null)
                    {
                        linkedLine.AdvertID = null;
                        _advertLineManager.EditAdvertLine(linkedLine);
                    }
                    DeleteAdvert(advert.ID);
                }
            }
        }

        #endregion Update Adverts
    }
}