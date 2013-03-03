using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using red_inc.Filters;
using red_inc.Models;

namespace red_inc.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class LoginRegisterController : Controller
    {
        public ActionResult Index()
        {
            return View("signin");
        }

        //
        // GET: /LoginRegister/
<<<<<<< HEAD
        public ActionResult Login(string email, string password, string returnUrl)
=======

		//[WebMethod]
        public ActionResult Login(LoginRegisterModel.LoginModel model, string returnUrl)
>>>>>>> 002ea72e73a3675298d70ad6d03322a6892a09cf
        {
            if (GlobalStaticVars.StaticCore.Login(email, password) != null)
            {
                return Redirect(returnUrl);
            }
            return View();
        }

    }
}
