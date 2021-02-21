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
    public class OrderLineManager : BaseManager,IOrderLineManager
    {
        #region Constructor
        public OrderLineManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {

        }
        #endregion
        #region CreateOrderLine
        public OrderLine CreateOrderLine(OrderLine orderLine)
        {
            if (orderLine == null)
                throw new ArgumentNullException(nameof(orderLine));

            _context.OrderLines.Add(orderLine);
            _context.SaveChanges();

            return orderLine;
        }
        #endregion
        #region DeleteOrderLine
        public void DeleteOrderLine(int id)
        {
            var p = GetOrderLine(id);

            if (p == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        #endregion
        #region EditOrderLine
        public OrderLine EditOrderLine(OrderLine orderLine)
        {
            if (orderLine == null)
                throw new ArgumentNullException(nameof(orderLine));

            _context.Entry(orderLine).State = EntityState.Modified;
            _context.SaveChanges();

            return orderLine;
        }
        #endregion
        #region GetOrderLine
        public OrderLine GetOrderLine(int id)
        {
            var qry = from t in _context.OrderLines
                      where t.ID == id
                      select t;

            return qry.SingleOrDefault();
        }
        #endregion
        #region GetOrderLines
        public DataSet<OrderLine> GetOrderLines(Order order)
        {
            var datasSet = new DataSet<OrderLine>();

            var qryBase = _context.OrderLines.Include("Item");
            var qry = qryBase.Select(x => x);

            if (order != null)
            {
                //Enkel orderlines ophalen die tot ons order behoren
                    qry = from t in qry
                          where t.OrderID == order.ID
                          select t;
            }
            qry = from t in qry
                  orderby t.ItemID
                  select t;
            datasSet.Data = qry.ToList();

            return datasSet;
        }
        #endregion
    }
}
