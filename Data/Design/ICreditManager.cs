using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    public interface ICreditManager
    {
        decimal GetCreditAmount(string UserID);

        bool AddCredit(string UserID, decimal amount);

        bool SubtractCredit(string UserID, decimal amount);
    }
}