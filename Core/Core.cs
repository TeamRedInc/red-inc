using core.Modules;
using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.User;
using core.Modules.Class;
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
        private readonly ProblemModel problemModel;
        private readonly ClassModel classModel;

        /// <summary>
        /// Primary thread of execution
        /// All logic starts here!
        /// </summary>
        public Core()
        {
            userModel = ModelFactory.UserModel;
            setModel = ModelFactory.ProblemSetModel;
            problemModel = ModelFactory.ProblemModel;
            classModel = ModelFactory.ClassModel;
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

        public ProblemData GetProblemById(int problemId)
        {
            return problemModel.GetById(problemId);
        }

        public List<ProblemSetData> GetSetsForProblem(int problemId)
        {
            return setModel.GetForProblem(new ProblemData(problemId));
        }

        public bool ModifyProblem(ProblemData prob)
        {
            return problemModel.Modify(prob);
        }

        public List<ClassData> GetAll()
        {
            List<ClassData> classList = new List<ClassData>();
            return classModel.GetAll();
        }

        public List<ClassData> GetStudentClasses(int userId)
        {
            List<ClassData> classList = new List<ClassData>();
            return classModel.GetStudentClasses(new UserData(userId));
        }

        public List<ClassData> GetInstructorClasses(int userId)
        {
            List<ClassData> classList = new List<ClassData>();
            return classModel.GetInstructorClasses(new UserData(userId));
        }
    }
}