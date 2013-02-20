using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules
{
    public class DataObject
    {
        private int id;

        public DataObject(int id)
        {
            this.id = id;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public static bool operator ==(DataObject lhs, DataObject rhs)
        {
            return lhs.Id == rhs.Id;
        }

        public static bool operator !=(DataObject lhs, DataObject rhs)
        {
            return !(lhs == rhs);
        }
    }
}
