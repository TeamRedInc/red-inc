using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace core.Modules
{
    public abstract class DataAccessObject<T> where T : DataObject
    {
        protected readonly string connStr;
        protected readonly string tableName;

        public DataAccessObject(string tableName)
        {
            connStr = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString).ToString();
            this.tableName = tableName;
        }

        /// <summary>
        /// Gets the data object with the specified id.</summary>
        /// <param name="id">The id of the data object to get</param>
        /// <returns>
        /// A DataObject object representing the data object if found, null otherwise</returns>
        public T GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[" + tableName + "] where Id = @id;";

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
                        return createObjectFromReader(reader);
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
        /// Gets all data objects in the database.</summary>
        /// <returns>
        /// A non-null, possibly empty list of filled DataObject objects</returns>
        public List<T> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<T> objects = new List<T>();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from dbo.[" + tableName + "];";

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            objects.Add(createObjectFromReader(reader));
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

                return objects;
            }
        }

        /// <summary>
        /// Creates a data object from a SqlDataReader.</summary>
        /// <param name="reader">The SqlDataReader to get object data from</param>
        /// <returns>
        /// A DataObject object</returns>
        public abstract T createObjectFromReader(SqlDataReader reader);
    }
}
