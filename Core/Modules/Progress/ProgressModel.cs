using core.Modules.Class;
using core.Modules.ProblemSet;
using core.Modules.User;
using System.Collections.Generic;

namespace core.Modules.Progress
{
    public class ProgressModel
    {
        public ProgressModel() { }

        private ProgressDao progressDao
        {
            get { return DaoFactory.ProgressDao; }
        }

        /// <summary>
        /// Calculates aggregated progress information for all students in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>A non-null, possibly empty list of StudentProgress objects</returns>
        public List<StudentProgress> GetStudentProgress(ClassData cls)
        {
            return progressDao.GetStudentProgress(cls);
        }

        /// <summary>
        /// Calculates aggregated progress data for all sets in the specified class.
        /// If a non-null user is passed in, then the data is limited to that student's progress.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <param name="user">The UserData object with the user's id (optional)</param>
        /// <returns></returns>
        public List<SetProgress> GetSetProgress(ClassData cls, UserData user = null)
        {
            return progressDao.GetSetProgress(cls, user);
        }

        /// <summary>
        /// Calculates aggregated progress data for all sets in the specified class.
        /// If a non-null user is passed in, then the data is limited to that student's progress.
        /// If a non-null set is passed in, then the data is limited to problems in that set.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <param name="user">The UserData object with the user's id (optional)</param>
        /// <param name="set">The ProblemSetData object with the set's id (optional)</param>
        /// <returns>A non-null, possibly empty list of ProblemProgress objects</returns>
        public List<ProblemProgress> GetProblemProgress(ClassData cls, UserData user = null, ProblemSetData set = null)
        {
            return progressDao.GetProblemProgress(cls, user, set);
        }
    }
}
