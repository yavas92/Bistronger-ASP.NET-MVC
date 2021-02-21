using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    public class PackageFilter : DefaultFilter
    {
        public PackageType? Type { get; set; }
    }
}