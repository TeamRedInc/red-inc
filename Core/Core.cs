using core.Modules;
using core.Modules.User;
using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class Core
    {
        private readonly UserModel userModel;
        private readonly ClassModel classModel;

        /// <summary>
        /// Primary thread of execution
        /// All logic starts here!
        /// </summary>
        public Core()
        {
            userModel = ModelFactory.UserModel;
            classModel = ModelFactory.ClassModel;
        }

        public bool AddUser(string email, string passwordHash, string firstName, string lastName, bool isAdmin)
        {
            UserData user = new UserData(0);
            user.Email = email;
            user.PasswordHash = passwordHash;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.IsAdmin = isAdmin;

            return userModel.Add(user);
        }

        public UserData Login(string email, string passwordHash)
        {
            UserData user = new UserData(0);
            user.Email = email;
            user.PasswordHash = passwordHash;

            return userModel.Login(user);
        }

        public List<ClassData> GetAllClasses()
        {
            List<ClassData> classList = new List<ClassData>();
            return classModel.GetAllClasses();
        }
    }
}
