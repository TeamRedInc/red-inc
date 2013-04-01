using CodeKicker.BBCode;
using redinc_reboot.Models;
using System.Web.Mvc;
using core.Modules.Class;
using WebMatrix.WebData;
using redinc_reboot.Filters;
using System.Linq;
using core.Modules.User;
using core.Modules.ProblemSet;
using System.Collections.Generic;

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

        public ActionResult JoinClass(int id)
        {
            GlobalStaticVars.StaticCore.AddStudent(WebSecurity.CurrentUserId, id);
            return RedirectToAction("Home");
        }

        public ActionResult ClassHome(int id)
        {
            ClassData cls = GlobalStaticVars.StaticCore.GetClassById(id);

            if (WebSecurity.CurrentUserId == cls.Instructor.Id)
            {
                Session["ClassId"] = id;
                Session["UserType"] = UserType.Instructor;
                IEnumerable<ProblemSetData> sets = GlobalStaticVars.StaticCore.GetSetsForClass(id);
                return View("InstructorClassHome", sets);
            }
            else if (GlobalStaticVars.StaticCore.IsStudent(WebSecurity.CurrentUserId, id))
            {
                Session["ClassId"] = id;
                Session["UserType"] = UserType.Student;
                IEnumerable<ProblemSetData> sets = GlobalStaticVars.StaticCore.GetSetsForStudent(WebSecurity.CurrentUserId, id);
                return View("StudentClassHome", sets);
            }

            Session.Clear();
            return RedirectToAction("Home");
        }
    }
}
