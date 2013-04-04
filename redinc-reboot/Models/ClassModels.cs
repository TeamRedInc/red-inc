using core.Modules.Class;
using core.Modules.ProblemSet;
using System.Collections.Generic;

namespace redinc_reboot.Models
{
    public class ClassViewModel
    {
        public ClassViewModel()
        {
            Sets = new List<ProblemSetData>();
        }
        public ClassData Class { get; set; }
        public ICollection<ProblemSetData> Sets { get; set; }
    }
}