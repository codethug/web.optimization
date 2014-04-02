using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uglify2.Compiler;

namespace Uglify2.Tests
{
    [TestClass]
    public class Uglify2Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] fileUrls = new string[]{"myFolder\\myFile.js"};
            string[] fileContents = new string[] { "function myFunc() { var myVariable = 'foo';\nconsole.log(myVariable); }" };
            string sourceMapUrl = "http://myserver.com/myFile.js.map";

            var compiler = new V8();
            var result = compiler.Compile(fileUrls, fileContents, sourceMapUrl);

            string expectedMinifiedFile = "function myFunc(){var o=\"foo\";console.log(o)}";
            string expectedSourceMap = "{\"version\":3,\"file\":\"http://myserver.com/myFile.js.map\",\"sources\":[\"myFolder\\\\myFile.js\"],\"names\":[\"myFunc\",\"myVariable\",\"console\",\"log\"],\"mappings\":\"AAAA,QAASA,UAAW,GAAIC,GAAa,KACrCC,SAAQC,IAAIF\"}";

            Assert.AreEqual(result.code, expectedMinifiedFile);
            Assert.AreEqual(result.map, expectedSourceMap);
        }
    }
}
