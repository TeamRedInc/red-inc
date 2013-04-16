using core.Modules.Class;
using core.Modules.Logging;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.Progress;
using core.Modules.User;
using System;

namespace core.Modules
{
    public static class DaoFactory
    {
        private static readonly Lazy<UserDao> userDao = new Lazy<UserDao>();
        private static readonly Lazy<ClassDao> classDao = new Lazy<ClassDao>();
        private static readonly Lazy<ProblemSetDao> problemSetDao = new Lazy<ProblemSetDao>();
        private static readonly Lazy<ProblemDao> problemDao = new Lazy<ProblemDao>();
        private static readonly Lazy<ProgressDao> progressDao = new Lazy<ProgressDao>();
        private static readonly Lazy<LoggingDao> loggingDao = new Lazy<LoggingDao>();

        public static UserDao UserDao
        {
            get { return userDao.Value; }
        }

        public static ClassDao ClassDao
        {
            get { return classDao.Value; }
        }

        public static ProblemSetDao ProblemSetDao
        {
            get { return problemSetDao.Value; }
        }

        public static ProblemDao ProblemDao
        {
            get { return problemDao.Value; }
        }

        public static ProgressDao ProgressDao
        {
            get { return progressDao.Value; }
        }

        public static LoggingDao LoggingDao
        {
            get { return loggingDao.Value; }
        }
    }
}
