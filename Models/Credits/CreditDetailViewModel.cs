using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Credits
{
    public class CreditDetailViewModel
    {
        public ApplicationUser User { get; set; }
        public int ID { get; set; }
        public string UserID { get; set; }
        public decimal Credits { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod Method { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}