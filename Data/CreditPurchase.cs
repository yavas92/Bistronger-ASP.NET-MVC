using System;
using Bistronger.Data.Enums;
using Bistronger.Areas.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
///  Joren
/// </summary>
namespace Bistronger.Data
{
    public class CreditPurchase
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "Credits kan niet negatief of nul zijn.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Credits { get; set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "Prijs kan niet negatief zijn.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}")]
        public DateTime CreatedOn { get; set; }
    }
}