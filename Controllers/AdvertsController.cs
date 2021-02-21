using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Data.Enums;
using Bistronger.Areas.Identity;
using Bistronger.Models.Adverts;
using System.Security.Claims;
using System.IO;

namespace Bistronger.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdvertManager _advertManager;
        private readonly IBusinessManager _businessManager;
        private readonly ICreditManager _creditManager;
        private readonly IOrderManager _orderManager;
        private readonly IAdvertLineManager _advertLineManager;
        private readonly IPackageManager _packageManager;

        public AdvertsController(ApplicationDbContext context,
            IAdvertManager advertManager, IBusinessManager businessManager, ICreditManager creditManager,
            IOrderManager orderManager, IAdvertLineManager advertLineManager, IPackageManager packageManager)
        {
            _context = context;
            _advertManager = advertManager;
            _businessManager = businessManager;
            _creditManager = creditManager;
            _orderManager = orderManager;
            _advertLineManager = advertLineManager;
            _packageManager = packageManager;
        }

        // GET: Adverts
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Index(AdvertFilter filter)
        {
            if (filter == null)
                filter = new AdvertFilter();

            var indexVM = new AdvertIndexViewModel
            {
                Adverts = _advertManager.GetAdverts(filter),
                Filter = filter
            };
            return View(indexVM);
        }

        // GET: Adverts/Details/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advert = _advertManager.GetAdvert(id.Value);
            if (advert == null)
            {
                return NotFound();
            }
            AdvertDetailViewModel vm = new AdvertDetailViewModel
            {
                Advert = advert
            };

            return View(vm);
        }

        // GET: Adverts/Purchase
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Purchase(bool succesful = true)
        {
            ViewData["UserID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vm = new AdvertPurchaseViewModel()
            {
                Successful = succesful,
                Businesses = _businessManager.GetBusinesses().Data,
                Packages = _packageManager.GetPackages(PackageType.Advert).Data
            };

            return View(vm);
        }

        // POST: Adverts/Purchase
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Purchase(AdvertPurchaseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Package selectedPackage = _packageManager.GetPackage(vm.SelectedPackageID);
                if (selectedPackage != null)
                {
                    if (_creditManager.SubtractCredit(userID, selectedPackage.Price))
                    {
                        Advert newAdvert = new Advert
                        {
                            BusinessID = vm.BusinessID,
                            Shows = selectedPackage.Amount
                        };
                        if (vm.PhotoData != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                vm.PhotoData.CopyTo(ms);
                                newAdvert.PhotoData = ms.ToArray();
                            }
                        }
                        newAdvert = _advertManager.CreateAdvert(newAdvert);

                        // Create empty order
                        Order advertOrder = new Order
                        {
                            UserID = userID,
                            OrderType = OrderType.AdvertPurchase,
                            Status = OrderStatus.Finished
                        };
                        advertOrder = _orderManager.CreateOrder(advertOrder);

                        // Create advertline and link to order
                        AdvertLine advertLine = new AdvertLine
                        {
                            OrderID = advertOrder.ID,
                            AdvertID = newAdvert.ID,
                            OriginalShows = newAdvert.Shows,
                            PricePaid = selectedPackage.Price
                        };
                        _advertLineManager.CreateAdvertLine(advertLine);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Purchase),
                            new
                            {
                                succesful = false
                            }
                        );
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Purchase),
                        new
                        {
                            succesful = false
                        }
                    );
                }
            }

            ViewData["UserID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(vm);
        }

        // GET: Adverts/Delete/5
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advert = _advertManager.GetAdvert(id.Value);
            if (advert == null)
            {
                return NotFound();
            }

            return View(advert);
        }

        // POST: Adverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin)]
        public IActionResult DeleteConfirmed(int id)
        {
            _advertManager.DeleteAdvert(id);
            return RedirectToAction(nameof(Index));
        }
    }
}