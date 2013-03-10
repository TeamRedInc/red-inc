using core.Modules.Class;
using core.Modules.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace core.Tests.Modules.User
{
    [TestClass]
    public class UserTest
    {
        private UserModel userModel = new UserModel();

        [TestMethod]
        public void TestGet()
        {
            UserData user = userModel.GetById(0);

            Assert.IsNull(user);

            user = userModel.GetById(7);

            Assert.AreEqual(user.Email, "jwien3@mail.gatech.edu");

            List<UserData> users = userModel.GetAll();

            Assert.IsTrue(users.Contains(user));
        }

        [TestMethod]
        public void TestAddStudent()
        {
            ClassData cls = new ClassData(1);
            UserData user = new UserData(8);

            //This user should already be a student in the class
            Assert.IsFalse(userModel.AddStudent(user, cls));
        }

        [TestMethod]
        public void TestGetStudents()
        {
            List<UserData> students = userModel.GetStudents(new ClassData(1));
            UserData student = userModel.GetById(8);

            Assert.IsTrue(students.Contains(student));

            student = userModel.GetById(7);

            Assert.IsFalse(students.Contains(student));
        }
    }
}
