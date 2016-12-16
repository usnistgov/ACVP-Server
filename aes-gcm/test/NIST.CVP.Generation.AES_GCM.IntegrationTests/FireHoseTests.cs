using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Testing.Abstractions;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture]
    public class FireHoseTests
    {
        private string _testFilePath = @"C:\ACAVPTestFiles\AES_GCM";

        [OneTimeSetUp]
        public void SetUp()
        {
           // ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
           // configurationBuilder.AddJsonFile("config.json");
            
           // var config = configurationBuilder.Build();

           //_testFilePath= config["TestFileDirectory"];

        }
        
        [Ignore("For integration -- coming soon")]
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testFilePath))

            {
                Assert.Fail("Test File Directory does not exist");
            }
            var testDir = new DirectoryInfo(_testFilePath);
            var parser = new LegacyResponseFileParser();
            var validator = new Validator(
                new DynamicParser(),  
                new ResultValidator(), 
                new TestCaseGeneratorFactory(
                    new Random800_90(), 
                    new AES_GCM(
                        new AES_GCMInternals(
                            new RijndaelFactory(
                                new RijndaelInternals()
                            )
                        ),
                        new RijndaelFactory(
                            new RijndaelInternals()
                        )
                    )
                )
            );
            foreach (var testFilePath in testDir.EnumerateFiles("*encrypt*.*"))
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }
                var testVector = parseResult.ParsedObject;
               // var validationResult = validator.ValidateWorker(testVector testVector.ResultProjection,);
               // Assert.AreEqual("passed", validationResult.Disposition);
            }
        }
       
    }
}
