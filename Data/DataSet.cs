using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    public class DataSet<T>
        where T : class
    {
        public List<T> Data { get; set; }

        public int PageSize { get; set; }
        public int PageAmount { get; set; }
        public int PageNumber { get; set; }
    }
}