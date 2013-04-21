using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core.Modules.Problem;
using core.Modules.ProblemSet;

namespace core.Modules.DataPort
{
    public class PortableClassData
    {
        public Dictionary<ProblemSetData,List<ProblemSetData>> ProblemSets;
        public List<ProblemData> Problems;

        public PortableClassData()
        {
            Problems = new List<ProblemData>();
            ProblemSets = new Dictionary<ProblemSetData, List<ProblemSetData>>();
        }
    }
}
