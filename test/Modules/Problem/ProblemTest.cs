using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.Problem;
using System.Collections.Generic;

namespace core.Tests.Modules.Problem
{
    [TestClass]
    public class ProblemTest
    {
        private ProblemDao problemDao = new ProblemDao();

        [TestMethod]
        public void TestAdd()
        {
            ProblemData problem = new ProblemData(0);
            problem.Name = "Hello, World!";

            //This test works as of 10:30am on 2/21
            //Commenting it out to prevent cluttering the database
            //- Josh
            //Assert.IsTrue(problemDao.Add(problem));
        }

        [TestMethod]
        public void TestGet()
        {
            Console.WriteLine("Here");
            ProblemData problem = problemDao.GetById(0);

            Assert.IsNull(problem);

            problem = problemDao.GetById(3);

            Assert.AreEqual("Hello, World!", problem.Name);
            Assert.IsNull(problem.Description);

            List<ProblemData> problems = problemDao.GetAll();

            problem = problemDao.GetById(problems[0].Id);

            Assert.AreEqual(problems[0], problem);
        }
    }
}
