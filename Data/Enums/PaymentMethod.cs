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
    public enum PaymentMethod
    {
        [Display(Name = "Onbekend")]
        Unknown = 0,

        [Display(Name = "PayPal")]
        Paypal = 10,

        [Display(Name = "Overschrijving")]
        BankTransfer = 20,

        [Display(Name = "Kredietkaart")]
        CreditCard = 30
    }
}