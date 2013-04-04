using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using core.Modules.Class;

namespace redinc_reboot.Models
{
    public class HomeClassListModel
    {
        public IEnumerable<ClassData> StudentClassList { get; set; }
        public IEnumerable<ClassData> InstructorClassList { get; set; }
        public IEnumerable<ClassData> AllOtherClassesList { get; set; }
    }
}
