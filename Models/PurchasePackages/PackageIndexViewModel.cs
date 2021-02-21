using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Models.Packages
{
    public class PackageIndexViewModel
    {
        public DataSet<Package> Packages { get; set; }
        public PackageFilter Filter { get; set; }
    }
}