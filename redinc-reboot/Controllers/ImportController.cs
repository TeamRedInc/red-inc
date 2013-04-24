using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace redinc_reboot.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult ImportClassById(HttpPostedFileBase file, int classId)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                var stream = file.InputStream;
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                GlobalStaticVars.StaticCore.ImportClassData(classId, content);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Home", "Class", new { id = classId });
        }
    }
}
