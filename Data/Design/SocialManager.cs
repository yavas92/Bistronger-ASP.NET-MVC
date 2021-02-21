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
    public class SocialManager:BaseManager, ISocialManager
    {
        public SocialManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
      : base(context, userManager, httpContextAccessor)
        {

        }
        public Social CreateSocial(Social social)
        {
            if (social == null)
                throw new ArgumentNullException(nameof(social));

            _context.Socials.Add(social);
            _context.SaveChanges();

            return social;
        }

        public void DeleteSocial(int id)
        {
            var p = GetSocial(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        public Social EditSocial(Social social)
        {
            if (social == null)
                throw new ArgumentNullException(nameof(social));

            _context.Entry(social).State = EntityState.Modified;
            _context.SaveChanges();

            return social;
        }
        public Social GetSocial(int id)
        {
            var qry = from t in _context.Socials
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }
        public Social GetSocialForBusiness(int businessID)
        {
            var qry = from t in _context.Socials
                      where t.BusinessID == businessID
                      select t;

            return qry.SingleOrDefault();
        }
        public DataSet<Social> GetSocials(int businessID = -1)
        {
            var datasSet = new DataSet<Social>();

            var qryBase = _context.Socials;
            var qry = qryBase.Select(x => x);

            //filter on socials of specific business
            if (businessID > -1)
            {
                qry = from t in _context.Socials
                      where t.BusinessID == businessID
                      select t;
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }
    }
}
