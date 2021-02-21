using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data
{
    public class Advert
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessID { get; set; }
        public Business Business { get; set; }

        public byte[] PhotoData { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Shows kan niet negatief zijn.")]
        public int Shows { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}
