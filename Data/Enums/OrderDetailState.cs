using System.ComponentModel.DataAnnotations;
/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum OrderDetailState
    {
        Unknown = 0,

        [Display(Name = "Payment")]
        Payment = 10,
    }
}