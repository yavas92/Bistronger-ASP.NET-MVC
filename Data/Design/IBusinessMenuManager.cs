using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IBusinessMenuManager
    {
        DataSet<BusinessMenu> GetBusinessMenus(int id);
        BusinessMenu GetBusinessMenu(int id);
        BusinessMenu GetBusinessMenuForBusiness(int id);
        BusinessMenu CreateBusinessMenu(BusinessMenu menu);
        BusinessMenu EditBusinessMenu(BusinessMenu menu);
        void DeleteBusinessMenu(int id);
    }
}
