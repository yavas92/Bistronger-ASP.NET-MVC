using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Adverts
{
    public class AdvertIndexViewModel
    {
        public DataSet<Advert> Adverts { get; set; }
        public AdvertFilter Filter { get; set; }
    }
}