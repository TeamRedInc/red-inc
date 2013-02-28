using core.Modules.Class;
using core.Modules.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.Class
{
    [TestClass]
    public class ClassTest
    {
        private ClassModel classModel = new ClassModel();

        [TestMethod]
        public void TestAdd()
        {
            ClassData cls = new ClassData(0);
            cls.Name = "CS 4911 Design Capstone Project";
            cls.Instructor = new UserData(1);

            //This class should already exist in the database
            Assert.IsFalse(classModel.Add(cls));

            cls.Name = "Something new";
            cls.Instructor = new UserData(0);

            //This should fail because the Instructor Id is invalid
            Assert.IsFalse(classModel.Add(cls));
        }

        [TestMethod]
        public void TestGet()
        {
            ClassData cls = classModel.GetById(0);

            Assert.IsNull(cls);

            cls = classModel.GetById(1);

            Assert.AreEqual(cls.Name, "CS 4911 Design Capstone Project");

            List<ClassData> classes = classModel.GetAll();

            cls = classModel.GetById(classes[0].Id);

            Assert.AreEqual(classes[0], cls);
        }

        [TestMethod]
        public void TestGetForUser()
        {
            ClassData cls1 = classModel.GetById(1);
            ClassData cls2 = classModel.GetById(2);

            List<ClassData> classes = classModel.GetStudentClasses(new UserData(1));

            Assert.IsFalse(classes.Contains(cls1));
            Assert.IsTrue(classes.Contains(cls2));

            classes = classModel.GetInstructorClasses(new UserData(1));

            Assert.IsTrue(classes.Contains(cls1));
            Assert.IsFalse(classes.Contains(cls2));
        }
    }
}
