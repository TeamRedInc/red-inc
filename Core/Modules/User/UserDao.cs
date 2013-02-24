using core.Modules.Problem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.User
{
    public class UserDao : DataAccessObject<UserData>
    {
        public UserDao() : base("User") { }

        /// <summary>
        /// Add a new user to the database.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(UserData user)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string cmdStr = "Insert into dbo.[" + tableName + "] (Email, PasswordHash";
                string paramList = ") values (@email, @pwd";

                SqlCommand cmd = conn.CreateCommand();

                //Email
                cmd.Parameters.Add("@email", SqlDbType.NVarChar);
                cmd.Parameters["@email"].Value = user.Email;

                //Password hash
                cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);
                cmd.Parameters["@pwd"].Value = user.PasswordHash;

                //First name
                if (!String.IsNullOrWhiteSpace(user.FirstName)) 
                {
                    cmdStr += ", FirstName";
                    paramList += ", @fname";
                    cmd.Parameters.Add("@fname", SqlDbType.NVarChar);
                    cmd.Parameters["@fname"].Value = user.FirstName;
                }

                //Last name
                if (!String.IsNullOrWhiteSpace(user.LastName))
                {                    
                    cmdStr += ", LastName";
                    paramList += ", @lname";
                    cmd.Parameters.Add("@lname", SqlDbType.NVarChar);
                    cmd.Parameters["@lname"].Value = user.LastName;
                }

                //Admin?
                if (user.IsAdmin)
                {                    
                    cmdStr += ", IsAdmin";
                    paramList += ", @isAdmin";
                    cmd.Parameters.Add("@isAdmin", SqlDbType.Bit);
                    cmd.Parameters["@isAdmin"].Value = user.IsAdmin;
                }

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
        /// Verify a user's login information.</summary>
        /// <param name="user">The UserData object with login credentials</param>
        /// <returns>
        /// true if the user is in the database and the password hashes match, false otherwise</returns>
        public bool Login(UserData user)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select count(*) from dbo.[" + tableName + "] where Email = @email and PasswordHash = @pwd";

                //Email
                cmd.Parameters.Add("@email", SqlDbType.NVarChar);
                cmd.Parameters["@email"].Value = user.Email;

                //Password hash
                cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);
                cmd.Parameters["@pwd"].Value = user.PasswordHash;

                try
                {
                    conn.Open();
                    int count = (int) cmd.ExecuteScalar();

                    return count == 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
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
                cmd.Parameters.Add("@userId", SqlDbType.Int);
                cmd.Parameters["@userId"].Value = user.Id;

                //Problem
                cmd.Parameters.Add("@problemId", SqlDbType.Int);
                cmd.Parameters["@problemId"].Value = problem.Id;

                //Correct
                cmd.Parameters.Add("@correct", SqlDbType.Bit);
                cmd.Parameters["@correct"].Value = correct;

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
                cmd.Parameters.Add("@userId", SqlDbType.Int);
                cmd.Parameters["@userId"].Value = user.Id;

                //Problem
                cmd.Parameters.Add("@problemId", SqlDbType.Int);
                cmd.Parameters["@problemId"].Value = problem.Id;

                //Correct
                cmd.Parameters.Add("@correct", SqlDbType.Bit);
                cmd.Parameters["@correct"].Value = correct;

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
        /// Creates a user from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get user data from</param>
        /// <returns>
        /// A UserData object</returns>
        public override UserData createFromReader(SqlDataReader reader)
        {
            UserData user = new UserData((int)reader["Id"]);
            user.Email = (string)reader["Email"];
            user.PasswordHash = (string)reader["PasswordHash"];
            user.FirstName = reader["FirstName"] as string;
            user.LastName = reader["LastName"] as string;
            user.IsAdmin = (bool)reader["IsAdmin"];
            return user;
        }
    }
}
