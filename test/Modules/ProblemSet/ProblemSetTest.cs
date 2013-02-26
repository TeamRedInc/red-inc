using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            //Assert.IsTrue(setDao.Add(set));
        }

        [TestMethod]
        public void TestGet()
        {
            ProblemSetData set = setDao.GetById(0);

            Assert.IsNull(set);

            set = setDao.GetById(1);

            Assert.AreEqual("Beginner Problems", set.Name);
            Assert.AreEqual(2, set.Class.Id);

            List<ProblemSetData> sets = setDao.GetAll();

            set = setDao.GetById(sets[0].Id);

            Assert.AreEqual(sets[0], set);
        }

        [TestMethod]
        public void TestGetForClass()
        {
            List<ProblemSetData> sets = setDao.GetForClass(new ClassData(1));

            Assert.IsNotNull(sets);
            Assert.AreEqual(0, sets.Count);

            sets = setDao.GetForClass(new ClassData(2));

            Assert.IsTrue(sets.Contains(new ProblemSetData(1)));
            Assert.IsTrue(sets.Contains(new ProblemSetData(2)));
        }

        [TestMethod]
        public void TestGetForProblem()
        {
            List<ProblemSetData> sets = setDao.GetForProblem(new ProblemData(2));

            Assert.IsTrue(sets.Contains(new ProblemSetData(1)));
            Assert.IsTrue(sets.Contains(new ProblemSetData(2)));
        }

        [TestMethod]
        public void TestAddPrereq()
        {
            ProblemSetData set1 = new ProblemSetData(2);
            ProblemSetData set2 = new ProblemSetData(1);
            set2.PrereqCount = 5;

            //This prereq should already exist in the database
            Assert.IsFalse(setDao.AddPrereq(set1, set2));
        }

        [TestMethod]
        public void TestGetPrereqs()
        {
            List<ProblemSetData> prereqs = setDao.GetPrereqs(new ProblemSetData(2));
            ProblemSetData set = new ProblemSetData(1);

            Assert.IsTrue(prereqs.Contains(set));
            Assert.AreEqual(5, prereqs.Find(ps => ps.Id == 1).PrereqCount);

            set = new ProblemSetData(2);

            Assert.IsFalse(prereqs.Contains(set));
        }
    }
}
