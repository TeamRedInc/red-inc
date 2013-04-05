using core.Modules.ProblemSet;
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

        private ProblemSetModel setModel
        {
            get { return ModelFactory.ProblemSetModel; }
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
        /// Modify a class's data.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's information</param>
        /// <returns>true if the modify was successful, false otherwise</returns>
        public bool Modify(ClassData cls)
        {
            return classDao.Modify(cls);
        }

        /// <summary>
        /// Deletes the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if the delete was successful, false otherwise</returns>
        public bool Delete(ClassData cls)
        {
            //Must manually delete problem sets because SQL Server will not allow multiple cascading delete paths
            setModel.DeleteAllForClass(cls);
            return classDao.Delete(cls);
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
    }
}
