using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bistronger.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{errorCode}")]
        public IActionResult HttpStatusCodeHandler(int errorCode)
        {
            switch (errorCode)
            {
                case 400:
                    return View("BadRequest");

                case 404:
                    return View("PageNotFound");

                case 500:
                    return View("InternalServerError");

                default:
                    return View("Default");
            }
        }
    }
}