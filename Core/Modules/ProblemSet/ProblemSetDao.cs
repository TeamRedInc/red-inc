using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.ProblemSet
{
    public class ProblemSetDao : DataAccessObject<ProblemSetData>
    {
        public ProblemSetDao() : base("ProblemSet") { }

        /// <summary>
        /// Creates a problem set from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get problem set data from</param>
        /// <returns>
        /// A ProblemSetData object</returns>
        public override ProblemSetData createFromReader(SqlDataReader reader)
        {
            ProblemSetData problem = new ProblemSetData((int)reader["Id"]);
            problem.Name = reader["Name"] as string;
            problem.Class = new ClassData((int)reader["ClassId"]);
            return problem;
        }
    }
}
