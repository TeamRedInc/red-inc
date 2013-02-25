using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace core.Modules.Class
{
    public class ClassDao : DataAccessObject<ClassData>
    {
        public ClassDao() : base("Class") { }

        /// <summary>
        /// Add a new class to the database.</summary>
        /// <param name="cls">The ClassData object with the class's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string cmdStr = "Insert into dbo.[" + tableName + "] (";
                string paramList = ") values (";

                SqlCommand cmd = conn.CreateCommand();

                //Name
                if (!String.IsNullOrWhiteSpace(cls.Name))
                {
                    cmdStr += "Name";
                    paramList += "@name";
                    cmd.Parameters.AddWithValue("@name", cls.Name);
                }

                //Instructor
                cmdStr += ", InstructorId";
                paramList += ", @instrId";
                cmd.Parameters.AddWithValue("@instrId", cls.Instructor.Id);

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
        /// Gets all classes the specified user is a student in.
        /// </summary>
        /// <param name="user">The UserData object with the student's id</param>
        /// <returns>A non-null, possibly empty list of filled ClassData objects</returns>
        public List<ClassData> GetStudentClasses(UserData user)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<ClassData> classes = new List<ClassData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[Student] s"
                    + " Join dbo.[" + tableName + "] c on c.Id = s.ClassId"
                    + " Where UserId = @userId;";

                //User
                cmd.Parameters.AddWithValue("@userId", user.Id);

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            classes.Add(createFromReader(reader));
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

                return classes;
            }
        }

        /// <summary>
        /// Gets all classes the specified user is an instructor for.
        /// </summary>
        /// <param name="user">The UserData object with the instructor's id</param>
        /// <returns>A non-null, possibly empty list of filled ClassData objects</returns>
        public List<ClassData> GetInstructorClasses(UserData user)
        {
            return base.GetAll("InstructorId", user.Id);
        }

        /// <summary>
        /// This method only exists so the superclass DataAccessObject can polymorphically call createFromReader.
        /// Use of the static createFromReader(SqlDataReader) method is preferred.
        /// </summary>
        public override ClassData createObjectFromReader(SqlDataReader reader)
        {
            return ClassDao.createFromReader(reader);
        }

        /// <summary>
        /// Creates a class from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get class data from</param>
        /// <returns>
        /// A ClassData object</returns>
        public static ClassData createFromReader(SqlDataReader reader)
        {
            ClassData cls = new ClassData((int)reader["Id"]);
            cls.Name = reader["Name"] as string;
            cls.Instructor = new UserData((int)reader["InstructorId"]);
            return cls;
        }
    }
}
