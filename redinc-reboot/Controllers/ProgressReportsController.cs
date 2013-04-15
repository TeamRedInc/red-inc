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
        private static ProgressReportModel viewModel = new ProgressReportModel();

        public ActionResult SetProgressView(int probSetId = 0)
        {
            try
            {
                if (viewModel.Class != null)
                {
                    if (viewModel.User != null)
                    {
                        if (probSetId == 0)
                        {
                            return RedirectToAction("StudentProgressView", "ProgressReports", new { viewModel.User.Id });
                        }
                        else
                        {
                            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress(viewModel.Class.Id,
                                viewModel.User.Id, probSetId);
                        }
                    }
                    else
                    {
                        if (probSetId != 0)
                            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress(viewModel.Class.Id,
                                WebSecurity.CurrentUserId, probSetId);
                    }
                }
                else
                {
                    if (probSetId != 0)
                        viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress((int)Session["ClassId"],
                            WebSecurity.CurrentUserId, probSetId);
                }
                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        public ActionResult StudentProgressView(int userId = 0)
        {
            try{
                if(viewModel.Class == null)
                {
                    if(userId == 0)
                    {
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], WebSecurity.CurrentUserId);
                        return View(viewModel);
                    }
                    else
                    {
                        viewModel.User = new UserData(userId);
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], userId);
                        return View(viewModel);
                    }
                }
                else
                {
                    viewModel.User = new UserData(userId);
                    viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress(viewModel.Class.Id, userId);
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
                if (classId == 0)
                {
                    viewModel.Class = GlobalStaticVars.StaticCore.GetClassById((int)Session["ClassId"]);
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
