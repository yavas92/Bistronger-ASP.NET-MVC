using Bistronger.Areas.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bistronger.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessHour> BusinessHours { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<BusinessMenu> BusinessMenus { get; set; }
        public DbSet<CreditPurchase> CreditPurchases { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AdvertLine> AdvertLines { get; set; }
        public DbSet<Package> Packages { get; set; }
    }
}