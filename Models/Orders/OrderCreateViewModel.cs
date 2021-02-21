using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Models.Orders
{
    public class OrderCreateViewModel
    {
        [HiddenInput]
        public int BusinessID { get; set; }
        [HiddenInput]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public BusinessMenu BusMenu { get; set; }
    }
}
