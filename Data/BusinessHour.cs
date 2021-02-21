using Bistronger.Data.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data
{
    public class BusinessHour
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessID { get; set; }
        public Business Business { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public DateTime OpeningsHour { get; set; }

        [Required]
        [BusinessHourGreaterThan("OpeningsHour", "Sluitingsuur moet minstens 1u na openingsuur liggen")]
        public DateTime ClosingHour { get; set; }
    }
}