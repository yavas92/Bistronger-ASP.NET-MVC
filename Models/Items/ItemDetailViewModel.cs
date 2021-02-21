using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn + Vic
/// </summary>
namespace Bistronger.Models.Items
{
    public class ItemDetailViewModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ItemType Type { get; set; }
        public byte[] DisplayPicture { get; set; }
    }
}