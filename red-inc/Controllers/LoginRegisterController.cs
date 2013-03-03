﻿using System;
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

		//[WebMethod]
        public ActionResult Login(LoginRegisterModel.LoginModel model, string returnUrl)
        {
            if (GlobalStaticVars.StaticCore.Login(model.Email, model.Password))
            {
                return Redirect(returnUrl);
            }
            return View();
        }

    }
}
