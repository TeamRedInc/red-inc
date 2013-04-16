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
        public ActionResult Set(int id)
        {
            try
            {
                ProgressReportModel viewModel = new ProgressReportModel();

                viewModel.ProblemSet = GlobalStaticVars.StaticCore.GetSetById(id);
                viewModel.ProblemProgressList = GlobalStaticVars.StaticCore.GetProblemProgress(viewModel.ProblemSet.Class.Id, 0, id);

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        public ActionResult StudentSet(int studentId, int setId)
        {
            try
            {
                ProgressReportModel viewModel = new ProgressReportModel();

                viewModel.User = GlobalStaticVars.StaticCore.GetUserById(studentId);
                viewModel.ProblemSet = GlobalStaticVars.StaticCore.GetSetById(setId);
                viewModel.ProblemProgressList = GlobalStaticVars.StaticCore.GetProblemProgress(viewModel.ProblemSet.Class.Id, studentId, setId);

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        public ActionResult Student(int id)
        {
            try
            {
                ProgressReportModel viewModel = new ProgressReportModel();

                viewModel.User = GlobalStaticVars.StaticCore.GetUserById(id);
                viewModel.SetProgressList = GlobalStaticVars.StaticCore.GetSetProgress((int)Session["ClassId"], id);
                viewModel.ProblemProgressList = GlobalStaticVars.StaticCore.GetProblemProgress((int)Session["ClassId"], id);

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        
        public ActionResult Class(int id)
        {
            try
            {
                if ((int) Session["ClassId"] != id || (UserType) Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }

                ProgressReportModel viewModel = new ProgressReportModel();

                viewModel.Class = GlobalStaticVars.StaticCore.GetClassById(id);
                viewModel.StudentProgressList = GlobalStaticVars.StaticCore.GetStudentProgress(id);
                viewModel.SetProgressList = GlobalStaticVars.StaticCore.GetSetProgress(id);
                viewModel.ProblemProgressList = GlobalStaticVars.StaticCore.GetProblemProgress(id);

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}
