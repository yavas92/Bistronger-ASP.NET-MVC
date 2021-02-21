using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Bistronger.Data.Enums;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IBusinessManager
    {
        DataSet<Business> GetBusinesses();

        DataSet<Business> GetBusinesses(BusinessFilter filter);

        Business GetBusiness(int id);
        Business GetBusinessForDetails(int id);

        Business CreateBusiness(Business business);

        Business EditBusiness(Business business);

        void DeleteBusiness(int id);

        BusinessHour AddBusinessHours(BusinessHour businessHour);

        DataSet<BusinessHour> GetBusinessHours(int businessID);

        BusinessHour GetBusinessHourToday(int businessID, DayOfWeek day);

        BusinessHour EditBusinessHour(BusinessHour businessHour);

        DataSet<BusinessHour> GetAllBusinessHours();

        BusinessOpenStatus GetBusinessOpenStatus(int businessID);
    }
}