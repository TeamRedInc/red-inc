using core.Modules.ProblemSet;
using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Problem
{
    public class ProblemModel : BaseModel<ProblemData>
    {
        public ProblemModel() : base(DaoFactory.ProblemDao) { }

        private ProblemDao problemDao
        {
            get { return (ProblemDao)dao; }
        }

        /// <summary>
        /// Add a new problem to the database.</summary>
        /// <param name="problem">The ProblemData object with the problem's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ProblemData problem)
        {
            return problemDao.Add(problem);
        }

        /// <summary>
        /// Add the specified problem to the specified problem set.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool AddToSet(ProblemData problem, ProblemSetData set)
        {
            //TODO: untagged set
            return problemDao.AddToSet(problem, set);
        }

        /// <summary>
        /// Removes the specified problem from the specified problem set.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveFromSet(ProblemData problem, ProblemSetData set)
        {
            //TODO: untagged set
            return problemDao.RemoveFromSet(problem, set);
        }

        /// <summary>
        /// Gets all problems for the specified problem set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemData objects</returns>
        public List<ProblemData> GetForSet(ProblemSetData set)
        {
            return problemDao.GetForSet(set);
        }

        /// <summary>
        /// Record a user's first attempt at a problem.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="correct">Whether or not the solution is correct</param>
        /// <returns>
        /// true if the operation was successful, false otherwise</returns>
        public bool AddSolution(UserData user, ProblemData problem, bool correct)
        {
            return problemDao.AddSolution(user, problem, correct);
        }

        /// <summary>
        /// Record a user's attempt at a problem that they previously attempted.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="correct">Whether or not the solution is correct</param>
        /// <returns>
        /// true if the operation was successful, false otherwise</returns>
        public bool UpdateSolution(UserData user, ProblemData problem, bool correct)
        {
            return problemDao.UpdateSolution(user, problem, correct);
        }
    }
}
