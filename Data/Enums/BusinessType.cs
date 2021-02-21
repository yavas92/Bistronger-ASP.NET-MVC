using System.ComponentModel.DataAnnotations;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum BusinessType
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "Italiaans")]
        Italiaans = 10,

        [Display(Name = "Chinees")]
        Chinees = 20,

        [Display(Name = "Spaans")]
        Spaans = 30,

        [Display(Name = "Belgisch")]
        Belgisch = 40,

        [Display(Name = "Marokkaans")]
        Marokkaans = 50,

        [Display(Name = "Grieks")]
        Grieks = 60,

        [Display(Name = "Turks")]
        Turks = 70,
    }
}