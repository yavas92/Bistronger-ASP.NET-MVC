using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data.Design
{
    public class TableManager : BaseManager, ITableManager
    {
        private IReservationManager _reservationManager;

        public TableManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IReservationManager reservationManager)
            : base(context, userManager, httpContextAccessor)
        {
            _reservationManager = reservationManager;
        }

        public DataSet<Table> GetTables(int businessID)
        {
            var dataSet = new DataSet<Table>();
            var qry = from t in _context.Tables.Include("Business")
                      where t.BusinessID == businessID
                      select t;
            dataSet.Data = qry.ToList();
            return dataSet;
        }

        public Table GetTable(int id)
        {
            var qry = from t in _context.Tables.Include("Business")
                      where t.ID == id
                      select t;
            Table? table = qry.SingleOrDefault();
            if (table != null)
            {
                if (_userManager.IsInRoleAsync(_user, UserRoleType.RestaurantOwner.ToString()).Result)
                {
                    if (table.Business.OwnerID == _user.Id)
                    {
                        return table;
                    }
                }
                else if (_userManager.IsInRoleAsync(_user, UserRoleType.Admin.ToString()).Result)
                {
                    return table;
                }
            }
            return null;
        }

        public Table CreateTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            _context.Tables.Add(table);
            _context.SaveChanges();
            return table;
        }

        public Table EditTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            _context.Entry(table).State = EntityState.Modified;
            _context.SaveChanges();
            return table;
        }

        public void DeleteTable(int id)
        {
            var table = GetTable(id);
            if (table == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");
            _context.Entry(table).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void SetTableAvailable(int id)
        {
            var table = GetTable(id);
            if (table == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");
            table.Available = true;
            EditTable(table);
        }

        public void SetTableUnavailable(int id)
        {
            var table = GetTable(id);
            if (table == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");
            table.Available = false;
            EditTable(table);
        }

        public Table GetFreeTable(int businessID, int peopleAmount, DateTime reservationDate)
        {
            // Get tables with enough seats for businessID and put table with least amount of seats first
            var qry = from t in GetTables(businessID).Data
                      where t.Seats >= peopleAmount
                      select t;

            // Get tables that are free at day/time
            qry = from t in qry
                  where _reservationManager.DoesReservationExistForTable(t.ID, reservationDate) == false
                  orderby t.Seats ascending
                  select t;

            // Return free table with least amount of seats
            if (qry.Count() > 0)
                return qry.First();
            return null;
        }

        public void UpdateTablesAvailability(int businessID)
        {
            foreach (Table table in GetTables(businessID).Data)
            {
                foreach (Reservation reservation in _reservationManager.GetReservations(table.ID).Data)
                {
                    if (reservation.ReservationDateFrom.Value.CompareTo(DateTime.Now) <= 0 && reservation.ReservationDateTo.Value.CompareTo(DateTime.Now) >= 0)
                    {
                        if (table.Available)
                        {
                            table.Available = false;
                            EditTable(table);
                        }
                    }
                }
            }
        }
    }
}