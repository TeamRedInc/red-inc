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
    }
}
