using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Uglify2.Bundler
{
    public class Uglify2BundleTransform : Uglify2Common, IBundleTransform
    {
        private string _sourceMapPath;
        public Uglify2BundleTransform(string sourceMapPath)
        {
            _sourceMapPath = sourceMapPath;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            var result = Minify(context, response);

            response.Content = result.code + "\r\n//# sourceMappingURL=" + _sourceMapPath;
            response.ContentType = "text/javascript";
            response.Cacheability = System.Web.HttpCacheability.Public;
        }
    }
}
