using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;

namespace Uglify2.Compiler
{
    public class V8 : CompilerCore
    {
        private V8ScriptEngine engine;

        public V8()
        {
#if DEBUG
            // Javascript Debugging can be done with ClearScript using Eclipse
            // See https://clearscript.codeplex.com/sourcecontrol/latest#ReadMe.txt for details
            engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, 9222);
#else
			engine = new V8ScriptEngine();
#endif

            _compilerLibraryResourceNames = new string[]
			{
                // The "Scripts." at the beginning of the name indicates it can be found in
                // the Scripts folder of this project.  Each of these files must be included
                // in the project with properties of:
                //     Build Action: Embedded Resource
                //     Copy to Output Directory: Do Not Copy
				"Scripts.util.js",
				"Scripts.array-set.js",
				"Scripts.base64.js",
				"Scripts.base64-vlq.js",
				"Scripts.source-map-generator.js",
				"Scripts.uglify2.standalone.js",
				"Scripts.minify.js"
			};
        }

        protected override string CallMinify(string arguments)
        {
            return (string)engine.Script.minify(arguments);
        }

        protected override void LoadScript(string scriptName, string scriptContents)
        {
            engine.Execute(scriptName, scriptContents);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (engine != null)
                {
                    engine.Dispose();
                    engine = null;
                }
            }
        }
    }
}
