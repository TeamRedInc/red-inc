using redinc_reboot.Models;
using System.Web.Mvc;

namespace redinc_reboot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

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
    }
}
