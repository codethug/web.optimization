using System;
using System.IO;
using Newtonsoft.Json;

namespace Uglify2.Compiler
{
    public abstract class CompilerCore : IDisposable
    {
        private object _lock = new object();
        private bool _initialized = false;
        protected string[] _compilerLibraryResourceNames;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileUrls"></param>
        /// <param name="fileContent"></param>
        /// <param name="sourceMapUrl">The URL that the minified javascript will point to so that the
        /// browser can find the source maps.  If omitted, source maps are not generated.</param>
        /// <param name="inputSourceMapContent">If another compiler, such as coffeescript, already 
        /// generated a source map and is handing off a precompiled javascript file to us,
        /// set InputSourceMapContent to the contents of the source map file
        /// that coffeescript generated so that we can map back to the 
        /// original coffeescript</param>
        /// <returns></returns>
        public CompileResult Compile(string[] fileUrls, string[] fileContents,
            string sourceMapUrl = null, string inputSourceMapContent = null)
        {
            if (fileUrls == null || fileContents == null || fileUrls.Length != fileContents.Length || fileUrls.Length == 0)
                throw new ArgumentException("Invalid sources");

            var options = new
            {
                outSourceMap = sourceMapUrl,
                inSourceMap = inputSourceMapContent
            };

            var arguments = JsonConvert.SerializeObject(new { fileUrls, fileContents, options });

            lock (_lock)
            {
                Initialize();
                var result = CallMinify(arguments);
                return JsonConvert.DeserializeObject<CompileResult>(result);
            }

        }

        private void Initialize()
        {
            if (!_initialized)
            {
                var foo = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

                var scope = this.GetType();

                foreach (var resourceName in _compilerLibraryResourceNames)
                {
                    var abbreviatedName = resourceName.StartsWith("Scripts.") ? resourceName.Substring(8) : resourceName;
                    var resource = "Uglify2." + resourceName;

                    string resourceString = null;
                    using (var resourceStream = scope.Assembly.GetManifestResourceStream(resource))
                    {
                        using (var reader = new StreamReader(resourceStream))
                        {
                            resourceString = reader.ReadToEnd();
                        }
                    }

                    LoadScript(abbreviatedName, resourceString);
                }
                _initialized = true;
            }
        }

        public abstract void Dispose();

        protected abstract void LoadScript(string scriptName, string scriptContents);
        protected abstract string CallMinify(string arguments);
    }
}
