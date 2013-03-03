using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.PyInterpret;

namespace core.Tests.Modules.PyInterpret
{
    [TestClass]
    public class PyInterpretUtilityTest
    {
     
        [TestMethod]
        public void FutureTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.FutureInterpret(@"from __future__ import division
x = 3/2
print x");
            Assert.AreEqual(output, "1.5\r\n");
        }

        [TestMethod]
        public void PrintTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Interpret("print \"TESTVALUE\"");
            Assert.AreEqual(output, "TESTVALUE\r\n");
        }

        [TestMethod]
        public void ReturnTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Interpret(@"def myFunction(number):
    print(number)

def professorFunction(num):
    myFunction(num)

professorFunction(3)    "
);
            Assert.AreEqual(output, "3\r\n");
        }

        [TestMethod]
        public void EncapsulatedTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Interpret(@"def encap():
    def myFunction(number):
        print(number)

    def professorFunction():
        myFunction(3)

    professorFunction()

encap()"
);
            Assert.AreEqual(output, "3\r\n");
        }
    }
}
