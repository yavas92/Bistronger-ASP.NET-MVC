using Bistronger.Areas.Identity;
using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Credits
{
    public class CreditIndexViewModel
    {
        public ApplicationUser User { get; set; }
        public DataSet<CreditPurchase> CreditPurchases { get; set; }
        public CreditFilter Filter { get; set; }
    }
}