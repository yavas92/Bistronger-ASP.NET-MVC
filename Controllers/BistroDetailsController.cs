using Bistronger.Areas.Identity;
using Bistronger.Data;
using Bistronger.Data.Design;
using Bistronger.Data.Enums;
using Bistronger.Models;
using Bistronger.Models.Businesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Controllers
{

    public class BistroDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBusinessManager _businessManager;
        private readonly ISocialManager _socialManager;
        public BistroDetailsController(ApplicationDbContext context, IBusinessManager businessManager, ISocialManager socialManager)
        {
            _context = context;
            _businessManager = businessManager;
            _socialManager = socialManager;

        }

        public IActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var business = _businessManager.GetBusinessForDetails(id.Value);
            var social = _socialManager.GetSocialForBusiness(id.Value);



            if (business == null)
            {
                return NotFound();
            }
            BusinessDetailViewModel vm = new BusinessDetailViewModel
            {
                ID = business.ID,
                OwnerID = business.OwnerID,
                Name = business.Name,
                City = business.City,
                HouseNumber = business.HouseNumber,
                Street = business.Street,
                DisplayPicture = business.DisplayPicture,
                Type = business.Type,
                Zipcode = business.Zipcode,
                Omschrijving = business.Omschrijving,
                BusinessHours = _businessManager.GetBusinessHours(business.ID).Data.ToList(),
                SocialMedia = social
            };




            return View(vm);
        }
    }
}
