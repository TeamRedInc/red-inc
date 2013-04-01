using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using core.Modules.Class;

namespace redinc_reboot.Models
{
    public class HomeClassListModel : Controller
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Instructor { get; set; }

        public List<ClassData> StudentClassList { get; set; }
        public List<ClassData> InstructorClassList { get; set; }
        public List<ClassData> AllOtherClassesList { get; set; }
    }
}
