using System.ComponentModel.DataAnnotations;

namespace Bistronger.Data.Enums
{
    /// <summary>
    /// aangemaakt door Scott
    /// </summary>
    public enum OrderType
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "Tafel reservatie")]
        TableReservation = 10,

        [Display(Name = "Online bestelling")]
        OnlineOrder = 20,

        [Display(Name = "Advertentie")]
        AdvertPurchase = 30
    }
}