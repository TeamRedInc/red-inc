using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.Problem
{
    [TestClass]
    public class ProblemTest
    {
        private ProblemModel problemModel = new ProblemModel();
        private ProblemSetModel setModel = new ProblemSetModel();

        [TestMethod]
        public void TestAdd()
        {
            ProblemData problem = new ProblemData(0);
            problem.Name = "Hello, World!";
            problem.Class = new ClassData(2);

            //This test works as of 10:30am on 2/21
            //Commenting it out to prevent cluttering the database
            //- Josh
            //Assert.IsTrue(problemModel.Add(problem));
        }

        [TestMethod]
        public void TestGet()
        {
            ProblemData problem = problemModel.GetById(0);

            Assert.IsNull(problem);

            problem = problemModel.GetById(1);

            Assert.AreEqual("Hello, World!", problem.Name);
            Assert.AreEqual(2, problem.Class.Id);

            List<ProblemData> problems = problemModel.GetAll();

            Assert.IsTrue(problems.Contains(problem));
        }

        [TestMethod]
        public void TestUpdateSets()
        {
            ProblemData prob = new ProblemData(3);
            List<ProblemSetData> original = setModel.GetForProblem(prob);

            //Test remove from sets
            Assert.IsTrue(problemModel.UpdateSets(prob, new List<ProblemSetData>()));
            Assert.AreEqual(0, setModel.GetForProblem(prob).Count);

            //Test add to sets
            Assert.IsTrue(problemModel.UpdateSets(prob, original));
            CollectionAssert.AreEqual(original, setModel.GetForProblem(prob));

            //Test add bad data
            List<ProblemSetData> badList = new List<ProblemSetData>(original);
            badList.Add(new ProblemSetData { Id = original[0].Id, PrereqCount = original[0].PrereqCount }); //Can't have duplicate set entries
            Assert.IsTrue(problemModel.UpdateSets(prob, badList));
            CollectionAssert.AreEqual(original, setModel.GetForProblem(prob));
        }

        [TestMethod]
        public void TestGetForSet()
        {
            List<ProblemData> problems = problemModel.GetForSet(new ProblemSetData(1));

            Assert.IsTrue(problems.Contains(new ProblemData(1)));
            Assert.IsTrue(problems.Contains(new ProblemData(2)));
            Assert.IsTrue(problems.Contains(new ProblemData(3)));
        }
    }
}
