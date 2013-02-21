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
    public class UserDao
    {
        private SqlConnectionStringBuilder csBuilder;

        public UserDao()
        {
            csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);
        }

        /// <summary>
        /// Add a new user to the database.</summary>
        /// <param name="user">The UserData object with the user's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool AddUser(UserData user)
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                string cmdStr = "Insert into dbo.[User] (Email, PasswordHash";
                string paramList = ") values (@email, @pwd";

                SqlCommand cmd = conn.CreateCommand();

                //Email
                cmd.Parameters.Add("@email", SqlDbType.NVarChar);
                cmd.Parameters["@email"].Value = user.Email;

                //Password hash
                cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);
                cmd.Parameters["@pwd"].Value = user.PasswordHash;

                //First name
                if (!String.IsNullOrEmpty(user.FirstName)) 
                {
                    cmdStr += ", FirstName";
                    paramList += ", @fname";
                    cmd.Parameters.Add("@fname", SqlDbType.NVarChar);
                    cmd.Parameters["@fname"].Value = user.FirstName;
                }

                //Last name
                if (!String.IsNullOrEmpty(user.LastName))
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
                    Console.WriteLine(e.Message);
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
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select count(*) from dbo.[User] where Email = @email and PasswordHash = @pwd";

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
                    Console.WriteLine(e.Message);
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
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
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
                    Console.WriteLine(e.Message);
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
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
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
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the user with the specified id.</summary>
        /// <param name="id">The id of the user to get</param>
        /// <returns>
        /// A UserData object representing the user if found, null otherwise</returns>
        public UserData GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[User] where Id = @id;";

                //Id
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createFromReader(reader);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets all users in the database.</summary>
        /// <returns>
        /// A non-null, possibly empty list of UserData objects</returns>
        public List<UserData> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                List<UserData> users = new List<UserData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[User];";

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            users.Add(createFromReader(reader));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return users;
            }
        }

        /// <summary>
        /// Creates a user from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get user data from</param>
        /// <returns>
        /// A UserData object</returns>
        public UserData createFromReader(SqlDataReader reader)
        {
            UserData user = new UserData((int)reader["Id"]);
            user.Email = (string)reader["Email"];
            user.PasswordHash = (string)reader["PasswordHash"];
            user.FirstName = (string)reader["FirstName"];
            user.LastName = (string)reader["LastName"];
            user.IsAdmin = (bool)reader["IsAdmin"];
            return user;
        }
    }
}
