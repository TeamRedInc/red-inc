using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.Class;
using core.Modules.User;
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
        public void TestAddStudent()
        {
            ClassData cls = new ClassData(1);
            UserData user = new UserData(7);

            //This user should already be a student in the class
            Assert.IsFalse(classDao.AddStudent(user, cls));
        }

        [TestMethod]
        public void TestGet()
        {
            ClassData cls = classDao.GetById(1);

            Assert.AreEqual(cls.Name, "CS 4911 Design Capstone Project");

            List<ClassData> classes = classDao.GetAll();

            cls = classDao.GetById(classes[0].Id);

            Assert.AreEqual(classes[0], cls);
        }
    }
}
