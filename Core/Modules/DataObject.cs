using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules
{
    [DataContract(IsReference=true)]
    public class DataObject : IEquatable<DataObject>
    {
        private int id;

        public DataObject(int id)
        {
            this.id = id;
        }
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public static bool operator ==(DataObject lhs, DataObject rhs)
        {
            if (Object.ReferenceEquals(lhs, null) && Object.ReferenceEquals(rhs, null))
                return true;
            else if (!Object.ReferenceEquals(lhs, null) && !Object.ReferenceEquals(rhs, null))
                return lhs.Id == rhs.Id;
            else
                return false;
        }

        public static bool operator !=(DataObject lhs, DataObject rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj is DataObject)
                return this.Equals((DataObject)obj);
            else
                return false;
        }

        public bool Equals(DataObject obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
