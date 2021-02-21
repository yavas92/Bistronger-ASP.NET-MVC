using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Models.Orders;
using Bistronger.Data.Design;
using Bistronger.Data.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Bistronger.Areas.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bistronger.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderManager _orderManager;
        private readonly IMenuItemManager _menuItemManager;
        private readonly IBusinessMenuManager _businessMenuManager;
        private readonly IOrderLineManager _orderLineManager;
        private readonly ICreditManager _creditManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, IOrderManager orderManager, IMenuItemManager menuItemManager,
            IBusinessMenuManager businessMenuManager, IOrderLineManager orderLineManager, ICreditManager creditManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _orderManager = orderManager;
            _menuItemManager = menuItemManager;
            _businessMenuManager = businessMenuManager;
            _orderLineManager = orderLineManager;
            _creditManager = creditManager;
            _userManager = userManager;

        }
        [AuthorizeRoles(UserRoleType.Customer, UserRoleType.Admin)]
        // GET: Orders
        public IActionResult Index()
        {
            var model = new OrderIndexViewModel
            {
                Orders = _orderManager.GetOrders()
            };

            return View(model);
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id, bool succesful = true, OrderDetailState state = OrderDetailState.Unknown)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = !User.IsInRole(UserRoleType.RestaurantOwner.ToString()) ? _orderManager.GetOrder(id.Value) : _orderManager.GetOrderDetailsAsBistro(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            OrderDetailViewModel vm = new OrderDetailViewModel
            {
                ID = order.ID,
                UserID = order.UserID,
                User = _userManager.FindByIdAsync(order.UserID).Result,
                OrderType = order.OrderType,
                Status = order.Status,
                CreatedOn = order.CreatedOn,
                OrderLines = _orderLineManager.GetOrderLines(order).Data,
                BusinessID = order.BusinessID,
                Successful = succesful,
                State = state == OrderDetailState.Payment ? OrderDetailState.Payment : OrderDetailState.Unknown
            };

            return View(vm);
        }

        [AuthorizeRoles(UserRoleType.Customer)]
        // GET: Orders/Create
        public IActionResult Create(int id)
        {
            OrderCreateViewModel model = new OrderCreateViewModel();
            model.BusinessID = id;
            model.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            model.BusMenu = _businessMenuManager.GetBusinessMenuForBusiness(id);
            if (model.BusMenu != null)
            {
                model.MenuItems = _menuItemManager.GetMenuItems(model.BusMenu.MenuID).Data;

                model.OrderLines = new List<OrderLine>();

                foreach (var item in model.MenuItems)
                {
                    OrderLine newOrderLine = new OrderLine
                    {
                        ItemID = item.ItemID,
                        Amount = 0
                    };

                    model.OrderLines.Add(newOrderLine);
                }
            }

            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Customer)]
        public IActionResult Create(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OrderLines.All(m => m.Amount == 0))
                {
                    return RedirectToAction(nameof(Index));
                }

                Order order = new Order
                {
                    Status = OrderStatus.Unpaid,
                    OrderType = OrderType.OnlineOrder,
                    UserID = model.UserID,
                    BusinessID = model.BusinessID
                };

                _orderManager.CreateOrder(order);

                foreach (var item in model.OrderLines)
                {
                    if (item.Amount > 0)
                    {
                        OrderLine newOrderLine = new OrderLine
                        {
                            ItemID = item.ItemID,
                            OrderID = order.ID,
                            Amount = item.Amount
                        };

                        _orderLineManager.CreateOrderLine(newOrderLine);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", model.UserID);
            return View(model);
        }

        [AuthorizeRoles(UserRoleType.Customer, UserRoleType.Admin)]
        // GET: Orders/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _orderManager.GetOrder(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            OrderDetailViewModel vm = new OrderDetailViewModel
            {
                ID = order.ID,
                UserID = order.UserID,
                User = _userManager.FindByIdAsync(order.UserID).Result,
                BusinessID = order.BusinessID,
                OrderType = order.OrderType,
                Status = order.Status,
                CreatedOn = order.CreatedOn,
                OrderLines = _orderLineManager.GetOrderLines(order).Data
            };

            return View(vm);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Customer, UserRoleType.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }

        public IActionResult Overview()
        {
            var model = new OrderOverviewViewModel
            {
                Orders = _orderManager.GetOrdersForBistros()
            };

            return View(model);
        }

        [AuthorizeRoles(UserRoleType.Customer)]
        public IActionResult Payment(int id)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userCredit = _creditManager.GetCreditAmount(userID);

            var order = _orderManager.GetOrder(id);

            var orderLines = _orderLineManager.GetOrderLines(order).Data;
            decimal? creditsToPay = 0;

            foreach (var item in orderLines)
            {
                creditsToPay += item.Amount * item.Item.Price;
            }

            if (_creditManager.SubtractCredit(userID, creditsToPay.Value))
            {
                order.Status = OrderStatus.InProgress;
                _orderManager.EditOrder(order);
                return RedirectToAction(nameof(Details), new { id = id, succesful = true, state = OrderDetailState.Payment });
            }
            else
            {
                return RedirectToAction(nameof(Details), new { id = id, succesful = false, state = OrderDetailState.Payment });
            }
        }
    }
}
