using core.Modules.Problem;
using core.Modules.ProblemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace redinc_reboot.Models
{
    public class ProblemViewModel
    {
        public ProblemViewModel()
        {
            Sets = new List<ProblemSetData>();
        }
        public ProblemData Problem { get; set; }
        public ICollection<ProblemSetData> Sets { get; set; }
    }
}