using core.Modules.Class;
using core.Modules.ProblemSet;
using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace core.Modules.Progress
{
    public class ProgressDao
    {        
        protected readonly string connStr;

        public ProgressDao()
        {
            connStr = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).ToString();
        }

        /// <summary>
        /// Calculates aggregated progress data for all students in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>A non-null, possibly empty list of StudentProgress objects</returns>
        public List<StudentProgress> GetStudentProgress(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<StudentProgress> list = new List<StudentProgress>();
                SqlCommand cmd = conn.CreateCommand();

                string selectCols = "u.Id, u.Email, u.FirstName, u.LastName";

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select " + selectCols + ", ");
                query.AppendLine("  Count(Case s.IsCorrect When 1 Then 1 Else null End) as NumCorrect, ");
                query.AppendLine("  Avg(Cast(s.NumAttempts as float)) as AvgAttempts ");
                query.AppendLine("from dbo.[User] u ");
                query.AppendLine("Join dbo.[Problem] p on p.ClassId = @clsId ");
                query.AppendLine("Join dbo.[Solution] s on s.UserId = u.Id and s.ProblemId = p.Id ");
                query.AppendLine("Group by " + selectCols);

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                cmd.CommandText = query.ToString();

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            list.Add(createStudentProgressFromReader(reader));
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

                return list;
            }
        }

        /// <summary>
        /// Calculates aggregated progress data for all sets in the specified class.
        /// If a non-null user is passed in, then the data is limited to that student's progress.
        /// </summary>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <param name="user">The UserData object with the user's id (optional)</param>
        /// <returns>A non-null, possibly empty list of SetProgress objects</returns>
        public List<SetProgress> GetSetProgress(ClassData cls, UserData user = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<SetProgress> list = new List<SetProgress>();
                SqlCommand cmd = conn.CreateCommand();

                string selectCols = "ps.Id, ps.Name";

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select " + selectCols + ", ");
                query.AppendLine("  Count(Case s.IsCorrect When 1 Then 1 Else null End) as NumCorrect, ");
                query.AppendLine("  Avg(Cast(s.NumAttempts as float)) as AvgAttempts ");
                query.AppendLine("from dbo.[ProblemSet] ps ");
                query.AppendLine("Join dbo.[ProblemSetProblem] psp on psp.ProblemSetId = ps.Id ");
                query.AppendLine("Join dbo.[Solution] s on s.ProblemId = psp.ProblemId ");
                query.AppendLine("Where ps.ClassId = @clsId ");
                if (user != null)
                {
                    query.AppendLine("and (s.UserId = @userId or s.UserId is null) ");
                    cmd.Parameters.AddWithValue("@userId", user.Id);
                }
                query.AppendLine("Group by " + selectCols);

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                cmd.CommandText = query.ToString();

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            list.Add(createSetProgressFromReader(reader));
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

                return list;
            }
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
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ProblemProgress> list = new List<ProblemProgress>();
                SqlCommand cmd = conn.CreateCommand();

                string selectCols = "probs.Id, probs.Name";

                StringBuilder query = new StringBuilder();
                query.AppendLine("Select " + selectCols + ", ");
                query.AppendLine("  Count(Case probs.IsCorrect When 1 Then 1 Else null End) as NumCorrect, ");
                query.AppendLine("  Avg(Cast(probs.NumAttempts as float)) as AvgAttempts ");
                query.AppendLine("from ( ");
                query.AppendLine("  Select distinct p.*, s.IsCorrect, IsNull(s.NumAttempts, 0) as NumAttempts ");
                query.AppendLine("  from dbo.[Problem] p ");
                query.AppendLine("  Left Join dbo.[Solution] s on s.ProblemId = p.Id ");
                query.AppendLine("  Join dbo.[ProblemSetProblem] psp on psp.ProblemId = p.Id ");
                query.AppendLine("  Where p.ClassId = @clsId ");
                if (user != null)
                {
                    query.AppendLine("  and (s.UserId = @userId or s.UserId is null) ");
                    cmd.Parameters.AddWithValue("@userId", user.Id);
                }
                if (set != null)
                {
                    query.AppendLine("  and psp.ProblemSetId = @setId ");
                    cmd.Parameters.AddWithValue("@setId", set.Id);
                }
                query.AppendLine(") probs ");
                query.AppendLine("Group by " + selectCols);

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                cmd.CommandText = query.ToString();

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            list.Add(createProblemProgressFromReader(reader));
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

                return list;
            }
        }

        public static StudentProgress createStudentProgressFromReader(SqlDataReader reader)
        {
            StudentProgress sp = new StudentProgress((int)reader["Id"]);
            sp.Email = (string)reader["Email"];
            sp.FirstName = reader["FirstName"] as string;
            sp.LastName = reader["LastName"] as string;
            sp.NumCorrect = (int)reader["NumCorrect"];
            sp.AvgAttempts = (double)reader["AvgAttempts"];
            return sp;
        }

        public static SetProgress createSetProgressFromReader(SqlDataReader reader)
        {
            SetProgress sp = new SetProgress((int)reader["Id"]);
            sp.Name = reader["Name"] as string;
            sp.NumCorrect = (int)reader["NumCorrect"];
            sp.AvgAttempts = (double)reader["AvgAttempts"];
            return sp;
        }

        public static ProblemProgress createProblemProgressFromReader(SqlDataReader reader)
        {
            ProblemProgress pp = new ProblemProgress((int)reader["Id"]);
            pp.Name = reader["Name"] as string;
            pp.NumCorrect = (int)reader["NumCorrect"];
            pp.AvgAttempts = (double)reader["AvgAttempts"];
            return pp;
        }
    }
}
