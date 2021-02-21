using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    /// <summary>
    /// Vic
    /// </summary>
    public interface IAdvertLineManager
    {
        AdvertLine GetAdvertLine(Order order);

        AdvertLine GetAdvertLine(int id);

        AdvertLine GetAdvertLineByAdvertID(int advertID);

        AdvertLine CreateAdvertLine(AdvertLine advertLine);

        AdvertLine EditAdvertLine(AdvertLine advertLine);

        void DeleteAdvertLine(int id);
    }
}