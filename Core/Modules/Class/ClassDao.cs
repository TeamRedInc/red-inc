using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Class
{
    public class ClassDao
    {
        private SqlConnectionStringBuilder csBuilder;

        public ClassDao()
        {
            csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);
        }

        /// <summary>
        /// Add a new class to the database.</summary>
        /// <param name="cls">The ClassData object with the class's information</param>
        /// <returns>
        /// true if the add was successful, false otherwise</returns>
        public bool Add(ClassData cls)
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Class] (Name, InstructorId) values (@name, @instrId);";

                //Name
                cmd.Parameters.Add("@name", SqlDbType.NVarChar);
                cmd.Parameters["@name"].Value = cls.Name;

                //Password hash
                cmd.Parameters.Add("@instrId", SqlDbType.Int);
                cmd.Parameters["@instrId"].Value = cls.Instructor.Id;

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
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Student] values (@studentId, @clsId);";

                //Student
                cmd.Parameters.Add("@studentId", SqlDbType.Int);
                cmd.Parameters["@studentId"].Value = student.Id;

                //Class
                cmd.Parameters.Add("@clsId", SqlDbType.Int);
                cmd.Parameters["@clsId"].Value = cls.Id;

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
        /// Gets the class with the specified id.</summary>
        /// <param name="id">The id of the class to get</param>
        /// <returns>
        /// A ClassData object representing the class if found, null otherwise</returns>
        public ClassData GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[Class] where Id = @id;";

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
        /// Gets all classes in the database.</summary>
        /// <returns>
        /// A non-null, possibly empty list of ClassData objects</returns>
        public List<ClassData> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                List<ClassData> classes = new List<ClassData>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[Class];";
                
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
        /// Creates a class from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get class data from</param>
        /// <returns>
        /// A ClassData object</returns>
        public ClassData createFromReader(SqlDataReader reader)
        {
            ClassData cls = new ClassData((int)reader["Id"]);
            cls.Name = reader["Name"] as string;
            cls.Instructor = new UserData((int)reader["InstructorId"]);
            return cls;
        }
    }
}
