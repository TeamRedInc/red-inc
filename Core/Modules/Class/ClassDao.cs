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
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar);
                    cmd.Parameters["@name"].Value = cls.Name;
                }

                //Instructor
                cmdStr += ", InstructorId";
                paramList += ", @instrId";
                cmd.Parameters.Add("@instrId", SqlDbType.Int);
                cmd.Parameters["@instrId"].Value = cls.Instructor.Id;

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
