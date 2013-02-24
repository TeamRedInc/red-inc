using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.ProblemSet;
using core.Modules.Class;
using System.Collections.Generic;

namespace core.Tests.Modules.ProblemSet
{
    [TestClass]
    public class ProblemSetTest
    {
        private ProblemSetDao setDao = new ProblemSetDao();

        [TestMethod]
        public void TestAdd()
        {
            ProblemSetData set = new ProblemSetData(0);
            set.Name = "Beginner Problems";
            set.Class = new ClassData(1);

            //This test works as of 5:15pm on 2/22
            //Commenting it out to prevent cluttering the database
            //- Josh
            Assert.IsTrue(setDao.Add(set));
        }

        [TestMethod]
        public void TestGet()
        {
            ProblemSetData set = setDao.GetById(0);

            Assert.IsNull(set);

            set = setDao.GetById(1);

            Assert.AreEqual("Beginner Problems", set.Name);
            Assert.AreEqual(1, set.Class.Id);

            List<ProblemSetData> sets = setDao.GetAll();

            set = setDao.GetById(sets[0].Id);

            Assert.AreEqual(sets[0], set);
        }
    }
}
