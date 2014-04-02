using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Uglify2.Bundler
{
    public class Uglify2BundleTransformMap : Uglify2Common, IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var result = Minify(context, response);

            response.Content = result.map;
            response.ContentType = "application/json";
            response.Cacheability = System.Web.HttpCacheability.Public;
        }
    }
}
