using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Abdullah
/// </summary>
namespace Bistronger.Areas.Identity
{
    /// <summary>
    /// Abdullah
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30, ErrorMessage = "Voornaam mag maximaal 30 karakters bevatten.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Achternaam mag maximaal 30 karakters bevatten.")]
        public string LastName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Credit { get; set; } = 0.0M;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}