using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Models.Orders
{
    public class OrderDetailViewModel
    {
        public int ID { get; set; }
        public int? BusinessID { get; set; }

        public string UserID { get; set; }

        public ApplicationUser User { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<OrderLine> OrderLines { get; set; }

        public bool Successful { get; set; }
        public OrderDetailState State { get; set; }
    }
}
