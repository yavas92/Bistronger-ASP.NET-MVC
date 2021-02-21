using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum PackageType
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "Advertenties")]
        Advert = 10,

        [Display(Name = "Credits")]
        Credits = 20
    }
}