using CodeKicker.BBCode;
using redinc_reboot.Models;
using System.Web.Mvc;
using core.Modules.Class;
using WebMatrix.WebData;

namespace redinc_reboot.Controllers
{
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
            ClassData[] classesArr = allClasses.ToArray();
            for (int i = 0; i < allClasses.Count; i++)
            {
                if (!viewModel.StudentClassList.Contains((ClassData) classesArr.GetValue(i)) ||
                    !viewModel.InstructorClassList.Contains((ClassData) classesArr.GetValue(i)))
                {
                    viewModel.AllOtherClassesList.Add((ClassData) classesArr.GetValue(i));
                }
            }
            return View(viewModel);
        }
    }
}
