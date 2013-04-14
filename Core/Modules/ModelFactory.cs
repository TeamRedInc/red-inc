﻿using core.Modules.Class;
using core.Modules.Logging;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.Progress;
using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules
{
    public static class ModelFactory
    {
        private static readonly Lazy<UserModel> userModel = new Lazy<UserModel>();
        private static readonly Lazy<ClassModel> classModel = new Lazy<ClassModel>();
        private static readonly Lazy<ProblemSetModel> problemSetModel = new Lazy<ProblemSetModel>();
        private static readonly Lazy<ProblemModel> problemModel = new Lazy<ProblemModel>();
        private static readonly Lazy<ProgressModel> progressModel = new Lazy<ProgressModel>();
        private static readonly Lazy<LoggingModel> loggingModel = new Lazy<LoggingModel>();

        public static UserModel UserModel
        {
            get
            {
                try { return userModel.Value; }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
            }
        }   

        public static ClassModel ClassModel
        {
            get { return classModel.Value; }
        }

        public static ProblemSetModel ProblemSetModel
        {
            get { return problemSetModel.Value; }
        }

        public static ProblemModel ProblemModel
        {
            get { return problemModel.Value; }
        }

        public static ProgressModel ProgressModel
        {
            get { return progressModel.Value; }
        }

        public static LoggingModel LoggingModel
        {
            get { return loggingModel.Value; }
        }
    }
}
