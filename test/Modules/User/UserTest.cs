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
        public void TestAdd()
        {
            UserData user = new UserData(0);
            user.Email = "jwien3@mail.gatech.edu";
            user.PasswordHash = "pass123";

            //This user should already exist in the database
            Assert.IsFalse(userModel.Add(user));
        }

        [TestMethod]
        public void TestLogin()
        {
            UserData user = new UserData(0);
            user.Email = "jwien3@mail.gatech.edu";
            user.PasswordHash = "pass123";

            UserData expected = userModel.GetById(1);

            //Successful login
            Assert.AreEqual(expected, userModel.Login(user));

            user.Email = "garbage@hotmail.com";

            //Bad email
            Assert.IsNull(userModel.Login(user));

            user.Email = "jwien3@mail.gatech.edu";
            user.PasswordHash = "badpass";

            //Bad password
            Assert.IsNull(userModel.Login(user));
        }

        [TestMethod]
        public void TestGet()
        {
            UserData user = userModel.GetById(0);

            Assert.IsNull(user);

            user = userModel.GetById(1);

            Assert.AreEqual(user.Email, "jwien3@mail.gatech.edu");

            List<UserData> users = userModel.GetAll();

            user = userModel.GetById(users[0].Id);

            Assert.AreEqual(users[0], user);
        }

        [TestMethod]
        public void TestAddStudent()
        {
            ClassData cls = new ClassData(1);
            UserData user = new UserData(7);

            //This user should already be a student in the class
            Assert.IsFalse(userModel.AddStudent(user, cls));
        }

        [TestMethod]
        public void TestGetStudents()
        {
            List<UserData> students = userModel.GetStudents(new ClassData(1));
            UserData student = userModel.GetById(2);

            Assert.IsTrue(students.Contains(student));

            student = userModel.GetById(1);

            Assert.IsFalse(students.Contains(student));
        }
    }
}
