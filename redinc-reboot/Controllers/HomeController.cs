﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using redinc_reboot.Models;
using core.Modules.ProblemSet;

namespace redinc_reboot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PythonTestPage()
        {
            ViewBag.Message = "Python test page.";

            return View();
        }

        //
        // POST: /Home/PythonTestPage

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PythonTestPage(CodeModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to execute code
                model.OutputCode = GlobalStaticVars.StaticCore.ExecutePythonCode(model.InputCode);
            }
            
            // Redisplay form with output
            return View(model);
        }

        public ActionResult ProblemSet(int id = 0)
        {
            ViewBag.Message = "Modify the problem set.";
            ProblemSetData set = GlobalStaticVars.StaticCore.GetSetById(id);
            IList<ProblemSetData> prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(id);
            ProblemSetViewModel model = new ProblemSetViewModel();
            model.Set = set;
            model.Prereqs = prereqs;
            return View(model);
        }

        public ActionResult EditProblemSet(ProblemSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                GlobalStaticVars.StaticCore.ModifySet(model.Set);
            }

            return RedirectToAction("ProblemSet", new { id = model.Set.Id });
        }

        public ActionResult NewPrereq()
        {
            return PartialView("EditorTemplates/PrereqRow", new ProblemSetData());
        }
    }
}
