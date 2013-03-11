using core.Modules;
using core.Modules.Class;
using core.Modules.ProblemSet;
using core.Modules.User;
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
        private readonly UserModel userModel;
        private readonly ProblemSetModel setModel;

        /// <summary>
        /// Primary thread of execution
        /// All logic starts here!
        /// </summary>
        public Core()
        {
            userModel = ModelFactory.UserModel;
            setModel = ModelFactory.ProblemSetModel;
        }

        public bool AddUser(int id, string email)
        {
            UserData user = new UserData(id);
            user.Email = email;

            return userModel.Add(user);
        }

        public List<ProblemSetData> GetSetsForStudent(int studentId, int classId)
        {
            return setModel.GetForStudent(new UserData(studentId), new ClassData(classId));
        }
    }
}
