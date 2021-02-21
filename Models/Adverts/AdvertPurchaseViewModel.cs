using Bistronger.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Adverts
{
    public class AdvertPurchaseViewModel
    {
        public bool Successful { get; set; }
        public int BusinessID { get; set; }
        public IFormFile PhotoData { get; set; }
        public List<Business> Businesses { get; set; }

        public List<Package> Packages { get; set; }
        public int SelectedPackageID { get; set; }
    }
}