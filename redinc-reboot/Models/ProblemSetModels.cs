using core.Modules.Problem;
using core.Modules.ProblemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace redinc_reboot.Models
{
    public class ProblemSetViewModel
    {
        public ProblemSetData Set { get; set; }
        public IList<ProblemSetData> Prereqs { get; set; }
        public IList<ProblemData> Problems { get; set; }
    }
}