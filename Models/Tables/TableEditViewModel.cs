using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Models.Tables
{
    public class TableEditViewModel
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }

        [Required(ErrorMessage = "Het aantal zitplaatsen is een verplicht veld.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Aantal zitplaatsen moet groter zijn als 0.")]
        public int Seats { get; set; }

        public bool Available { get; set; }
    }
}