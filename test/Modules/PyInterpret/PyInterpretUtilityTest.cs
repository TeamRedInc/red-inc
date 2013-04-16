using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using core.Modules.PyInterpret;

namespace core.Tests.Modules.PyInterpret
{
    [TestClass]
    public class PyInterpretUtilityTest
    {
        [TestMethod]
        public void UnallowedCodeTest1()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"import os";
            var output = util.Execute(studentCode);
            Assert.AreEqual(output, "Code not permitted: os");
        }

        [TestMethod]
        public void UnallowedCodeTest2()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"from os import";
            var output = util.Execute(studentCode);
            Assert.AreEqual(output, "Code not permitted: os");
        }

        [TestMethod]
        public void UnallowedCodeTest3()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"import os.path";
            var output = util.Execute(studentCode);
            Assert.AreEqual(output, "Code not permitted: os");
        }

        [TestMethod]
        public void UnallowedCodeTest4()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"exec etc";
            var output = util.Execute(studentCode);
            Assert.AreEqual(output, "Code not permitted: exec");
        }

        [TestMethod]
        public void UnallowedCodeTest5()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"eval(etc)";
            var output = util.Execute(studentCode);
            Assert.AreEqual(output, "Code not permitted: eval");
        }

        [TestMethod]
        public void ProfessorCodeTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var studentCode = @"def myFunc():
	return 1";
            var profCode = @"output = myFunc()
if(output == 1):
	print(""Correct!"")
else:
	print(""You fail!"")";
            var output = util.Test(studentCode, profCode);
            Assert.AreEqual(output, "Correct!\r\n");
        }

        [TestMethod]
        public void FutureTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Execute(@"x = 3/2
print(x)");
            Assert.AreEqual(output, "1.5\r\n");
        }

        [TestMethod]
        public void PrintTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Execute("print(\"TESTVALUE\")");
            Assert.AreEqual(output, "TESTVALUE\r\n");
        }

        [TestMethod]
        public void ReturnTest()
        {
            PyInterpretUtility util = new PyInterpretUtility();
            var output = util.Execute(@"def myFunction(number):
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
            var output = util.Execute(@"def encap():
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
