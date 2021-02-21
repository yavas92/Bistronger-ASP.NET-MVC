using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Enums;
using Bistronger.Data.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Vic & Scott
/// </summary>
namespace Bistronger.Models
{
    public class ReservationCreateViewModel
    {
        public bool FoundTable { get; set; }

        public int BusinessID { get; set; }
        public Business BusinessToReserve { get; set; }
        public List<BusinessHour> BusinessHours { get; set; }

        [Required(ErrorMessage = "Aantal gasten is verplicht.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Het gekozen aantal is ongeldig.")]
        public int AmountOfGuests { get; set; } = 1;

        [Required(ErrorMessage = "Verplicht om een reservatiedag en -uur te selecteren.")]
        public DateTime? ReservationDate { get; set; }
    }
}