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
    public class ProgressController : Controller
    {
        private static ProgressReportModel viewModel = new ProgressReportModel();

        public ActionResult Set(int id = 0)
        {
            try
            {
                if (viewModel.Class != null)
                {
                    if (viewModel.User != null)
                    {
                        if (id == 0)
                        {
                            return RedirectToAction("StudentProgressView", new { viewModel.User.Id });
                        }
                        else
                        {
                            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress(viewModel.Class.Id,
                                viewModel.User.Id, id);
                        }
                    }
                    else
                    {
                        if (id != 0)
                            viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress(viewModel.Class.Id,
                                WebSecurity.CurrentUserId, id);
                    }
                }
                else
                {
                    if (id != 0)
                        viewModel.ProblemSetProgressList = GlobalStaticVars.StaticCore.ProblemProgress((int)Session["ClassId"],
                            WebSecurity.CurrentUserId, id);
                }
                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        public ActionResult Student(int id = 0)
        {
            try{
                if(viewModel.Class == null)
                {
                    if(id == 0)
                    {
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], WebSecurity.CurrentUserId);
                        return View(viewModel);
                    }
                    else
                    {
                        viewModel.User = new UserData(id);
                        viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress((int)Session["ClassId"], id);
                        return View(viewModel);
                    }
                }
                else
                {
                    viewModel.User = new UserData(id);
                    viewModel.StudentProgressList = GlobalStaticVars.StaticCore.SetProgress(viewModel.Class.Id, id);
                    return View(viewModel);
                }
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        
        public ActionResult Class(int id = 0)
        {
            try
            {
                if ((int) Session["ClassId"] != id || (UserType) Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }
                if (id == 0)
                {
                    viewModel.Class = GlobalStaticVars.StaticCore.GetClassById((int)Session["ClassId"]);
                    viewModel.ClassProgressList = GlobalStaticVars.StaticCore.StudentProgress((int)Session["ClassId"]);
                }
                else
                {
                    viewModel.Class = GlobalStaticVars.StaticCore.GetClassById(id);
                    viewModel.ClassProgressList = GlobalStaticVars.StaticCore.StudentProgress(id);
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
