using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    public class CreditFilter : DefaultFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public OrderStatus? Status { get; set; }
        public string UserID { get; set; }
        public int? PurchaseID { get; set; }
    }
}