using Bistronger.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Menus
{
    public class MenuDetailViewModel
    {
        [HiddenInput]
        public int ID { get; set; }
        public string Email { get; set; }
        public string Naam { get; set; }
        public DataSet<MenuItem> menuItems { get; set; }
    }
}
