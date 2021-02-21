using System.ComponentModel.DataAnnotations;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum BusinessOpenStatus
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "Open")]
        Open = 10,

        [Display(Name = "Gesloten")]
        Closed = 20,

        [Display(Name = "Sluit binnekort")]
        ClosesSoon = 30,

        [Display(Name = "Opent binnekort")]
        OpenSoon = 30,
    }
}