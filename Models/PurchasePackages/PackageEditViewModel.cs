using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Models.Packages
{
    public class PackageEditViewModel
    {
        [HiddenInput]
        public int ID { get; set; }

        [HiddenInput]
        public PackageType Type { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Hoeveelheid moet groter zijn als 0.")]
        public int Amount { get; set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "Prijs moet groter of gelijk zijn aan 0.")]
        public decimal Price { get; set; }
    }
}