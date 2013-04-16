using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Logging
{
    public class LoggingModel
    {
        public LoggingModel() { }

        private LoggingDao loggingDao
        {
            get { return DaoFactory.LoggingDao; }
        }

        /// <summary>
        /// Adds a new log entry to the database.
        /// </summary>
        /// <param name="log">The LogData object with the logging information</param>
        /// <returns>true if the add was successful, false otherwise</returns>
        public bool Add(LogData log)
        {
            return loggingDao.Add(log);
        }

        /// <summary>
        /// Gets all logs for the given year, month, day, hour, minute, or second.
        /// </summary>
        /// <param name="y">year</param>
        /// <param name="m">month</param>
        /// <param name="d">day</param>
        /// <param name="h">hour</param>
        /// <param name="min">minute</param>
        /// <param name="s">second</param>
        /// <returns>A non-null, possibly empty list of LogData objects</returns>
        public List<LogData> GetByDate(int y, int m = -1, int d = -1, int h = -1, int min = -1, int s = -1)
        {
            DateTime begin, end;

            if (s >= 0)
            {
                begin = new DateTime(y, m, d, h, min, s);
                end = begin.AddSeconds(1);
            }
            else if (min >= 0)
            {
                begin = new DateTime(y, m, d, h, min, 0);
                end = begin.AddMinutes(1);
            }
            else if (h >= 0)
            {
                begin = new DateTime(y, m, d, h, 0, 0);
                end = begin.AddHours(1);
            }
            else if (d >= 0)
            {
                begin = new DateTime(y, m, d);
                end = begin.AddDays(1);
            }
            else if (m >= 0)
            {
                begin = new DateTime(y, m, 1);
                end = begin.AddMonths(1);
            }
            else
            {
                begin = new DateTime(y, 1, 1);
                end = begin.AddYears(1);
            }

            return loggingDao.Get(begin, end);
        }

        /// <summary>
        /// Gets all logs in the database.
        /// </summary>
        /// <returns>A non-null, possibly empty list of LogData objects</returns>
        public List<LogData> GetAll()
        {
            return loggingDao.Get();
        }

        /// <summary>
        /// Gets a list of logs in the range [begin, end).
        /// </summary>
        /// <param name="begin">The earliest DateTime to include logs from</param>
        /// <param name="end">The latest DateTime to include logs from</param>
        /// <returns>A non-null, possibly empty list of LogData objects</returns>
        public List<LogData> GetInRange(DateTime begin, DateTime end)
        {
            return loggingDao.Get(begin, end);
        }
    }
}
