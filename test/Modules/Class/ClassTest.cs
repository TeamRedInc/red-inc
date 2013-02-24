using core.Modules.Class;
using core.Modules.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.Class
{
    [TestClass]
    public class ClassTest
    {
        private ClassDao classDao = new ClassDao();

        [TestMethod]
        public void TestAdd()
        {
            ClassData cls = new ClassData(0);
            cls.Name = "CS 4911 Design Capstone Project";
            cls.Instructor = new UserData(1);

            //This class should already exist in the database
            Assert.IsFalse(classDao.Add(cls));
        }

        [TestMethod]
        public void TestGet()
        {
            ClassData cls = classDao.GetById(0);

            Assert.IsNull(cls);

            cls = classDao.GetById(1);

            Assert.AreEqual(cls.Name, "CS 4911 Design Capstone Project");

            List<ClassData> classes = classDao.GetAll();

            cls = classDao.GetById(classes[0].Id);

            Assert.AreEqual(classes[0], cls);
        }

        [TestMethod]
        public void TestGetForUser()
        {
            ClassData cls1 = classDao.GetById(1);
            ClassData cls2 = classDao.GetById(2);

            List<ClassData> classes = classDao.GetStudentClasses(new UserData(1));

            Assert.IsFalse(classes.Contains(cls1));
            Assert.IsTrue(classes.Contains(cls2));

            classes = classDao.GetInstructorClasses(new UserData(1));

            Assert.IsTrue(classes.Contains(cls1));
            Assert.IsFalse(classes.Contains(cls2));
        }
    }
}
