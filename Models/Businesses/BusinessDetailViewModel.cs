using Bistronger.Data;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Models.Businesses
{
    public class BusinessDetailViewModel
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string Street { get; set; }

        public int HouseNumber { get; set; }
        public string Mailbox { get; set; }

        public int Zipcode { get; set; }

        public string City
        {
            get;
            set;
        }

        public BusinessType Type { get; set; }
        public byte[] DisplayPicture { get; set; }
        public List<BusinessHour> BusinessHours { get; set; }

        public string Omschrijving { get; set; }
        public Social SocialMedia { get; set; }

        public int MenuID { get; set; }
    }
}
