using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Uglify2.Bundler
{
    public static class BundleExtensions
    {
        public static void Add(this BundleCollection bundles, ScriptBundleWithMap scriptMapBundle)
        {
            scriptMapBundle.AddToBundleCollection(bundles);
        }
    }
}
