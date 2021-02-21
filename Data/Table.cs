using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data
{
    public class Table
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessID { get; set; }

        public Business Business { get; set; }

        [Required(ErrorMessage = "Het aantal zitplaatsen is een verplicht veld.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Aantal zitplaatsen moet positief zijn.")]
        public int Seats { get; set; }

        [Required]
        public bool Available { get; set; } = true;
    }
}