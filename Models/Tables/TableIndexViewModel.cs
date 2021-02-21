using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Models.Tables
{
    public class TableIndexViewModel
    {
        public Business Business { get; set; }
        public DataSet<Table> Tables { get; set; }
    }
}