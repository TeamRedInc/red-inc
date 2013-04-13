using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using redinc_reboot.Models;
using WebMatrix.WebData;

namespace redinc_reboot.Controllers
{
    public class ProgressReportsController : Controller
    {
        public ActionResult ProblemSetProgress()
        {
            var viewModel = new ProgressReportModel();
            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress((int)Session["ClassId"], WebSecurity.CurrentUserId, 0);
            return View(viewModel);
        }

        public ActionResult StudentProgress()
        {
            var viewModel = new ProgressReportModel();
            viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], WebSecurity.CurrentUserId);
            return View(viewModel);
        }

        public ActionResult ClassProgress()
        {
            var viewModel = new ProgressReportModel();
            viewModel.ClassProgressList = GlobalStaticVars.StaticCore.StudentProgress((int)Session["ClassId"]);
            return View(viewModel);
        }
    }
}
