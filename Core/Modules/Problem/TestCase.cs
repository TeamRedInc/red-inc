using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Problem
{
    public class TestCase : DataObject
    {
        private string input;
        private string output;
        private bool isSample;

        public TestCase(int id) : base(id) { }

        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        public string Output
        {
            get { return output; }
            set { output = value; }
        }

        public bool IsSample
        {
            get { return isSample; }
            set { isSample = value; }
        }

        public override string ToString()
        {
            return String.Format("Test Case: Id={0}", Id);
        }
    }
}
