using core.Modules.Class;
using core.Modules.User;
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
    public class ProblemSetDao : BaseDao<ProblemSetData>
    {
        public ProblemSetDao() : base("ProblemSet") { }

        /// <summary>
        /// Add a new problem set to the database.</summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>
        /// the new set's id if the add was successful, 0 otherwise</returns>
        public int Add(ProblemSetData set)
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
        /// Modify a problem set's data.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's information</param>
        /// <returns>true if the modify was successful, false otherwise</returns>
        public bool Modify(ProblemSetData set)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Update dbo.[" + tableName + "]"
                    + " Set Name = @name"
                    + " Where Id = @id;";

                //Name
                cmd.Parameters.AddWithValue("@name", set.Name);

                //Id
                cmd.Parameters.AddWithValue("@id", set.Id);

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
        /// Gets all problem sets in the specified class that match the search query.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <param name="search">The search query string</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects</returns>
        public List<ProblemSetData> SearchInClass(ClassData cls, string search)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemSetData> sets = new List<ProblemSetData>();
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select ps.*, Case ");
                query.AppendLine("  When ps.Name Like @search Then 1 ");
                query.AppendLine("  When ps.Name Like Concat(@search, '%') Then 2 ");
                query.AppendLine("  When ps.Name Like Concat('%', @search, '%') Then 3 ");
                query.AppendLine("End as 'Priority' ");
                query.AppendLine("from dbo.[" + tableName + "] ps ");
                query.AppendLine("Where ps.ClassId = @clsId and ps.Name Like Concat('%', @search, '%') ");
                query.AppendLine("Order by Priority ");

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                //Search query
                cmd.Parameters.AddWithValue("@search", search);

                cmd.CommandText = query.ToString();

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            sets.Add(createObjectFromReader(reader));
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
        /// Gets all problem sets in the specified class. Each set is also determined to be
        /// locked or unlocked based on the specified user's progress in the class.
        /// </summary>
        /// <param name="user">The UserData object with the user's id</param>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>A non-null, possibly empty list of filled ProblemSetData objects with Locked properties set</returns>
        public List<ProblemSetData> GetForStudent(UserData user, ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemSetData> sets = new List<ProblemSetData>();
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("With LockedSets(Id, Name, ClassId) as (");
                query.AppendLine("  Select Distinct ps.* from dbo.[ProblemSet] ps");
                query.AppendLine("  Join dbo.[Prereq] prereq on prereq.ProblemSetId = ps.Id");
                query.AppendLine("  Where ps.ClassId = @classId");
                query.AppendLine("  and prereq.NumProblems > (");
                query.AppendLine("      Select count(*) from dbo.[Solution] s");
                query.AppendLine("      Join dbo.[Problem] p on p.Id = s.ProblemId");
                query.AppendLine("      Join dbo.[ProblemSetProblem] psp on psp.ProblemId = p.Id");
                query.AppendLine("      Where psp.ProblemSetId = prereq.RequiredSetId");
                query.AppendLine("      and s.UserId = @userId and s.IsCorrect = 1");
                query.AppendLine("  )");
                query.AppendLine("), UnlockedSets(Id, Name, ClassId) as (");
                query.AppendLine("  ( Select * from ProblemSet ps Where ps.ClassId = @classId )");
                query.AppendLine("  Except");
                query.AppendLine("  ( Select * from LockedSets )");
                query.AppendLine(") ");
                query.AppendLine("Select ls.*, Cast(1 as Bit) as 'Locked' from LockedSets ls ");
                query.AppendLine("Union ");
                query.AppendLine("Select us.*, Cast(0 as Bit) as 'Locked' from UnlockedSets us ");
                query.AppendLine("Where Id in ( Select ProblemSetId from dbo.[ProblemSetProblem] ) ");
                query.AppendLine("Order by Locked, Id;");

                cmd.CommandText = query.ToString();

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Class
                cmd.Parameters.AddWithValue("@classId", cls.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            ProblemSetData s = createFromReader(reader);
                            s.Locked = (bool)reader["Locked"];
                            sets.Add(s);
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

                return sets;
            }
        }

        /// <summary>
        /// Add new prerequisites to the database.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereqs">A collection of ProblemSetData objects with the prerequisite sets' data</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool AddPrereqs(ProblemSetData set, IEnumerable<ProblemSetData> prereqs)
        {
            if (prereqs == null || !prereqs.Any())
                return true; //Nothing to add

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Insert into dbo.[Prereq] values ");
                
                int i = 0;
                foreach (ProblemSetData prereq in prereqs)
                {
                    if (i != 0)
                        query.AppendLine(",");

                    query.Append("(@setId, @prereqId" + i + ", @numProbs" + i + ")");

                    //Prereq Set
                    cmd.Parameters.AddWithValue("@prereqId" + i, prereq.Id);

                    //Number of problems
                    cmd.Parameters.AddWithValue("@numProbs" + i, prereq.PrereqCount);

                    ++i;
                }

                //Parent Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

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
        /// Modify the number of required problems for the specified prerequisites.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereqs">A collection of ProblemSetData objects with the prerequisite sets' data</param>
        /// <returns>true if the update was successful, false otherwise</returns>
        public bool UpdatePrereqs(ProblemSetData set, IEnumerable<ProblemSetData> prereqs)
        {
            if (prereqs == null || !prereqs.Any())
                return true; //Nothing to update

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Update dbo.[Prereq] Set NumProblems = Case RequiredSetId ");

                StringBuilder whereClause = new StringBuilder();
                whereClause.Append("RequiredSetId in (");

                int i = 0;
                foreach (ProblemSetData prereq in prereqs)
                {
                    if (i != 0)
                        whereClause.Append(",");

                    query.AppendLine("When @prereqId" + i + " Then @numProbs" + i + " ");
                    whereClause.Append("@prereqId" + i);

                    //Prereq Set
                    cmd.Parameters.AddWithValue("@prereqId" + i, prereq.Id);

                    //Number of problems
                    cmd.Parameters.AddWithValue("@numProbs" + i, prereq.PrereqCount);

                    ++i;
                }
                whereClause.Append(")");

                query.AppendLine("End ");
                query.AppendLine("Where ProblemSetId = @setId and ");
                query.Append(whereClause);

                //Parent Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

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
        /// Removes the specified prerequisites.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereqs">A collection of ProblemSetData objects with the prerequisite sets' ids</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemovePrereqs(ProblemSetData set, IEnumerable<ProblemSetData> prereqs)
        {
            if (prereqs == null || !prereqs.Any())
                return true; //Nothing to remove

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.Append("Delete from dbo.[Prereq] Where ProblemSetId = @setId and RequiredSetId in (");

                int i = 0;
                foreach (ProblemSetData prereq in prereqs)
                {
                    if (i != 0)
                        query.Append(",");

                    query.Append("@prereqId" + i);

                    //Prereq Set
                    cmd.Parameters.AddWithValue("@prereqId" + i, prereq.Id);

                    ++i;
                }
                query.Append(")");

                //Parent Set
                cmd.Parameters.AddWithValue("@setId", set.Id);

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
        /// This method only exists so the superclass BaseDao can polymorphically call createFromReader.
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
