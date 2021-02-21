using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bistronger.Data
{
    /// <summary>
    /// aangemaakt door Vic
    /// </summary>
    public class MenuItem
    {
        public int ID { get; set; }
        [Required]
        [ForeignKey(nameof(Menu))]
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemID { get; set; }
        public Item Item { get; set; }
    }
}
