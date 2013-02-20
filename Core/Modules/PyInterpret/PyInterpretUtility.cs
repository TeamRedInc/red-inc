using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace core.Modules.PyInterpret
{
    public class PyInterpretUtility
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;

        public PyInterpretUtility()
        {
            _engine = Python.CreateEngine();
            _scope = null;
        }

        public string PyInterpret(string code)
        {
            _scope = _engine.CreateScope();
            string output = "";
            _scope.SetVariable("output",output);
            _engine.Execute(code, _scope);
            return _scope.GetVariable("output");
        }
    }
}
