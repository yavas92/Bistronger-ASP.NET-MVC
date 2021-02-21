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
    public class ReservationManager : BaseManager, IReservationManager
    {
        public ReservationManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public Reservation CreateReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return reservation;
        }

        public void DeleteReservation(int id)
        {
            var p = GetReservation(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Reservation EditReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            _context.Entry(reservation).State = EntityState.Modified;
            _context.SaveChanges();

            return reservation;
        }

        public Reservation GetReservation(int id)
        {
            var qry = from t in _context.Reservations
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

        public DataSet<Reservation> GetReservations()
        {
            var datasSet = new DataSet<Reservation>();

            var qryBase = _context.Reservations;
            var qry = qryBase.Select(x => x).Include("User").Include("TableReservation").Include("TableReservation.Business");

            if (_user != null)
            {
                // Indien customer: alle reservations voor onze id
                if (_userManager.IsInRoleAsync(_user, UserRoleType.Customer.ToString()).Result)
                {
                    qry = from t in qry
                          where t.UserID == _user.Id
                          select t;
                }

                // Indien owner: alle reservations voor al onze businesses
                else if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    // via tableId -> table -> business -> owner id
                    qry = from t in qry
                          where t.TableReservation.Business.OwnerID == _user.Id
                          select t;
                }
            }

            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public DataSet<Reservation> GetReservations(int tableID)
        {
            var datasSet = new DataSet<Reservation>();

            var qryBase = _context.Reservations;
            var qry = qryBase.Select(x => x);

            qry = from t in qry
                  where t.TableID == tableID
                  select t;

            datasSet.Data = qry.ToList();

            return datasSet;
        }

        public bool DoesReservationExistForTable(int tableID, DateTime startTime)
        {
            // check if startTime is during a reservation
            var qry = from t in GetReservations(tableID).Data
                      where startTime >= t.ReservationDateFrom.Value && startTime <= t.ReservationDateTo
                      select t;
            return qry.ToList().Count > 0;
        }
    }
}