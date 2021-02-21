using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Vic
    /// </summary>
    public interface IOrderLineManager
    {
        DataSet<OrderLine> GetOrderLines(Order order);
        OrderLine GetOrderLine(int id);
        OrderLine CreateOrderLine(OrderLine orderLine);
        OrderLine EditOrderLine(OrderLine orderLine);
        void DeleteOrderLine(int id);
    }
}
