﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Problem
{
    public class ProblemData : DataObject
    {
        private string name;
        private string description;
        private List<TestCase> testCases;

        public ProblemData(int id) : base(id) { }

        public string Name
        {
            get
            {
                if (String.IsNullOrWhiteSpace(name))
                    return "Problem " + Id;
                return name;
            }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public List<TestCase> TestCases
        {
            get
            {
                if (testCases == null)
                    testCases = new List<TestCase>();
                return testCases; 
            }
            set { testCases = value; }
        }

        public override string ToString()
        {
            return String.Format("Problem Set: Id={0}, Name={1}", Id, Name);
        }
    }
}
