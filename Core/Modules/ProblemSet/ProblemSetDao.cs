﻿using core.Modules.Class;
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
        /// Deletes the specified problem set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the set's id</param>
        /// <returns>true if the delete was successful, false otherwise</returns>
        public bool Delete(ProblemSetData set)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Delete from dbo.[" + tableName + "] Where Id = @id;";

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
        /// Deletes all problem sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if the delete was successful, false otherwise</returns>
        public bool DeleteAllForClass(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Delete from dbo.[" + tableName + "] Where ClassId = @clsId;";

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

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
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemSetData> unlocked = new List<ProblemSetData>();
                List<ProblemSetData> locked = new List<ProblemSetData>();
                List<ProblemSetData> solved = new List<ProblemSetData>();
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select distinct ps.*, Case ");
                query.AppendLine("  When 0 = ( ");
                query.AppendLine("      Select count(*) from dbo.[Problem] p ");
                query.AppendLine("      Join dbo.[ProblemSetProblem] psp on psp.ProblemId = p.Id ");
                query.AppendLine("      Left Join dbo.[Solution] s on s.ProblemId = p.Id and s.UserId = @userId ");
                query.AppendLine("      Where psp.ProblemSetId = ps.Id ");
                query.AppendLine("      and (s.IsCorrect = 0 or s.IsCorrect is null) ");
                query.AppendLine("  ) Then 'Solved' ");
                query.AppendLine("  When 0 != ( ");
                query.AppendLine("      Select count(*) from dbo.[Prereq] prereq ");
                query.AppendLine("      Where prereq.ProblemSetId = ps.Id ");
                query.AppendLine("      and prereq.NumProblems > ( ");
                query.AppendLine("          Select count(*) from dbo.[Solution] s ");
                query.AppendLine("          Join dbo.[Problem] p on p.Id = s.ProblemId ");
                query.AppendLine("          Join dbo.[ProblemSetProblem] psp on psp.ProblemId = p.Id ");
                query.AppendLine("          Where psp.ProblemSetId = prereq.RequiredSetId ");
                query.AppendLine("          and s.UserId = @userId and s.IsCorrect = 1 ");
                query.AppendLine("      ) ");
                query.AppendLine("  ) Then 'Locked' ");
                query.AppendLine("  Else 'Unlocked' ");
                query.AppendLine("End as 'Status' ");
                query.AppendLine("from dbo.[" + tableName + "] ps ");
                query.AppendLine("Where ps.ClassId = @clsId ");
                query.AppendLine("and ps.Id in ( Select ProblemSetId from dbo.[ProblemSetProblem] ) ");

                cmd.CommandText = query.ToString();

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            ProblemSetData s = createFromReader(reader);
                            string status = (string)reader["Status"];
                            if (status == "Solved")
                                solved.Add(s);
                            else if (status == "Locked")
                                locked.Add(s);
                            else if (status == "Unlocked")
                                unlocked.Add(s);
                            else
                                Console.WriteLine("Found invalid status " + status + " in ProblemSetDao.GetForStudent()");
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

                return Tuple.Create(unlocked, locked, solved);
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
        /// Removes the specified prerequisites. If the list of prereqs to remove is null or empty,
        /// this method will instead remove all prerequisites for the parent set.
        /// </summary>
        /// <param name="set">The ProblemSetData object with the parent set's id</param>
        /// <param name="prereqs">A collection of ProblemSetData objects with the prerequisite sets' ids</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemovePrereqs(ProblemSetData set, IEnumerable<ProblemSetData> prereqs)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.Append("Delete from dbo.[Prereq] Where ProblemSetId = @setId");

                if (prereqs != null && prereqs.Any())
                {
                    query.Append(" and RequiredSetId in (");
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
        /// Removes all prerequisites for sets in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemovePrereqsForClass(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Delete from dbo.[Prereq] p "
                    + "Join dbo.[" + tableName + "] ps on ps.Id = p.ProblemSetId "
                    + "Where ps.ClassId = @clsId;";

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

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
