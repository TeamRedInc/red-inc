using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Class
{
    public class ClassData : DataObject
    {
        private string name;
        private UserData instructor;

        public ClassData() : base(0) { }

        public ClassData(int id) : base(id) { }

        public string Name
        {
            get
            {
                if (String.IsNullOrWhiteSpace(name))
                    return "Class " + Id;
                return name;
            }
            set { name = value; }
        }

        public UserData Instructor
        {
            get { return instructor; }
            set { instructor = value; }
        }

        public override string ToString()
        {
            return String.Format("Class: Id={0}, Name={1}", Id, Name);
        }
    }
}
