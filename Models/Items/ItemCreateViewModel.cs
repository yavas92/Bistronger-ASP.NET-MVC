using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn + Vic
/// </summary>
namespace Bistronger.Models.Items
{
    public class ItemCreateViewModel
    {
        [Required(ErrorMessage = "Naam dient correct ingevuld te zijn.")]
        [StringLength(30, ErrorMessage = "Naam mag maximaal 30 karakters bevatten.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Beschrijving mag maximaal 500 karakters bevatten.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Geen geldige prijs.")]
        [Range(0, Double.MaxValue, ErrorMessage = "Prijs moet een positief getal zijn.")]
        public decimal Price { get; set; }

        [Required]
        public ItemType Type { get; set; }

        public IFormFile DisplayPicture { get; set; }
    }
}