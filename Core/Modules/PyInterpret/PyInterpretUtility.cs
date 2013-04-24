using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

        private const int MAX_RECURSION_DEPTH = 100;
        private const int MAX_OP_DURATION_IN_MS = 5000;

        public PyInterpretUtility()
        {
            _engine = Python.CreateEngine();
            _stdout = new MemoryStream();
            _engine.Runtime.IO.SetOutput(_stdout, Encoding.ASCII);
            _scope = null;
        }

        public string Test(string studentCode, string professorCode)
        {
            var studentCodeArray = studentCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < studentCodeArray.Length; i++)
            {
                studentCodeArray[i] = studentCodeArray[i].Replace("\t", Indent(4));
                //studentCodeArray[i] = Indent(1) + studentCodeArray[i];
            }
            var profCodeArray = professorCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < profCodeArray.Length; i++)
            {
                profCodeArray[i] = profCodeArray[i].Replace("\t", Indent(4));
                profCodeArray[i] = Indent(1) + profCodeArray[i];
            }
            var code = String.Join("\r\n", studentCodeArray) + @"

def professorFunction():
" + String.Join("\r\n", profCodeArray) + @"

professorFunction()";

            return Execute(code);
        }

        public string Execute(string code)
        {
            // Regex for security - this probably isn't enough for the long run
            Match match = Regex.Match(code + " ", @"[\.\s()]*((os)|(exec)|(eval))[\.\s()]*");
            if (match.Success)
            {
                return "Code not permitted: " + match.Groups[1].Value;
            }

            _scope = _engine.CreateScope();
            string output = "";

            var paths = _engine.GetSearchPaths();
            //string dir = System.IO.Path.GetFullPath("../../../Core/Lib");
            //string dir = Path.GetFullPath(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), "..\\Core\\Lib"));
            string dir = HttpContext.Current.Server.MapPath("~/Libs/python");
            
            paths.Add(dir);
            _engine.SetSearchPaths(paths);

            code = @"from __future__ import division
from __future__ import print_function
from __future__ import unicode_literals

import sys

sys.setrecursionlimit("+MAX_RECURSION_DEPTH+@")

" + code;

            try
            {
                output = CallWithTimeout(Run, code, _scope, MAX_OP_DURATION_IN_MS);
                return output;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string Run(string code, ScriptScope scope)
        {
            try
            {
                _engine.Execute(code, scope);
                //return _scope.GetVariable("output");
                StreamReader reader = new StreamReader(_stdout);
                var output = _encoding.GetString(_stdout.ToArray());
                return output;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string CallWithTimeout(Func<string, ScriptScope, string> action, string code, ScriptScope scope, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Func<string, ScriptScope, string> wrappedAction = (incode, inscope) =>
            {
                threadToKill = Thread.CurrentThread;
                return action(incode, inscope);
            };

            IAsyncResult result = wrappedAction.BeginInvoke(code, scope, null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                return wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        private string Indent(int count)
        {
            return "".PadLeft(count*4);
        }
    }
}
