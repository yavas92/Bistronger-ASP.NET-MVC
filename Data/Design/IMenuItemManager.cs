using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IMenuItemManager
    {
        DataSet<MenuItem> GetMenuItems(int id = -1);
        MenuItem GetMenuItem(int id);
        MenuItem GetMenuItemByMenuIdAndItemId(int menuId, int itemId);
        MenuItem CreateMenuItem(MenuItem menu);
        MenuItem EditMenuItem(MenuItem menu);
        void DeleteMenuItem(int id);
    }
}
