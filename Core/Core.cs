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
            //userModel = ModelFactory.UserModel;
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

        public string ExecutePythonCode(string code)
        {
            var py = new Modules.PyInterpret.PyInterpretUtility();
            return py.FutureInterpret(code);
        }

        public ProblemSetData GetSetById(int setId)
        {
            return setModel.GetById(setId);
        }

        public bool ModifySet(ProblemSetData set)
        {
            return setModel.Modify(set);
        }

        public List<ProblemSetData> GetSetPrereqs(int setId)
        {
            return setModel.GetPrereqs(new ProblemSetData(setId));
        }

        public List<ProblemSetData> GetSetsForClass(int classId)
        {
            return setModel.GetForClass(new ClassData(classId));
        }

        public bool UpdateSetPrereqs(int setId, ICollection<ProblemSetData> prereqs)
        {
            return setModel.UpdatePrereqs(new ProblemSetData(setId), prereqs);
        }
    }
}