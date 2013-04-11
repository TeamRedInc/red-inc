using core.Modules;
using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.User;
using System;
using System.Collections.Generic;

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

        public bool AddUser(UserData user)
        {
            return userModel.Add(user);
        }

        public Tuple<List<ProblemSetData>, List<ProblemSetData>, List<ProblemSetData>> GetSetsForStudent(int studentId, int classId)
        {
            return setModel.GetForStudent(new UserData(studentId), new ClassData(classId));
        }

        public string ExecutePythonCode(string code)
        {
            var py = new Modules.PyInterpret.PyInterpretUtility();
            return py.Execute(code);
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

        public List<ClassData> GetAllClasses()
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

        public bool AddClass(ClassData newClass)
        {
            return classModel.Add(newClass);
        }

        public bool UpdateProblemSets(int problemId, ICollection<ProblemSetData> sets)
        {
            return problemModel.UpdateSets(new ProblemData(problemId), sets);
        }

        public List<ProblemSetData> SearchSetsInClass(int classId, string search)
        {
            return setModel.SearchInClass(new ClassData(classId), search);
        }

        public List<ProblemData> GetProblemsForSet(ProblemSetData set)
        {
            return problemModel.GetForSet(set);
        }

        public bool AddStudent(int userId, int classId)
        {
            return userModel.AddStudent(new UserData(userId), new ClassData(classId));
        }

        public bool IsStudent(int userId, int classId)
        {
            return userModel.IsStudent(new UserData(userId), new ClassData(classId));
        }

        public ClassData GetClassById(int classId)
        {
            return classModel.GetById(classId);
        }

        public List<UserData> GetStudentsForClass(int classId)
        {
            return userModel.GetStudents(new ClassData(classId));
        }

        public int AddProblem(ProblemData prob)
        {
            return problemModel.Add(prob);
        }

        public int AddSet(ProblemSetData set)
        {
            return setModel.Add(set);
        }

        public List<ProblemData> GetUnsolvedProblemsForSet(int setId, int userId)
        {
            return problemModel.GetForSetAndUser(new ProblemSetData(setId), new UserData(userId), false);
        }

        public bool ModifyClass(ClassData cls)
        {
            return classModel.Modify(cls);
        }

        public bool DeleteProblem(int problemId)
        {
            return problemModel.Delete(new ProblemData(problemId));
        }

        public bool DeleteSet(int setId)
        {
            return setModel.Delete(new ProblemSetData(setId));
        }

        public bool DeleteClass(int classId)
        {
            return classModel.Delete(new ClassData(classId));
        }

        /// <summary>
        /// Tests student code against the instructor solution code for the specified problem
        /// and records the result in the database if desired.
        /// </summary>
        /// <param name="userId">The currently logged in user solving the problem</param>
        /// <param name="studentCode">The student solution code</param>
        /// <param name="problemId">The problem being solved</param>
        /// <param name="record">true to record this attempt in the database, false otherwise (optional, defaults to true)</param>
        /// <returns>null if the solution is correct, otherwise an error message</returns>
        public string SolveProblem(int userId, string studentCode, int problemId, bool record = true)
        {
            ProblemData prob = problemModel.GetById(problemId);
            var py = new Modules.PyInterpret.PyInterpretUtility();

            string output = py.Test(studentCode, prob.SolutionCode);
            bool correct = false;

            if (output == "Correct")
                correct = true;

            if (record)
                problemModel.UpdateSolution(new UserData(userId), prob, correct);

            if (correct)
                return null;
            else
                return output;
        }
    }
}