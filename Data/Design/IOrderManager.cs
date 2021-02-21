using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Vic
    /// </summary>
    public interface IOrderManager
    {
        DataSet<Order> GetOrders();
        DataSet<Order> GetOrdersForBistros();
        Order GetOrder(int id);
        Order GetOrderDetailsAsBistro(int id);
        Order CreateOrder(Order order);
        Order EditOrder(Order order);
        void DeleteOrder(int id);
    }
}
