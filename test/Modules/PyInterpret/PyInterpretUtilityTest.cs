using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.PyInterpret;

namespace core.Tests.Modules.PyInterpret
{
    [TestClass]
    public class PyInterpretUtilityTest
    {
        [TestMethod]
        public void TestPyInterpret()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.PyInterpret("output = \"TESTVALUE\"");
            Assert.AreEqual(output, "TESTVALUE");
        }
    }
}
