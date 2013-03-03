using System;
using System.Collections.Generic;
using System.IO;
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
        private MemoryStream _stdout;
        private Encoding _encoding = Encoding.UTF8;

        public PyInterpretUtility()
        {
            _engine = Python.CreateEngine();
            _stdout = new MemoryStream();
            _engine.Runtime.IO.SetOutput(_stdout, Encoding.ASCII);
            _scope = null;
        }

        public string Interpret(string code)
        {
            _scope = _engine.CreateScope();
            string output = "";
            //_scope.SetVariable("output",output);
            
            _engine.Execute(code, _scope);
            //return _scope.GetVariable("output");
            StreamReader reader = new StreamReader(_stdout);
            output = _encoding.GetString(_stdout.ToArray());
            return output;
        }

        public string FutureInterpret(string code)
        {
            _scope = _engine.CreateScope();
            string output = "";
            //_scope.SetVariable("output",output);

            var paths = _engine.GetSearchPaths();
            string dir = System.IO.Path.GetFullPath("../../../Core/Lib");
            // Use Server.MapPath("../../../Core/Lib") for Azure compatibility
            paths.Add(dir);
            _engine.SetSearchPaths(paths);

            _engine.Execute(code, _scope);
            //return _scope.GetVariable("output");
            StreamReader reader = new StreamReader(_stdout);
            output = _encoding.GetString(_stdout.ToArray());
            return output;
        }
    }
}
