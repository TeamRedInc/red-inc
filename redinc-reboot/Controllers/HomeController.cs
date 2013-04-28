using CodeKicker.BBCode;
using core.Modules.Class;
using core.Modules.ProblemSet;
using core.Modules.User;
using redinc_reboot.Filters;
using redinc_reboot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace redinc_reboot.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Learning code made easy.";

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

        public string ParseBBCode(string data)
        {
            return BBCode.ToHtml(data);
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
            try
            {
                if (ModelState.IsValid)
                {
                    // Attempt to execute code
                    model.OutputCode = GlobalStaticVars.StaticCore.ExecutePythonCode(model.InputCode);
                }

                // Redisplay form with output
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        [Authorize]
        public ActionResult Home()
        {
            try
            {
                Session.Clear();

                var viewModel = new HomeClassListModel();
                //All classes the current user is a student in
                viewModel.StudentClassList = GlobalStaticVars.StaticCore.GetStudentClasses(WebSecurity.CurrentUserId);

                //All classes the current user is an instructor of
                viewModel.InstructorClassList = GlobalStaticVars.StaticCore.GetInstructorClasses(WebSecurity.CurrentUserId);

                //All the other classes that the current user can join
                var allClasses = GlobalStaticVars.StaticCore.GetAllClasses();
                viewModel.AllOtherClassesList = allClasses.Except(viewModel.StudentClassList).Except(viewModel.InstructorClassList);

                if (TempData["Error"] != null)
                    ViewBag.Error = TempData["Error"];

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}
