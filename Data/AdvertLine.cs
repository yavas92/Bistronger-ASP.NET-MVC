using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bistronger.Data
{
    /// <summary>
    /// aangemaakt door Scott
    /// </summary>
    public class AdvertLine
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }

        public Order Order { get; set; }

        public int? AdvertID { get; set; }

        [Required]
        public int OriginalShows { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePaid { get; set; }
    }
}