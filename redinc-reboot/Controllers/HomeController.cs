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
            if (ModelState.IsValid)
            {
                // Attempt to execute code
                model.OutputCode = GlobalStaticVars.StaticCore.ExecutePythonCode(model.InputCode);
            }
            
            // Redisplay form with output
            return View(model);
        }

        public ActionResult Home()
        {
            var viewModel = new HomeClassListModel();
            viewModel.StudentClassList = GlobalStaticVars.StaticCore.GetStudentClasses(WebSecurity.CurrentUserId);
            viewModel.InstructorClassList = GlobalStaticVars.StaticCore.GetInstructorClasses(WebSecurity.CurrentUserId);
            var allClasses = GlobalStaticVars.StaticCore.GetAllClasses();
            viewModel.AllOtherClassesList = allClasses.Except(viewModel.StudentClassList).Except(viewModel.InstructorClassList);
            
            return View(viewModel);
        }
    }
}
