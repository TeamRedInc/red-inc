using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace core.Modules.Logging
{
    public class LoggingDao
    {
        protected readonly string connStr;

        public LoggingDao()
        {
            connStr = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).ToString();
        }

        /// <summary>
        /// Adds a new log entry to the database.
        /// </summary>
        /// <param name="log">The LogData object with the logging information</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool Add(LogData log)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "Insert into dbo.[Log] (UserId, Message) values (@userId, @message);";

                //User
                cmd.Parameters.AddWithValue("@userId", log.User.Id);

                //Message
                cmd.Parameters.AddWithValue("@message", log.Message);
                
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
        /// Gets a list of logs.
        /// If both a begin and end DateTime are provided, all dates in the range [begin, end) are returned.
        /// Otherwise all logs are returned.
        /// </summary>
        /// <param name="begin">The earliest DateTime to include logs from</param>
        /// <param name="end">The latest DateTime to include logs from</param>
        /// <returns>A non-null, possibly empty list of LogData objects</returns>
        public List<LogData> Get(DateTime? begin = null, DateTime? end = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                List<LogData> logs = new List<LogData>();
                SqlCommand cmd = conn.CreateCommand();

                string cmdStr = "Select * from dbo.[Log] ";

                if (begin != null && end != null)
                {
                    cmdStr += "Where DateTime >= @begin and DateTime < @end ";
                    cmd.Parameters.AddWithValue("@begin", begin);
                    cmd.Parameters.AddWithValue("@end", end);
                }

                cmdStr += "Order by DateTime DESC;";

                cmd.CommandText = cmdStr;

                SqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                            logs.Add(createFromReader(reader));
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

                return logs;
            }
        }

        public static LogData createFromReader(SqlDataReader reader)
        {
            LogData log = new LogData();
            log.DateTime = (DateTime)reader["DateTime"];
            log.User = new UserData((int)reader["UserId"]);
            log.Message = reader["Message"] as string;
            return log;
        }
    }
}
