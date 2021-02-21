using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bistronger.Data
{
    /// <summary>
    /// aangemaakt door Scott
    /// </summary>
    public class OrderLine
    {
        public int ID { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }
        public Order Order { get; set; }
        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemID { get; set; }
        public Item Item { get; set; }
        
        [Range(0, Int32.MaxValue, ErrorMessage = "Hoeveelheid moet een positief getal zijn en >= 0.")]
        public int? Amount { get; set; }
    }
}
