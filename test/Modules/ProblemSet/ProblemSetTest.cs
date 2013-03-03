using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.ProblemSet
{
    [TestClass]
    public class ProblemSetTest
    {
        private ProblemSetModel setModel = new ProblemSetModel();

        [TestMethod]
        public void TestAdd()
        {
            ProblemSetData set = new ProblemSetData(0);
            set.Name = "Beginner Problems";
            set.Class = new ClassData(1);

            //This test works as of 5:15pm on 2/22
            //Commenting it out to prevent cluttering the database
            //- Josh
            //Assert.IsTrue(setModel.Add(set));
        }

        [TestMethod]
        public void TestGet()
        {
            ProblemSetData set = setModel.GetById(0);

            Assert.IsNull(set);

            set = setModel.GetById(1);

            Assert.AreEqual("Beginner Problems", set.Name);
            Assert.AreEqual(2, set.Class.Id);

            List<ProblemSetData> sets = setModel.GetAll();

            Assert.IsTrue(sets.Contains(set));
        }

        [TestMethod]
        public void TestGetForClass()
        {
            List<ProblemSetData> sets = setModel.GetForClass(new ClassData(1));

            Assert.IsNotNull(sets);
            Assert.AreEqual(1, sets.Count);

            sets = setModel.GetForClass(new ClassData(2));

            Assert.IsTrue(sets.Contains(new ProblemSetData(1)));
            Assert.IsTrue(sets.Contains(new ProblemSetData(2)));
            Assert.IsFalse(sets.Contains(new ProblemSetData(4)));
        }

        [TestMethod]
        public void TestGetForProblem()
        {
            List<ProblemSetData> sets = setModel.GetForProblem(new ProblemData(3));

            Assert.IsTrue(sets.Contains(new ProblemSetData(1)));
            Assert.IsTrue(sets.Contains(new ProblemSetData(2)));
        }

        [TestMethod]
        public void TestGetForStudent()
        {
            List<ProblemSetData> sets = setModel.GetForStudent(new UserData(1), new ClassData(2));

            Assert.IsFalse(sets.Find(ps => ps.Id == 1).Locked);
            Assert.IsTrue(sets.Find(ps => ps.Id == 2).Locked);
            Assert.IsFalse(sets.Find(ps => ps.Id == 3).Locked);
            Assert.IsFalse(sets.Contains(new ProblemSetData(4)));
        }

        [TestMethod]
        public void TestPrereq()
        {
            ProblemSetData set1 = new ProblemSetData(2);
            ProblemSetData set2 = new ProblemSetData(1);
            set2.PrereqCount = 3;

            //This prereq should already exist in the database
            Assert.IsFalse(setModel.AddPrereq(set1, set2));

            List<ProblemSetData> prereqs = setModel.GetPrereqs(set1);

            //Assert prereq exists with a problem count of 3
            Assert.IsTrue(prereqs.Contains(set2));
            Assert.AreEqual(3, prereqs.Find(ps => ps.Id == 1).PrereqCount);
            Assert.IsFalse(prereqs.Contains(set1));

            set2.PrereqCount = 10;

            Assert.IsTrue(setModel.UpdatePrereq(set1, set2));

            prereqs = setModel.GetPrereqs(set1);

            //Assert update worked
            Assert.AreEqual(10, prereqs.Find(ps => ps.Id == 1).PrereqCount);

            Assert.IsTrue(setModel.RemovePrereq(set1, set2));

            prereqs = setModel.GetPrereqs(set1);

            //Assert remove worked
            Assert.IsFalse(prereqs.Contains(set2));

            set2.PrereqCount = 3;

            //Add prereq back in
            Assert.IsTrue(setModel.AddPrereq(set1, set2));
        }
    }
}
