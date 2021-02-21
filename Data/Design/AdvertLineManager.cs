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
    public class AdvertLineManager : BaseManager, IAdvertLineManager
    {
        #region Constructor

        public AdvertLineManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        #endregion Constructor

        #region CreateAdvertLine

        public AdvertLine CreateAdvertLine(AdvertLine advertLine)
        {
            if (advertLine == null)
                throw new ArgumentNullException(nameof(advertLine));

            _context.AdvertLines.Add(advertLine);
            _context.SaveChanges();

            return advertLine;
        }

        #endregion CreateAdvertLine

        #region DeleteAdvertLine

        public void DeleteAdvertLine(int id)
        {
            var p = GetAdvertLine(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        #endregion DeleteAdvertLine

        #region EditAdvertLine

        public AdvertLine EditAdvertLine(AdvertLine advertLine)
        {
            if (advertLine == null)
                throw new ArgumentNullException(nameof(advertLine));

            _context.Entry(advertLine).State = EntityState.Modified;
            _context.SaveChanges();

            return advertLine;
        }

        #endregion EditAdvertLine

        #region GetAdvertLine

        public AdvertLine GetAdvertLine(int id)
        {
            var qry = from t in _context.AdvertLines
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }

        #endregion GetAdvertLine

        #region GetAdvertLine (by order)

        public AdvertLine GetAdvertLine(Order order)
        {
            var qryBase = _context.AdvertLines;
            var qry = qryBase.Select(x => x);

            if (order != null)
            {
                //Enkel de advertline ophalen die tot ons order behoort
                qry = from t in qry
                      where t.OrderID == order.ID
                      select t;
            }
            return qry.SingleOrDefault();
        }

        #endregion GetAdvertLine (by order)

        #region GetAdvertLine (by advert ID)

        public AdvertLine GetAdvertLineByAdvertID(int advertID)
        {
            var qry = from t in _context.AdvertLines
                      where t.AdvertID == advertID
                      select t;
            return qry.SingleOrDefault();
        }

        #endregion GetAdvertLine (by advert ID)
    }
}