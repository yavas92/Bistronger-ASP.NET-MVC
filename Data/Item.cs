using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;

/// <summary>
/// Joren
/// </summary>

namespace Bistronger.Data
{
    public class Item
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Naam dient correct ingevuld te zijn.")]
        [StringLength(30, ErrorMessage = "Naam mag maximaal 30 karakters bevatten.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Beschrijving mag maximaal 500 karakters bevatten.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Geen geldige prijs.")]
        [Range(0, Double.MaxValue, ErrorMessage = "Prijs moet een positief getal zijn.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public ItemType Type { get; set; }

        public byte[] DisplayPicture { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerID { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}