using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using redinc_reboot.Models;
using core.Modules.User;
using WebMatrix.WebData;

namespace redinc_reboot.Controllers
{
    public class ProgressReportsController : Controller
    {
        int classId = 0;

        public ActionResult ProblemSetProgress()
        {
            var viewModel = new ProgressReportModel();
            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress((int)Session["ClassId"], WebSecurity.CurrentUserId, 0);
            return View(viewModel);
        }

        public ActionResult StudentProgress(int userId = 0)
        {
            try{
                ProgressReportModel viewModel = new ProgressReportModel();
                if(viewModel.Class == null)
                {
                    if(userId == 0)
                    {
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], WebSecurity.CurrentUserId);
                        return View(viewModel);
                    }
                    else
                    {
                        viewModel.User.Id = userId;
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], userId);
                        return View(viewModel);
                    }
                }
                else
                {
                    viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress(viewModel.Class.Id, WebSecurity.CurrentUserId);
                    return View(viewModel);
                }
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        
        public ActionResult ClassProgressView(int classId = 0)
        {
            try
            {
                if ((int) Session["ClassId"] != classId || (UserType) Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }
                ProgressReportModel viewModel = new ProgressReportModel();
                if (classId == 0)
                {
                    viewModel.ClassProgressList = GlobalStaticVars.StaticCore.StudentProgress((int)Session["ClassId"]);
                }
                else
                {
                    viewModel.Class = GlobalStaticVars.StaticCore.GetClassById(classId);
                    viewModel.ClassProgressList = GlobalStaticVars.StaticCore.StudentProgress(classId);
                }
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}
