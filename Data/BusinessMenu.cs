using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    public class BusinessMenu
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Menu))]
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
