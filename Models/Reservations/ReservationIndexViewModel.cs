using Bistronger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Vic & Scott
/// </summary>
namespace Bistronger.Models.Reservations
{
    public class ReservationIndexViewModel
    {
        public DataSet<Reservation> Reservations { get; set; }
    }
}
