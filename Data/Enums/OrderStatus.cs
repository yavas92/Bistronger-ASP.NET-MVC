using System.ComponentModel.DataAnnotations;

namespace Bistronger.Data.Enums
{
    /// <summary>
    /// aangemaakt door Scott
    /// </summary>
    public enum OrderStatus
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "Bezig")]
        InProgress = 10,

        [Display(Name = "Onbetaald")]
        Unpaid = 20,

        [Display(Name = "Betaald")]
        Paid = 25,

        [Display(Name = "Geannuleerd")]
        Cancelled = 30,

        [Display(Name = "Klaar")]
        Finished = 40
    }
}