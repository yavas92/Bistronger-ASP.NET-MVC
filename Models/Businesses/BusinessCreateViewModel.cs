using Bistronger.Data;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Models.Businesses
{
    public class BusinessCreateViewModel
    {
        [HiddenInput]
        public int ID { get; set; }
        [HiddenInput]
        public string OwnerID { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(30, ErrorMessage = "Naam mag maximaal 30 karakters bevatten.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Straatnaam is verplicht")]
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
        public string City
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Zaak type is verplicht")]
        public BusinessType Type { get; set; }
        public IFormFile DisplayPicture { get; set; }

        public List<BusinessHour> BusinessHours { get; set; }

        [StringLength(500, ErrorMessage = "Omschrijving mag maximaal 500 karakters bevatten.")]
        public string Omschrijving { get; set; }

        public Social Social { get; set; }

        public int? MenuID { get; set; }

        public BusinessCreateViewModel()
        {
            BusinessHours = new List<BusinessHour>();
            foreach (DayOfWeek day in (BusinessType[])Enum.GetValues(typeof(DayOfWeek)))
            {
                if (day == DayOfWeek.Saturday)
                {
                    BusinessHours.Add(new BusinessHour
                    {
                        Day = DayOfWeek.Sunday,
                    });
                }
                else
                {
                    BusinessHours.Add(new BusinessHour
                    {
                        Day = day + 1,
                    });
                }
            }
        }
    }
}
