using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace Uglify2.Bundler
{
    public class ScriptBundleWithMap
    {
        private readonly Bundle _jsBundle;
        private readonly Bundle _mapBundle;

        public ScriptBundleWithMap(string virtualPath)
        {
            _jsBundle = new Bundle(virtualPath, new Uglify2BundleTransform(VirtualPathUtility.ToAbsolute(virtualPath + "__map")));
            _mapBundle = new Bundle(virtualPath + "__map", new Uglify2BundleTransformMap());
        }

        public ScriptBundleWithMap(string virtualPath, string cdnPath)
        {
            _jsBundle = new Bundle(virtualPath, cdnPath, new Uglify2BundleTransform(VirtualPathUtility.ToAbsolute(virtualPath + "__map")));
            _mapBundle = new Bundle(virtualPath + "__map", cdnPath + "__map", new Uglify2BundleTransformMap());
        }

        public ScriptBundleWithMap Include(params string[] virtualPaths)
        {
            _jsBundle.Include(virtualPaths);
            _mapBundle.Include(virtualPaths);
            return this;
        }

        internal void AddToBundleCollection(BundleCollection bundles)
        {
            bundles.Add(_jsBundle);
            bundles.Add(_mapBundle);
        }
    }
}
