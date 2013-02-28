﻿using core.Modules.Class;
using core.Modules.Problem;
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
        
        /// <summary>
        /// Add a new problem set to the database.</summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ProblemSetData set)
        {
            return setDao.Add(set);
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
        /// Gets all problem sets for the specified problem.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> GetForProblem(ProblemData problem)
        {
            return setDao.GetForProblem(problem);
        }

        /// <summary>
        /// Add a new prerequisite to the database.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereq">The ProblemSetData object with the prerequisite set's data</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool AddPrereq(ProblemSetData set, ProblemSetData prereq)
        {
            if (prereq.PrereqCount > 0)
                return setDao.AddPrereq(set, prereq);
            else
                return false;
        }

        /// <summary>
        /// Modify the number of required problems for a prerequisite.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereq">The ProblemSetData object with the prerequisite set's data</param>
        /// <returns>true if the operation was successful, false otherwise</returns>
        public bool UpdatePrereq(ProblemSetData set, ProblemSetData prereq)
        {
            if (prereq.PrereqCount > 0)
                return setDao.UpdatePrereq(set, prereq);
            else
                return false;
        }
        
        /// <summary>
        /// Removes the specified prerequisite.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereq">The ProblemSetData object with the prerequisite set's data</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemovePrereq(ProblemSetData set, ProblemSetData prereq)
        {
            return setDao.RemovePrereq(set, prereq);
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
