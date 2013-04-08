﻿using core.Modules.Class;
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

        private ClassModel classModel
        {
            get { return ModelFactory.ClassModel; }
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
            //Fill the data objects
            student = this.GetById(student.Id);
            cls = classModel.GetById(cls.Id);

            //Verify that the user requesting to join the class has an email with the required domain
            if (!String.IsNullOrWhiteSpace(cls.RequiredDomain) && student.Email.EndsWith(cls.RequiredDomain))
                return userDao.AddStudent(student, cls);
            else
                return false;
        }
        
        /// <summary>
        /// Checks if the specified user is a student in the specified class.
        /// </summary>
        /// <param name="user">The UserData object with the user's id</param>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if user is a student in cls, false otherwise</returns>
        public bool IsStudent(UserData user, ClassData cls)
        {
            return userDao.IsStudent(user, cls);
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
