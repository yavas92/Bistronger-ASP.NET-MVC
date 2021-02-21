using Bistronger.Areas.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data
{
    public class Menu
    {
        public int ID { get; set; }
        public string Naam { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}