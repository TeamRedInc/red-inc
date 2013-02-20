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

        public string Name
        {
            get 
            {
                if (String.IsNullOrEmpty(name))
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

        public override string ToString()
        {
            return String.Format("Problem Set: Id={0}, Name={1}", Id, Name);
        }
    }
}
