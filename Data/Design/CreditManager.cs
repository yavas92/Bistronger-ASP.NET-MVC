using Bistronger.Areas.Identity;
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
    /// Joren
    /// </summary>
    public class CreditManager : BaseManager, ICreditManager
    {
        public CreditManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public decimal GetCreditAmount(string UserID)
        {
            if (!String.IsNullOrWhiteSpace(UserID))
            {
                var qry = from user in _context.Users
                          where String.Equals(user.Id, UserID)
                          select user;
                ApplicationUser currentUser = qry.SingleOrDefault();
                if (currentUser != null)
                {
                    return currentUser.Credit;
                }
            }
            return -1;
        }

        public bool AddCredit(string UserID, decimal amount)
        {
            if (!String.IsNullOrWhiteSpace(UserID) && amount >= 0)
            {
                var qry = from user in _context.Users
                          where String.Equals(user.Id, UserID)
                          select user;
                ApplicationUser currentUser = qry.SingleOrDefault();
                if (currentUser != null)
                {
                    currentUser.Credit += amount;
                    _context.Entry(currentUser).State = EntityState.Modified;
                    return _context.SaveChanges() > 0;
                }
            }
            return false;
        }

        public bool SubtractCredit(string UserID, decimal amount)
        {
            if (!String.IsNullOrWhiteSpace(UserID) && amount >= 0)
            {
                var qry = from user in _context.Users
                          where String.Equals(user.Id, UserID)
                          select user;
                ApplicationUser currentUser = qry.SingleOrDefault();
                if (currentUser != null)
                {
                    if (GetCreditAmount(UserID) >= amount)
                    {
                        currentUser.Credit -= amount;
                        _context.Entry(currentUser).State = EntityState.Modified;
                        return _context.SaveChanges() > 0;
                    }
                }
            }
            return false;
        }
    }
}