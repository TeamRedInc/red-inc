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
        public Dictionary<ProblemData, List<ProblemSetData>> Problems;

        public PortableClassData()
        {
            Problems = new Dictionary<ProblemData, List<ProblemSetData>>();
            ProblemSets = new Dictionary<ProblemSetData, List<ProblemSetData>>();
        }
    }
}
