using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.ProblemSet
{
    public class ProblemSetData : DataObject
    {
        private string name;
        private ClassData _class;
        private int prereqCount;
        private Boolean locked;

        public ProblemSetData() : base(0) { }

        public ProblemSetData(int id) : base(id) { }

        public string Name
        {
            get 
            {
                if (String.IsNullOrWhiteSpace(name))
                    return "Problem Set " + Id;
                return name; 
            }
            set { name = value; }
        }

        public ClassData Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// The number of problems that must be completed to satisfy the prerequisite on this set.
        /// This only applies when this ProblemSetData object is acting as a prerequisite for another set.
        /// </summary>
        public int PrereqCount
        {
            get { return prereqCount; }
            set { prereqCount = value; }
        }

        public override string ToString()
        {
            return String.Format("Problem Set: Id={0}, Name={1}", Id, Name);
        }
    }
}
