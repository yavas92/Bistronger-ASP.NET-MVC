using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    public interface IReservationManager
    {
        DataSet<Reservation> GetReservations();

        DataSet<Reservation> GetReservations(int tableID);

        Reservation GetReservation(int id);

        Reservation CreateReservation(Reservation reservation);

        Reservation EditReservation(Reservation reservation);

        void DeleteReservation(int id);

        bool DoesReservationExistForTable(int tableID, DateTime startTime);
    }
}