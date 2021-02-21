using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn
/// </summary>

namespace Bistronger.Data.Design
{
    public interface IMenuManager
    {
        DataSet<Menu> GetMenus(string ownerID = null);
        Menu GetMenu(int id);
        Menu CreateMenu(Menu menu);
        Menu EditMenu(Menu menu);
        void DeleteMenu(int id);
    }
}
