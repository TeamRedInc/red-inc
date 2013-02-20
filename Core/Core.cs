using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class Core
    {
        /// <summary>
        /// Primary thread of execution
        /// All logic starts here!
        /// </summary>
        public Core()
        {
            try // Catching everything so we can continue work while disregarding SQL connectivity issues
            {
                SqlConnectionStringBuilder csBuilder =
                    new SqlConnectionStringBuilder(
                        ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);

                using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "Insert into dbo.[User] (Email, PasswordHash) values (@email, @pwd)";

                    cmd.Parameters.Add("@email", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);

                    cmd.Parameters["@email"].Value = "jwien3@mail.gatech.edu";
                    cmd.Parameters["@pwd"].Value = "pass123";

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from dbo.[User]";

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = (int) reader["Id"];
                            String Email = (String) reader["Email"];
                            bool isAdmin = (bool) reader["IsAdmin"];
                            Console.WriteLine("{0}: {1} {2}", id, Email, isAdmin);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
