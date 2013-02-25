using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        /// Add a new problem set to the database.</summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ProblemSetData set)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string cmdStr = "Insert into dbo.[" + tableName + "] (";
                string paramList = ") values (";

                SqlCommand cmd = conn.CreateCommand();

                //Name
                if (!String.IsNullOrWhiteSpace(set.Name))
                {
                    cmdStr += "Name";
                    paramList += "@name";
                    cmd.Parameters.AddWithValue("@name", set.Name);
                }

                //Class
                cmdStr += ", ClassId";
                paramList += ", @classId";
                cmd.Parameters.AddWithValue("@classId", set.Class.Id);

                cmd.CommandText = cmdStr + paramList + ");";

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets all problem sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> GetForClass(ClassData cls)
        {
            return base.GetAll("ClassId", cls.Id);
        }

        /// <summary>
        /// Gets all problem sets for the specified problem.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> GetForProblem(Problem.ProblemData problem)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemSetData> sets = new List<ProblemSetData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[ProblemSetProblem] psp"
                    + " Join dbo.[" + tableName + "] ps on ps.Id = psp.ProblemSetId"
                    + " Where ProblemId = @problemId;";

                //Problem
                cmd.Parameters.AddWithValue("@problemId", problem.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            sets.Add(createFromReader(reader));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return sets;
            }
        }

        /// <summary>
        /// Add a new prerequisite to the database.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereq">The ProblemSetData object with the prerequisite set's data</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool AddPrereq(ProblemSetData set, ProblemSetData prereq)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Prereq] values (@setId, @prereqId, @numProbs);";

                //Parent Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

                //Prereq Set
                cmd.Parameters.AddWithValue("@prereqId", prereq.Id);

                //Number of problems
                cmd.Parameters.AddWithValue("@numProbs", prereq.PrereqCount);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets all of the prerequisite sets for the specified set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the problem set's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects with PrereqCount properties set</returns>
        public List<ProblemSetData> GetPrereqs(ProblemSetData set)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemSetData> prereqs = new List<ProblemSetData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[Prereq] p"
                    + " Join dbo.[" + tableName + "] ps on ps.Id = p.RequiredSetId"
                    + " Where ProblemSetId = @setId;";

                //Problem Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            ProblemSetData p = createFromReader(reader);
                            p.PrereqCount = (int)reader["NumProblems"];
                            prereqs.Add(p);
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return prereqs;
            }
        }

        /// <summary>
        /// This method only exists so the superclass DataAccessObject can polymorphically call createFromReader.
        /// Use of the static createFromReader(SqlDataReader) method is preferred.
        /// </summary>
        public override ProblemSetData createObjectFromReader(SqlDataReader reader)
        {
            return ProblemSetDao.createFromReader(reader);
        }

        /// <summary>
        /// Creates a problem set from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get problem set data from</param>
        /// <returns>
        /// A ProblemSetData object</returns>
        public static ProblemSetData createFromReader(SqlDataReader reader)
        {
            ProblemSetData problem = new ProblemSetData((int)reader["Id"]);
            problem.Name = reader["Name"] as string;
            problem.Class = new ClassData((int)reader["ClassId"]);
            return problem;
        }
    }
}
