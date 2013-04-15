using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using core.Modules.Progress;
using core.Modules.Class;
using core.Modules.User;
using core.Modules.ProblemSet;

namespace redinc_reboot.Models
{
    public class ProgressReportModel
    {
        public IEnumerable<ProblemProgress> ProblemSetProgressList { get; set; }
        public IEnumerable<SetProgress> StudentProgressList { get; set; }
        public IEnumerable<StudentProgress> ClassProgressList { get; set; }
        
        public ClassData Class { get; set; }
        public UserData User { get; set; }
        public ProblemSetData ProblemSet { get; set; }
    }
}
