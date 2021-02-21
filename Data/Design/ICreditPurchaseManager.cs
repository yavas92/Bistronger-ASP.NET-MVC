using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Vic & Scott
    /// </summary>
    public interface ICreditPurchaseManager
    {
        DataSet<CreditPurchase> GetCreditPurchases();

        DataSet<CreditPurchase> GetCreditPurchases(CreditFilter filter);

        CreditPurchase GetCreditPurchase(int id);

        CreditPurchase CreateCreditPurchase(CreditPurchase creditPurchase);

        CreditPurchase EditCreditPurchase(CreditPurchase creditPurchase);

        void DeleteCreditPurchase(int id);
    }
}