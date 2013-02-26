using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                cmd.Parameters.AddWithValue("@email", user.Email);

                //Password hash
                cmd.Parameters.AddWithValue("@pwd", user.PasswordHash);

                //First name
                if (!String.IsNullOrWhiteSpace(user.FirstName)) 
                {
                    cmdStr += ", FirstName";
                    paramList += ", @fname";
                    cmd.Parameters.AddWithValue("@fname", user.FirstName);
                }

                //Last name
                if (!String.IsNullOrWhiteSpace(user.LastName))
                {                    
                    cmdStr += ", LastName";
                    paramList += ", @lname";
                    cmd.Parameters.AddWithValue("@lname", user.LastName);
                }

                //Admin?
                if (user.IsAdmin)
                {                    
                    cmdStr += ", IsAdmin";
                    paramList += ", @isAdmin";
                    cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);
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
        /// The filled UserData object representing the logged in user, or null if login failed</returns>
        public UserData Login(UserData user)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[" + tableName + "] where Email = @email and PasswordHash = @pwd;";

                //Email
                cmd.Parameters.AddWithValue("@email", user.Email);

                //Password hash
                cmd.Parameters.AddWithValue("@pwd", user.PasswordHash);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        UserData u = createFromReader(reader);
                        //Successfully return only if there is one matching row
                        if (!reader.Read())
                            return u;
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

                return null;
            }
        }

        /// <summary>
        /// Adds a user to a class as a student.</summary>
        /// <param name="student">The UserData object with the student user's id</param>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool AddStudent(UserData student, ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Student] values (@studentId, @clsId);";

                //Student
                cmd.Parameters.AddWithValue("@studentId", student.Id);

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
        /// Gets all students in the specified class.
        /// </summary>
        /// <param name="cls">The ClassData object with the class' id</param>
        /// <returns>A non-null, possibly empty list of filled UserData objects</returns>
        public List<UserData> GetStudents(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<UserData> students = new List<UserData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[Student] s"
                    + " Join dbo.[" + tableName + "] u on u.Id = s.UserId"
                    + " Where ClassId = @clsId;";

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            students.Add(createFromReader(reader));
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

                return students;
            }
        }

        /// <summary>
        /// This method only exists so the superclass DataAccessObject can polymorphically call createFromReader.
        /// Use of the static createFromReader(SqlDataReader) method is preferred.
        /// </summary>
        public override UserData createObjectFromReader(SqlDataReader reader)
        {
            return UserDao.createFromReader(reader);
        }

        /// <summary>
        /// Creates a user from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get user data from</param>
        /// <returns>
        /// A UserData object</returns>
        public static UserData createFromReader(SqlDataReader reader)
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
