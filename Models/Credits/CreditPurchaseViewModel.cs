using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Credits
{
    public class CreditPurchaseViewModel
    {
        public List<Package> Packages { get; set; }
        public int SelectedPackageID { get; set; }
    }
}