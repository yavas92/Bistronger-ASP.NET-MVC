
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Models.Businesses;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Bistronger.Areas.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Bistronger.Data.Enums;

/// <summary>
/// Stijn
/// </summary>
namespace Bistronger
{
    [Authorize]
    public class BusinessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBusinessManager _businessManager;
        private readonly ISocialManager _socialManager;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusinessMenuManager _businessMenuManager;

        public BusinessesController(ApplicationDbContext context, IBusinessManager businessManager, ISocialManager socialManager, UserManager<ApplicationUser> userManager
            , IBusinessMenuManager businessMenuManager)
        {
            _context = context;
            _businessManager = businessManager;
            _socialManager = socialManager;
            _userManager = userManager;
            _businessMenuManager = businessMenuManager;
        }

        [AllowAnonymous]
        // GET: Businesses
        public IActionResult Index(int skipAmount, BusinessFilter filter)
        {
            if (filter == null)
                filter = new BusinessFilter();

            var indexVM = new BusinessIndexViewModel
            {
                Businesses = _businessManager.GetBusinesses(filter),
                Filter = filter
            };
            return View(indexVM);
        }

        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        // GET: Businesses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = _businessManager.GetBusiness(id.Value);
            var social = _socialManager.GetSocialForBusiness(id.Value);
            var busMenu = _businessMenuManager.GetBusinessMenuForBusiness(id.Value);

            if (business == null)
            {
                return NotFound();
            }
            BusinessDetailViewModel vm = new BusinessDetailViewModel
            {
                ID = business.ID,
                OwnerID = business.OwnerID,
                Email = business.Owner.Email,
                Name = business.Name,
                City = business.City,
                HouseNumber = business.HouseNumber,
                Mailbox = business.Mailbox,
                Street = business.Street,
                DisplayPicture = business.DisplayPicture,
                Type = business.Type,
                Zipcode = business.Zipcode,
                Omschrijving = business.Omschrijving,
                BusinessHours = _businessManager.GetBusinessHours(business.ID).Data.ToList(),
                SocialMedia = social,
                MenuID = busMenu != null ? busMenu.MenuID : 0
            };

            return View(vm);
        }

        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        // GET: Businesses/Create
        public IActionResult Create()
        {
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            BusinessCreateViewModel vm = new BusinessCreateViewModel();
            return View(vm);
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.RestaurantOwner)]
        public IActionResult Create(BusinessCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var business = new Business
                {
                    OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Name = model.Name,
                    Street = model.Street,
                    HouseNumber = model.HouseNumber,
                    Mailbox = model.Mailbox,
                    Zipcode = model.Zipcode,
                    City = model.City,
                    Type = model.Type,
                    Omschrijving = model.Omschrijving
                };

                if (model.DisplayPicture != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.DisplayPicture.CopyTo(ms);
                        business.DisplayPicture = ms.ToArray();
                    }
                }

                var social = model.Social;
                social.Business = business;
                social.BusinessID = business.ID;

                _businessManager.CreateBusiness(business);
                _socialManager.CreateSocial(social);

                if ((model.MenuID != null) && model.MenuID.Value > 0)
                {
                    var busMenu = new BusinessMenu
                    {
                        BusinessID = business.ID,
                        MenuID = model.MenuID.Value
                    };
                    _businessMenuManager.CreateBusinessMenu(busMenu);
                }


                foreach (BusinessHour hour in model.BusinessHours)
                {
                    hour.BusinessID = business.ID;
                    _businessManager.AddBusinessHours(hour);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(model);
        }

        // GET: Businesses/Edit/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = _businessManager.GetBusiness(id.Value);
            var social = _socialManager.GetSocialForBusiness(id.Value);
            var businessMenu = _businessMenuManager.GetBusinessMenuForBusiness(id.Value);
            if (business == null)
            {
                return NotFound();
            }
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var businesVM = new BusinessEditViewModel
            {
                ID = business.ID,
                OwnerID = business.OwnerID,
                Email = business.Owner.Email,
                Name = business.Name,
                Street = business.Street,
                HouseNumber = business.HouseNumber,
                Mailbox = business.Mailbox,
                Zipcode = business.Zipcode,
                City = business.City,
                Type = business.Type,
                DisplayPictureBytes = business.DisplayPicture,
                BusinessHours = _businessManager.GetBusinessHours(business.ID).Data.ToList(),
                Omschrijving = business.Omschrijving,
                Social = social,
                MenuID = businessMenu == null ? null : businessMenu.MenuID,
                SocialID = social.ID
            };

            if (businessMenu != null)
                businesVM.BusMenuID = businessMenu.ID;

            return View(businesVM);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Edit(int id, BusinessEditViewModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var business = new Business
                {
                    ID = model.ID,
                    OwnerID = model.OwnerID,
                    Name = model.Name,
                    Street = model.Street,
                    HouseNumber = model.HouseNumber,
                    Mailbox = model.Mailbox,
                    Zipcode = model.Zipcode,
                    City = model.City,
                    Type = model.Type,
                    DisplayPicture = model.DisplayPictureBytes,
                    Omschrijving = model.Omschrijving
                };

                //If new image we need to overwrite it
                if (model.DisplayPictureUpload != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.DisplayPictureUpload.CopyTo(ms);
                        business.DisplayPicture = ms.ToArray();
                    }
                }

                var social = model.Social;
                social.Business = business;
                social.BusinessID = business.ID;
                social.ID = model.SocialID;


                var busMenu = new BusinessMenu();


                if (model.MenuID > 0)
                {
                    busMenu.ID = model.BusMenuID;
                    busMenu.BusinessID = model.ID;
                    busMenu.MenuID = model.MenuID.Value;
                }

                try
                {
                    _businessManager.EditBusiness(business);
                    _socialManager.EditSocial(social);

                    //If a menu was selected we either edit or create one
                    if (model.MenuID != -1)
                    {
                        //if menu exists we edit it, else we create it
                        if (busMenu.ID > 0)
                            _businessMenuManager.EditBusinessMenu(busMenu);
                        else
                            _businessMenuManager.CreateBusinessMenu(busMenu);
                    }

                    foreach (BusinessHour hour in model.BusinessHours)
                    {
                        hour.BusinessID = business.ID;
                        _businessManager.EditBusinessHour(hour);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessExists(business.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(model);
        }

        // GET: Businesses/Delete/5
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = _businessManager.GetBusiness(id.Value);

            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(UserRoleType.Admin, UserRoleType.RestaurantOwner)]
        public ActionResult DeleteConfirmed(int id)
        {
            _businessManager.DeleteBusiness(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.ID == id);
        }
    }
}