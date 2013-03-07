using core.Modules.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace core.Modules.User
{
    public class UserDao : BaseDao<UserData>
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
                string cmdStr = "Insert into dbo.[" + tableName + "] (Id, Email";
                string paramList = ") values (@id, @email";

                SqlCommand cmd = conn.CreateCommand();

                //Id
                cmd.Parameters.AddWithValue("@id", user.Id);

                //Email
                cmd.Parameters.AddWithValue("@email", user.Email);

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
        /// Checks if the specified user is a student in the specified class.
        /// </summary>
        /// <param name="user">The UserData object with the user's id</param>
        /// <param name="cls">The ClassData object with the class's id</param>
        /// <returns>true if user is a student in cls, false otherwise</returns>
        public bool IsStudent(UserData user, ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select count(1) from dbo.[Student] Where UserId = @userId and ClassId = @clsId;";

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                //Class
                cmd.Parameters.AddWithValue("@clsId", cls.Id);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

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
        /// This method only exists so the superclass BaseDao can polymorphically call createFromReader.
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
            user.FirstName = reader["FirstName"] as string;
            user.LastName = reader["LastName"] as string;
            user.IsAdmin = (bool)reader["IsAdmin"];
            return user;
        }
    }
}
