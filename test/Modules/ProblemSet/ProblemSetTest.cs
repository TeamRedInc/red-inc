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
            List<ProblemSetData> sets = setModel.GetForStudent(new UserData(7), new ClassData(2));

            Assert.IsFalse(sets.Find(ps => ps.Id == 1).Locked);
            Assert.IsTrue(sets.Find(ps => ps.Id == 2).Locked);
            Assert.IsFalse(sets.Find(ps => ps.Id == 3).Locked);
            Assert.IsFalse(sets.Contains(new ProblemSetData(4)));
        }

        [TestMethod]
        public void TestPrereqs()
        {
            ProblemSetData parent = new ProblemSetData(3);
            List<ProblemSetData> original = setModel.GetPrereqs(parent);

            //Test remove prereqs
            Assert.IsTrue(setModel.UpdatePrereqs(parent, new List<ProblemSetData>()));
            Assert.AreEqual(0, setModel.GetPrereqs(parent).Count);

            //Test add prereqs
            Assert.IsTrue(setModel.UpdatePrereqs(parent, original));
            CollectionAssert.AreEqual(original, setModel.GetPrereqs(parent));

            //Test add bad data
            List<ProblemSetData> badList = new List<ProblemSetData>(original);
            badList.Add(new ProblemSetData { Id = original[0].Id, PrereqCount = original[0].PrereqCount }); //Can't have duplicate prereq entries
            badList.Add(new ProblemSetData { Id = 3, PrereqCount = 1 }); //Set can't require itself
            Assert.IsTrue(setModel.UpdatePrereqs(parent, badList));
            CollectionAssert.AreEqual(original, setModel.GetPrereqs(parent));

            //Test update prereqs
            foreach (ProblemSetData p in original)
                p.PrereqCount += 1;
            Assert.IsTrue(setModel.UpdatePrereqs(parent, original));
            foreach (ProblemSetData p in setModel.GetPrereqs(parent))
            {
                ProblemSetData op = original.Find(s => s.Id == p.Id);
                Assert.AreEqual(op.PrereqCount, p.PrereqCount);
            }

            foreach (ProblemSetData p in original)
                p.PrereqCount -= 1;
            Assert.IsTrue(setModel.UpdatePrereqs(parent, original));
            foreach (ProblemSetData p in setModel.GetPrereqs(parent))
            {
                ProblemSetData op = original.Find(s => s.Id == p.Id);
                Assert.AreEqual(op.PrereqCount, p.PrereqCount);
            }
        }
    }
}
