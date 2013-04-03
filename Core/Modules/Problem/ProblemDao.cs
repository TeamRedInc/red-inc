using core.Modules.ProblemSet;
using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace core.Modules.Problem
{
    public class ProblemDao : BaseDao<ProblemData>
    {
        public ProblemDao() : base("Problem") { }

        /// <summary>
        /// Add a new problem to the database.</summary>
        /// <param name="problem">The ProblemData object with the problem's information</param>
        /// <returns>
        /// the new problem's id if the add was successful, 0 otherwise</returns>
        public int Add(ProblemData problem)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string cmdStr = "Insert into dbo.[" + tableName + "] (";
                string paramList = ") values (";

                SqlCommand cmd = conn.CreateCommand();

                bool fieldInserted = false;
                //Name
                if (!String.IsNullOrWhiteSpace(problem.Name))
                {
                    fieldInserted = true;
                    cmdStr += "Name";
                    paramList += "@name";
                    cmd.Parameters.AddWithValue("@name", problem.Name);
                }

                //Description
                if (!String.IsNullOrWhiteSpace(problem.Description))
                {
                    if (fieldInserted)
                    {
                        cmdStr += ", ";
                        paramList += ", ";
                    }
                    fieldInserted = true;
                    cmdStr += "Description";
                    paramList += "@description";
                    cmd.Parameters.AddWithValue("@description", problem.Description);
                }
                
                //Solution Code
                if (!String.IsNullOrWhiteSpace(problem.SolutionCode))
                {
                    if (fieldInserted)
                    {
                        cmdStr += ", ";
                        paramList += ", ";
                    }
                    fieldInserted = true;
                    cmdStr += "SolutionCode";
                    paramList += "@slnCode";
                    cmd.Parameters.AddWithValue("@slnCode", problem.SolutionCode);
                }

                cmd.CommandText = cmdStr + paramList + "); Select SCOPE_IDENTITY()";

                try
                {
                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    return Decimal.ToInt32((decimal)obj);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Modify a problem's data.
        /// </summary>
        /// <param name="set">The ProblemData object with the problem's information</param>
        /// <returns>true if the modify was successful, false otherwise</returns>
        public bool Modify(ProblemData problem)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Update dbo.[" + tableName + "]"
                    + " Set Name = @name,"
                    + " Description = @description,"
                    + " SolutionCode = @slnCode"
                    + " Where Id = @id;";

                //Name
                cmd.Parameters.AddWithValue("@name", problem.Name);

                //Description
                cmd.Parameters.AddWithValue("@description", problem.Description);

                //Solution Code
                cmd.Parameters.AddWithValue("@slnCode", problem.SolutionCode);

                //Id
                cmd.Parameters.AddWithValue("@id", problem.Id);

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
        /// Add the specified problem to the specified problem sets.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="sets">A collection of ProblemSetData objects with the sets' ids</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool AddToSets(ProblemData problem, IEnumerable<ProblemSetData> sets)
        {
            if (sets == null || !sets.Any())
                return true; //Nothing to add to

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Insert into dbo.[ProblemSetProblem] values ");

                int i = 0;
                foreach (ProblemSetData set in sets)
                {
                    if (i != 0)
                        query.AppendLine(",");

                    query.Append("(@setId" + i + ", @problemId)");

                    //Problem Set
                    cmd.Parameters.AddWithValue("@setId" + i, set.Id);

                    ++i;
                }

                //Problem
                cmd.Parameters.AddWithValue("@problemId", problem.Id);

                cmd.CommandText = query.ToString();

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
        /// Removes the specified problem from the specified problem sets.
        /// </summary>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="sets">A collection of ProblemSetData objects with the sets' ids</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveFromSets(ProblemData problem, IEnumerable<ProblemSetData> sets)
        {
            if (sets == null || !sets.Any())
                return true; //Nothing to remove from

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.Append("Delete from dbo.[ProblemSetProblem] Where ProblemId = @problemId and ProblemSetId in (");

                int i = 0;
                foreach (ProblemSetData set in sets)
                {
                    if (i != 0)
                        query.Append(",");

                    query.Append("@setId" + i);

                    //Problem Set
                    cmd.Parameters.AddWithValue("@setId" + i, set.Id);

                    ++i;
                }
                query.Append(")");

                //Problem
                cmd.Parameters.AddWithValue("@problemId", problem.Id);

                cmd.CommandText = query.ToString();

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
        /// Gets all problems for the specified problem set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemData objects</returns>
        public List<ProblemData> GetForSet(ProblemSetData set)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemData> problems = new List<ProblemData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[ProblemSetProblem] psp"
                    + " Join dbo.[" + tableName + "] p on p.Id = psp.ProblemId"
                    + " Where psp.ProblemSetId = @setId;";

                //Problem Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            problems.Add(createFromReader(reader));
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

                return problems;
            }
        }

        /// <summary>
        /// Gets all problems in the specified problem set that 
        /// the specified user has either solved or not solved.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <param name="user">The UserData object with the user's id</param>
        /// <param name="solved">true to get solved problems, false to get unsolved problems</param>
        /// <returns>A non-null, possibly empty list of filled ProblemData objects</returns>
        public List<ProblemData> GetForSetAndUser(ProblemSetData set, UserData user, bool solved)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemData> problems = new List<ProblemData>();
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select * from dbo.[ProblemSetProblem] psp ");
                query.AppendLine("Join dbo.[" + tableName + "] p on p.Id = psp.ProblemId ");
                query.AppendLine("Where psp.ProblemSetId = @setId ");
                query.AppendLine("and ( Exists ( ");
                query.AppendLine("  Select * from dbo.[Solution] s ");
                query.AppendLine("  Where s.ProblemId = p.Id ");
                query.AppendLine("  and s.UserId = @userId ");
                query.AppendLine("  and s.IsCorrect = @correct ");
                if (solved)
                    query.AppendLine(") ) ");
                else
                {
                    query.AppendLine(") ");
                    query.AppendLine("or Not Exists ( ");
                    query.AppendLine("  Select * from dbo.[Solution] s ");
                    query.AppendLine("  Where s.ProblemId = p.Id ");
                    query.AppendLine("  and s.UserId = @userId ");
                    query.AppendLine(") )");
                }

                //Problem Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Correct
                cmd.Parameters.AddWithValue("@correct", solved);

                cmd.CommandText = query.ToString();

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            problems.Add(createFromReader(reader));
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

                return problems;
            }
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
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Solution] (UserId, ProblemId, IsCorrect) values (@userId, @problemId, @correct);";

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Problem
                cmd.Parameters.AddWithValue("@problemId", problem.Id);

                //Correct
                cmd.Parameters.AddWithValue("@correct", correct);

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
        /// Record a user's attempt at a problem that they previously attempted.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <param name="problem">The ProblemData object with the problem's id</param>
        /// <param name="correct">Whether or not the solution is correct</param>
        /// <returns>
        /// true if the operation was successful, false otherwise</returns>
        public bool UpdateSolution(UserData user, ProblemData problem, bool correct)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Update dbo.[Solution] Set NumAttempts = NumAttempts + 1, IsCorrect = @correct"
                    + " Where UserId = @userId and ProblemId = @problemId;";

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Problem
                cmd.Parameters.AddWithValue("@problemId", problem.Id);

                //Correct
                cmd.Parameters.AddWithValue("@correct", correct);

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
        /// This method only exists so the superclass BaseDao can polymorphically call createFromReader.
        /// Use of the static createFromReader(SqlDataReader) method is preferred.
        /// </summary>
        public override ProblemData createObjectFromReader(SqlDataReader reader)
        {
            return ProblemDao.createFromReader(reader);
        }

        /// <summary>
        /// Creates a problem from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get problem data from</param>
        /// <returns>
        /// A ProblemData object</returns>
        public static ProblemData createFromReader(SqlDataReader reader)
        {
            ProblemData problem = new ProblemData((int)reader["Id"]);
            problem.Name = reader["Name"] as string;
            problem.Description = reader["Description"] as string;
            problem.SolutionCode = reader["SolutionCode"] as string;
            return problem;
        }
    }
}
