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
        public IEnumerable<ProblemProgress> ProblemProgressList { get; set; }
        public IEnumerable<SetProgress> SetProgressList { get; set; }
        public IEnumerable<StudentProgress> StudentProgressList { get; set; }
        
        public ClassData Class { get; set; }
        public UserData User { get; set; }
        public ProblemSetData ProblemSet { get; set; }
    }
}
