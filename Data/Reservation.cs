using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data
{
    public class Reservation
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Table))]
        public int TableID { get; set; }

        public Table TableReservation { get; set; }

        [Required(ErrorMessage = "Aantal gasten is verplicht.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Het gekozen aantal is ongeldig.")]
        public int AmountOfGuests { get; set; } = 1;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}")]
        [Required(ErrorMessage = "Verplicht om een start-datum te selecteren.")]
        public DateTime? ReservationDateFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}")]
        [Required(ErrorMessage = "Verplicht om een eind-datum te selecteren.")]
        public DateTime? ReservationDateTo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}