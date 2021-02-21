using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Models.Credits;
using Bistronger.Data.Design;
using System.Security.Claims;
using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Controllers
{
    [Authorize]
    public class CreditsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICreditPurchaseManager _creditPurchaseManager;
        private readonly ICreditManager _creditManager;
        private readonly IPackageManager _packageManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreditsController(ApplicationDbContext context, ICreditPurchaseManager creditPurchaseManager,
            ICreditManager creditManager, UserManager<ApplicationUser> userManager, IPackageManager packageManager)
        {
            _context = context;
            _creditPurchaseManager = creditPurchaseManager;
            _creditManager = creditManager;
            _userManager = userManager;
            _packageManager = packageManager;
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner, UserRoleType.Customer)]
        // GET: Credits
        public IActionResult Index(CreditFilter filter)
        {
            if (filter == null)
                filter = new CreditFilter();

            var indexVM = new CreditIndexViewModel
            {
                User = _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result,
                CreditPurchases = _creditPurchaseManager.GetCreditPurchases(filter),
                Filter = filter
            };

            return View(indexVM);
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner, UserRoleType.Customer)]
        // GET: Credits/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = _creditPurchaseManager.GetCreditPurchase(id.Value);
            if (purchase == null)
            {
                return NotFound();
            }
            CreditDetailViewModel vm = new CreditDetailViewModel
            {
                User = _userManager.FindByIdAsync(purchase.UserID).Result,
                ID = purchase.ID,
                UserID = purchase.UserID,
                Credits = purchase.Credits,
                TotalPrice = purchase.TotalPrice,
                Method = purchase.Method,
                Status = purchase.Status,
                CreatedOn = purchase.CreatedOn
            };

            return View(vm);
        }

        [AuthorizeRoles(UserRoleType.RestaurantOwner, UserRoleType.Customer)]
        // GET: Credits/Purchase
        public IActionResult Purchase()
        {
            ViewData["UserID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CreditPurchaseViewModel vm = new CreditPurchaseViewModel()
            {
                Packages = _packageManager.GetPackages(PackageType.Credits).Data
            };
            return View(vm);
        }

        // POST: Credits/Purchase
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.RestaurantOwner, UserRoleType.Customer)]
        public IActionResult Purchase(CreditPurchaseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Randomly generate payment method and status (for now?)
                // -------------
                Random r = new Random();
                var pmethods = Enum.GetValues(typeof(PaymentMethod));
                var ostatuses = Enum.GetValues(typeof(OrderStatus));
                // -------------

                Package selectedPackage = _packageManager.GetPackage(vm.SelectedPackageID);
                if (selectedPackage != null)
                {
                    CreditPurchase purchase = new CreditPurchase()
                    {
                        UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Credits = selectedPackage.Amount,
                        TotalPrice = selectedPackage.Price,
                        Method = (PaymentMethod)pmethods.GetValue(r.Next(1, pmethods.Length)),
                        Status = (OrderStatus)ostatuses.GetValue(r.Next(1, ostatuses.Length))
                    };

                    if (_creditPurchaseManager.CreateCreditPurchase(purchase) != null)
                    {
                        if (!_creditManager.AddCredit(purchase.UserID, purchase.Credits))
                        {
                            throw new DbUpdateException();
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new NullReferenceException(nameof(selectedPackage));
                }
            }
            ViewData["UserID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(vm);
        }

        private bool CreditPurchaseExists(int id)
        {
            return _context.CreditPurchases.Any(e => e.ID == id);
        }
    }
}