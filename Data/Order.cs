using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bistronger.Data
{
    /// <summary>
    /// aangemaakt door Scott
    /// </summary>
    public class Order
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public int? BusinessID { get; set; }

        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}