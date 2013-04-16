using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Logging
{
    public class LogData
    {
        private DateTime dateTime;
        private UserData user;
        private string message;

        public LogData() { }

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public UserData User
        {
            get { return user; }
            set { user = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
