using Bistronger.Areas.Identity;
using Bistronger.Areas.Identity.Manage;
using Bistronger.Data;
using Bistronger.Data.Design;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUserRole, UserRoleManager>();
            services.AddScoped<IBusinessManager, BusinessManager>();
            services.AddScoped<IAdvertManager, AdvertManager>();
            services.AddScoped<IBusinessMenuManager, BusinessMenuManager>();
            services.AddScoped<IItemManager, ItemManager>();
            services.AddScoped<IMenuItemManager, MenuItemManager>();
            services.AddScoped<IMenuManager, MenuManager>();
            services.AddScoped<IOrderLineManager, OrderLineManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ISocialManager, SocialManager>();
            services.AddScoped<ICreditPurchaseManager, CreditPurchaseManager>();
            services.AddScoped<ICreditManager, CreditManager>();
            services.AddScoped<ITableManager, TableManager>();
            services.AddScoped<IReservationManager, ReservationManager>();
            services.AddScoped<IAdvertLineManager, AdvertLineManager>();
            services.AddScoped<IPackageManager, PackageManager>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserRole userRoleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                // Requires "Production" as environment in launchSettings
                app.UseStatusCodePagesWithRedirects("/error/{0}");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });

            userRoleManager.CreateRoleIfNotExists();
        }
    }
}