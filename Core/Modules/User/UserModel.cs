using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.User
{
    public class UserModel : BaseModel<UserData>
    {
        public UserModel() : base(DaoFactory.UserDao) { }

        private UserDao userDao
        {
            get { return (UserDao)dao; }
        }

        /// <summary>
        /// Add a new user to the database.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(UserData user)
        {
            return userDao.Add(user);
        }

        /// <summary>
        /// Adds a user to a class as a student.</summary>
        /// <param name="student">The UserData object with the student user's id</param>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool AddStudent(UserData student, ClassData cls)
        {
            return userDao.AddStudent(student, cls);
        }

        /// <summary>
        /// Gets all students in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>A non-null, possibly empty list of filled UserData objects</returns>
        public List<UserData> GetStudents(ClassData cls)
        {
            return userDao.GetStudents(cls);
        }
    }
}
