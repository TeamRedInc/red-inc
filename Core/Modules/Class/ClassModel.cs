using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Class
{
    public class ClassModel : BaseModel<ClassData>
    {
        public ClassModel() : base(DaoFactory.ClassDao) { }

        private ClassDao classDao
        {
            get { return (ClassDao)dao; }
        }

        /// <summary>
        /// Add a new class to the database.</summary>
        /// <param name="cls">The ClassData object with the class's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ClassData cls)
        {
            return classDao.Add(cls);
        }

        /// <summary>
        /// Gets all classes the specified user is a student in.
        /// </summary>
        /// <param name="user">The UserData object with the student's id</param>
        /// <returns>A non-null, possibly empty list of filled ClassData objects</returns>
        public List<ClassData> GetStudentClasses(UserData user)
        {
            return classDao.GetStudentClasses(user);
        }

        /// <summary>
        /// Gets all classes the specified user is an instructor for.
        /// </summary>
        /// <param name="user">The UserData object with the instructor's id</param>
        /// <returns>A non-null, possibly empty list of filled ClassData objects</returns>
        public List<ClassData> GetInstructorClasses(UserData user)
        {
            return classDao.GetInstructorClasses(user);
        }

        /// <summary>
        /// Gets all classes a user can register for or is registered for.
        /// </summary>
        /// <returns>A non-null, possibly empty list of filled ClassData objects</returns>
        public List<ClassData> GetAllClasses()
        {
            return classDao.GetAllClasses();
        }
    }
}
