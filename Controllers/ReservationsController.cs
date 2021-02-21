using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Data.Enums;
using Bistronger.Models;
using Bistronger.Models.Reservations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// Vic & Scott
/// </summary>
namespace Bistronger.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderManager _orderManager;
        private readonly IBusinessManager _businessManager;
        private readonly IReservationManager _reservationManager;
        private readonly ITableManager _tableManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IOrderManager orderManager, IBusinessManager businessManager, IReservationManager reservationManager, ITableManager tableManager)
        {
            _context = context;
            _orderManager = orderManager;
            _businessManager = businessManager;
            _reservationManager = reservationManager;
            _userManager = userManager;
            _tableManager = tableManager;
        }

        [AuthorizeRoles(UserRoleType.Customer, UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Index(ReservationIndexViewModel vm)
        {
            if (vm == null)
            {
                return NotFound();
            }

            var indexVM = new ReservationIndexViewModel
            {
                Reservations = _reservationManager.GetReservations()
            };

            return View(indexVM);
        }

        // GET: Tables/Add
        [AuthorizeRoles(UserRoleType.Customer)]
        public IActionResult Create(int? businessID)
        {
            if (businessID == null || !businessID.HasValue)
            {
                return NotFound();
            }
            Business business = _businessManager.GetBusinessForDetails(businessID.Value);
            if (business == null)
            {
                return NotFound();
            }
            var addVM = new ReservationCreateViewModel
            {
                FoundTable = true,
                BusinessID = businessID.Value,
                BusinessToReserve = business,
                BusinessHours = _businessManager.GetBusinessHours(business.ID).Data,
            };

            return View(addVM);
        }

        // POST: Tables/ADD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Customer)]
        public IActionResult Create(ReservationCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Table table = _tableManager.GetFreeTable(vm.BusinessID, vm.AmountOfGuests, vm.ReservationDate.Value);
                if (table != null)
                {
                    Reservation newReservation = new Reservation
                    {
                        UserID = userID,
                        TableID = table.ID,
                        AmountOfGuests = vm.AmountOfGuests,
                        ReservationDateFrom = vm.ReservationDate,
                        ReservationDateTo = vm.ReservationDate.Value.AddHours(1)
                    };
                    _reservationManager.CreateReservation(newReservation);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    vm.FoundTable = false;
                }
            }

            // Can't pass objects with vm -> opnieuw opvragen
            Business business = _businessManager.GetBusinessForDetails(vm.BusinessID);
            if (business == null)
            {
                return NotFound();
            }
            vm.BusinessToReserve = business;
            vm.BusinessHours = _businessManager.GetBusinessHours(vm.BusinessID).Data;
            return View(vm);
        }
    }
}