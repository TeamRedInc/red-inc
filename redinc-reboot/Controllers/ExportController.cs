using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using core.Modules.Class;
using core.Modules.User;

namespace redinc_reboot.Controllers
{
    public class ExportController : Controller
    {
        //
        // GET: /Export/

        public ActionResult ExportClassById(int id)
        {
            try
            {
                if ((int)Session["ClassId"] != id || (UserType)Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }

                ClassData cls = GlobalStaticVars.StaticCore.GetClassById(id);
                string fileData = GlobalStaticVars.StaticCore.ExportClassData(id);
                string fileName = cls.Name + "_DataExport_" + DateTime.Now.ToString(@"yyyy-MM-dd") + ".txt";

                return File(new UTF8Encoding().GetBytes(fileData), "text/xml", fileName);
            }
            catch
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

    }
}
