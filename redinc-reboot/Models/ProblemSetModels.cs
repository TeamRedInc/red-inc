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
        public ProblemSetViewModel()
        {
            Prereqs = new List<ProblemSetData>();
            Problems = new List<ProblemData>();
        }
        public ProblemSetData Set { get; set; }
        public ICollection<ProblemSetData> Prereqs { get; set; }
        public ICollection<ProblemData> Problems { get; set; }
    }
}