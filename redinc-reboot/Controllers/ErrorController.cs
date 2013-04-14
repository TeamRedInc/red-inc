using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace redinc_reboot.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/Unauthorized

        public ActionResult Unauthorized()
        {
            return View();
        }

        //
        // GET: /Error/DoesNotExist
        public ActionResult DoesNotExist()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }
    }
}
