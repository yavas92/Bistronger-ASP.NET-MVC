using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data
{
    public class Business
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(30, ErrorMessage = "Naam mag maximaal 30 karakters bevatten.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Straat is verplicht")]
        [StringLength(30, ErrorMessage = "Straatnaam mag maximaal 30 karakters bevatten.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Huisnummer is ongeldig.")]
        public int HouseNumber { get; set; }

        [StringLength(5, ErrorMessage = "Busnummer mag maximaal 5 karakters bevatten.")]
        public string Mailbox { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Range(1000, 9999, ErrorMessage = "Postcode is ongeldig.")]
        public int Zipcode { get; set; }

        [Required(ErrorMessage = "Stad is verplicht")]
        [StringLength(30, ErrorMessage = "Stad mag maximaal 30 karakters bevatten.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zaak type is verplicht")]
        public BusinessType Type { get; set; }
        public byte[] DisplayPicture { get; set; }

        [StringLength(500, ErrorMessage = "Omschrijving mag maximaal 500 karakters bevatten.")]
        public string Omschrijving { get; set; }
    }
}