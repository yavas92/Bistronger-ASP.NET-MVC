using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Joren
    /// </summary>
    public interface ITableManager
    {
        DataSet<Table> GetTables(int businessID);

        Table GetTable(int id);

        Table CreateTable(Table table);

        Table EditTable(Table table);

        void DeleteTable(int id);

        void SetTableAvailable(int id);

        void SetTableUnavailable(int id);

        Table GetFreeTable(int businessID, int peopleAmount, DateTime reservationDate);

        void UpdateTablesAvailability(int businessID);
    }
}