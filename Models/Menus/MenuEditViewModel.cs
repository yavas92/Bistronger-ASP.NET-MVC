using Bistronger.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Menus
{
    public class MenuEditViewModel
    {
        [HiddenInput]
        public int ID { get; set; }
        [HiddenInput]
        public string OwnerID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        public List<Item> Items { get; set; }

    }
}
