using System.ComponentModel.DataAnnotations;

/// <summary>
/// Abdullah
/// </summary>
namespace Bistronger.Data.Enums
{
    public enum UserRoleType
    {
        Unknown = 0,

        [Display(Name = "Klant")]
        Customer = 10,

        [Display(Name = "Uitbater")]
        RestaurantOwner = 20,

        [Display(Name = "Admin")]
        Admin = 30
    }
}