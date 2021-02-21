using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Vic
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IAdvertManager
    {
        DataSet<Advert> GetAdverts(bool checkForOwnership = false);

        DataSet<Advert> GetAdverts(AdvertFilter filter);

        Advert GetAdvert(int id);

        Advert CreateAdvert(Advert advert);

        Advert EditAdvert(Advert advert);

        void DeleteAdvert(int id);

        void ReduceAdvertViewCount();
    }
}