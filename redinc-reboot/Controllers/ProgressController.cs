using core.Modules.Class;
using core.Modules.User;
using redinc_reboot.Models;
using System;
using System.Text;
using System.Web.Mvc;

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
                viewModel.StudentProgressList = GlobalStaticVars.StaticCore.GetStudentProgress(viewModel.ProblemSet.Class.Id, id);
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

        public ActionResult Export(int id)
        {
            try
            {
                if ((int)Session["ClassId"] != id || (UserType)Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }

                ClassData cls = GlobalStaticVars.StaticCore.GetClassById(id);
                string fileData = GlobalStaticVars.StaticCore.GetGradeCsv(id);
                string fileName = cls.Name + "_Progress_" + DateTime.Now.ToString(@"yyyy-MM-dd") + ".csv";

                return File(new UTF8Encoding().GetBytes(fileData), "text/csv", fileName);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}
