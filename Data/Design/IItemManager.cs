using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn + Vic
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IItemManager
    {
        DataSet<Item> GetItems();

        DataSet<Item> GetItems(List<MenuItem> menuItems, string ownerID = null);

        Item GetItem(int id);

        Item CreateItem(Item item);

        Item EditItem(Item item);

        void DeleteItem(int id);
    }
}
