using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using Uglify2.Compiler;

namespace Uglify2.Bundler
{
    public class Uglify2Common
    {
        protected CompileResult Minify(BundleContext context, BundleResponse response)
        {
            //using (var fixture = new SASSandJavascript(new InstanceProvider<IJavaScriptRuntime>(() => new IEJavaScriptRuntime())))
            using (var fixture = new V8())
            {
                var fileContents = new Dictionary<string, string>();

                foreach (var assetFile in response.Files)
                {
                    var virtualPath = assetFile.VirtualFile.VirtualPath;
                    var absolutePath = VirtualPathUtility.ToAbsolute(virtualPath);
                    var path = context.HttpContext.Server.MapPath(assetFile.VirtualFile.VirtualPath.Replace("/", "\\"));
                    var fileContent = File.ReadAllText(path);
                    fileContents.Add(absolutePath, fileContent);
                }

                var sourceMapUrl = VirtualPathUtility.ToAbsolute(context.BundleVirtualPath) + ".map";
                return fixture.Compile(fileContents.Keys.ToArray(), fileContents.Values.ToArray(), sourceMapUrl);
            }
        }
    }
}
