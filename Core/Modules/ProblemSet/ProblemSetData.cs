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

        /// <summary>
        /// Whether or not this problem set is available to do problems from.
        /// This only applies when looking up problem sets for a particular user in a particular class.
        /// </summary>
        public Boolean Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public override string ToString()
        {
            return String.Format("Problem Set: Id={0}, Name={1}", Id, Name);
        }
    }
}
