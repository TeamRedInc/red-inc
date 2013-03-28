using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.User
{
    public class UserData : DataObject
    {
        private string email;
        private string firstName;
        private string lastName;
        private bool isAdmin;

        public UserData() : base(0) { }

        public UserData(int id) : base(id) {}

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

        public override string ToString()
        {
            return String.Format("User: Id={0}, Email={1}", Id, Email);
        }
    }
}
