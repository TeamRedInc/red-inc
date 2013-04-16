﻿using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.User;
using core.Modules.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.ProblemSet
{
    public class ProblemSetModel : BaseModel<ProblemSetData>
    {
        public ProblemSetModel() : base(DaoFactory.ProblemSetDao) { }

        private ProblemSetDao setDao
        {
            get { return (ProblemSetDao)dao; }
        }

        private UserDao userDao
        {
            get { return DaoFactory.UserDao; }
        }
        
        /// <summary>
        /// Add a new problem set to the database.</summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>
        /// the new set's id if the add was successful, 0 otherwise</returns>
        public int Add(ProblemSetData set)
        {
            return setDao.Add(set);
        }
        
        /// <summary>
        /// Modify a problem set's data.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>true if the modify was successful, false otherwise</returns>
        public bool Modify(ProblemSetData set)
        {
            return setDao.Modify(set);
        }

        /// <summary>
        /// Deletes the specified problem set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>true if the delete was successful, false otherwise</returns>
        public bool Delete(ProblemSetData set)
        {
            //Must manually delete prerequistes because SQL Server will not allow multiple cascading delete paths
            setDao.RemovePrereqs(set, null);
            return setDao.Delete(set);
        }

        /// <summary>
        /// Deletes all problem sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if the delete was successful, false otherwise</returns>
        public bool DeleteAllForClass(ClassData cls)
        {
            //Must manually delete prerequistes because SQL Server will not allow multiple cascading delete paths
            setDao.RemovePrereqsForClass(cls);
            return setDao.DeleteAllForClass(cls);
        }

        /// <summary>
        /// Gets all problem sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> GetForClass(ClassData cls)
        {
            return setDao.GetForClass(cls);
        }

        /// <summary>
        /// Gets all problem sets in the specified class that match the search query.
        /// If a null or empty search query is given, returns all sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <param name="search">The search query string</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> SearchInClass(ClassData cls, string search)
        {
            if (String.IsNullOrWhiteSpace(search))
                return GetForClass(cls);
            else
                return setDao.SearchInClass(cls, search);
        }

        /// <summary>
        /// Gets all problem sets for the specified problem.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> GetForProblem(ProblemData problem)
        {
            return setDao.GetForProblem(problem);
        }

        /// <summary>
        /// Gets all problem sets in the specified class. Each set is also determined to be
        /// locked or unlocked based on the specified user's progress in the class.
        /// Gets all problem sets in the specified class that have problems in them.
        /// The sets are separated into 3 categories:
        /// <list type="bullet">
        /// <item><description>
        /// Unlocked - All of the set's prerequisites are satisfied and the set has problems not solved by the user.
        /// </description></item>
        /// <item><description>
        /// Locked - Not all of the set's prerequistes are satisfied.
        /// </description></item>
        /// <item><description>
        /// Solved - All of the set's prerequisites are satisfied and all problems in the set have been solved by the user.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <param name="user">The UserData object with the user's id</param>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>
        /// A tuple containing 3 non-null, possibly empty lists of filled ProblemSetData objects.
        /// The lists represent the unlocked, locked, and solved sets, respectively.
        /// </returns>
        public Tuple<List<ProblemSetData>, List<ProblemSetData>, List<ProblemSetData>> GetForStudent(UserData user, ClassData cls)
        {
            if (userDao.IsStudent(user, cls))
                return setDao.GetForStudent(user, cls);
            else
                return Tuple.Create(new List<ProblemSetData>(), new List<ProblemSetData>(), new List<ProblemSetData>());
        }

        public bool UpdatePrereqs(ProblemSetData set, ICollection<ProblemSetData> prereqs)
        {
            ICollection<ProblemSetData> oldPrereqs = GetPrereqs(set);

            //Remove self prerequisite
            prereqs.Remove(set);

            //Remove duplicates
            IEnumerable<ProblemSetData> newPrereqs = prereqs.Distinct();

            IEnumerable<ProblemSetData> toAdd = newPrereqs.Except(oldPrereqs);
            IEnumerable<ProblemSetData> toRemove = oldPrereqs.Except(newPrereqs);
            IEnumerable<ProblemSetData> toUpdate = newPrereqs.Intersect(oldPrereqs, 
                new DelegateEqualityComparer<ProblemSetData>((n, o) => n == o && n.PrereqCount != o.PrereqCount));

            bool add = setDao.AddPrereqs(set, toAdd);
            bool remove = setDao.RemovePrereqs(set, toRemove);
            bool update = setDao.UpdatePrereqs(set, toUpdate);

            return add && remove && update;
        }

        /// <summary>
        /// Gets all of the prerequisite sets for the specified set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the problem set's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects with PrereqCount properties set</returns>
        public List<ProblemSetData> GetPrereqs(ProblemSetData set)
        {
            return setDao.GetPrereqs(set);
        }
    }
}
