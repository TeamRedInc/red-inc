﻿using core.Modules.Problem;
using core.Modules.ProblemSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.Problem
{
    [TestClass]
    public class ProblemTest
    {
        private ProblemModel problemModel = new ProblemModel();

        [TestMethod]
        public void TestAdd()
        {
            ProblemData problem = new ProblemData(0);
            problem.Name = "Hello, World!";

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
            Assert.IsNull(problem.Description);

            List<ProblemData> problems = problemModel.GetAll();

            problem = problemModel.GetById(problems[0].Id);

            Assert.AreEqual(problems[0], problem);
        }

        [TestMethod]
        public void TestAddToSet()
        {
            ProblemData problem = new ProblemData(2);
            ProblemSetData set = new ProblemSetData(2);

            //This problem should already belong to this set
            Assert.IsFalse(problemModel.AddToSet(problem, set));
        }

        [TestMethod]
        public void TestRemoveFromSet()
        {
            ProblemData problem = new ProblemData(2);
            ProblemSetData set = new ProblemSetData(2);

            Assert.IsTrue(problemModel.RemoveFromSet(problem, set));

            Assert.IsTrue(problemModel.AddToSet(problem, set));
        }

        [TestMethod]
        public void TestGetForSet()
        {
            List<ProblemData> problems = problemModel.GetForSet(new ProblemSetData(1));

            Assert.IsTrue(problems.Contains(new ProblemData(1)));
            Assert.IsTrue(problems.Contains(new ProblemData(2)));
        }
    }
}
