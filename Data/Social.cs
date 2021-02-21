using Bistronger.Areas.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    /// <summary>
    /// Abdullah
    /// </summary>
    public class Social
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessID { get; set; }
        public Business Business { get; set; }

        [Url(ErrorMessage = "Geen geldige url")]
        public string Website { get; set; }
        [Url(ErrorMessage = "Geen geldige url")]

        public string Facebook { get; set; }
        [Url(ErrorMessage = "Geen geldige url")]

        public string Instagram { get; set; }
        [Url(ErrorMessage = "Geen geldige url")]
        public string Twitter { get; set; }
    }
}
