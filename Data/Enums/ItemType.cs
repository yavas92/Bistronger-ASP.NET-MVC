
using System.ComponentModel.DataAnnotations;
/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum ItemType
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,
        [Display(Name = "Voorgerecht")]
        Voorgerecht = 10,
        [Display(Name = "Hoofdgerecht")]
        Hoofdgerecht = 20,
        [Display(Name = "Nagerecht")]
        Nagerecht = 30,
        [Display(Name = "Drank")]
        Drank = 40,
        [Display(Name = "Alcohol")]
        Alcohol = 50,
        [Display(Name = "Advert")]
        Advert = 60
    }
}