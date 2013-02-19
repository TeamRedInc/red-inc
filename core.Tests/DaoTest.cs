using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core;
using core.Modules.User;

namespace core.Tests
{
    [TestClass]
    public class DaoTest
    {
        [TestMethod]
        public void CoreTest()
        {
            //Core c = new Core();
        }

        [TestMethod]
        public void UserTest()
        {
            UserData u = new UserData();
            u.Id = 2;
            Assert.AreEqual(2, u.Id);

            UserData u2 = new UserData();
            u2.Id = 2;
            Assert.IsTrue(u == u2);
        }
    }
}
