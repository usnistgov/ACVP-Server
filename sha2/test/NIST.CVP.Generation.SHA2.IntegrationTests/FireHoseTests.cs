using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.IntegrationTests
{
    [TestFixture]
    public class FireHoseTests
    {
        private string _testPath;

        private SHA _sha;
        //private SHA2_MCT _shaMCT;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles");
            _sha = new SHA();
            //_shaMCT = new SHA2_MCT(_sha);
        }

        //[Test]
        public void ShouldParseAndRunCAVSFiles()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist.");
            }

            
        }
    }
}
