web.optimization
================

Implementations of System.Web.Optimization.IBundleTransformer to enhance the Bundling and Minification support in .Net

Uses Chrome V8 engine as packaged in ClearScript.  This allows us to run a javascript engine in .Net, which allows us to run Uglify2 to minify assets.  Source code for ClearScript can be found at https://clearscript.codeplex.com, and ClearScript is licensed under the MS-PL (http://opensource.org/licenses/MS-PL).

When using outside ASP.Net, notice the post-build steps that are part of the Uglify2.Test project.  These ensure that the ClearScript native assemblies are copied to the proper folder for testing.

When deploying as a web application, do not include the post-build steps that are part of the Uglify2.Test project.  Instead, follow the instructions at https://clearscript.codeplex.com/discussions/458779 for including the native ClearScript assemblies.

The javascript libraries that are used to minify resources, such as uglify2, need to be included in the Uglify project as embedded resources.  This is done by right-clicking on the .js file, properties, then setting the Build Action to Embedded resource.
