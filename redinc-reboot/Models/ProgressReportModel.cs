using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using core.Modules.Progress;

namespace redinc_reboot.Models
{
    public class ProgressReportModel : Controller
    {
        public IEnumerable<ProblemProgress> ProblemSetProgressList { get; set; }
        public IEnumerable<SetProgress> StudentProgressList { get; set; }
        public IEnumerable<StudentProgress> ClassProgressList { get; set; }

    }
}
