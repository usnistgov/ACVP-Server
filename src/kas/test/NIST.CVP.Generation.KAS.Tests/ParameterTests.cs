using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture]
    public class ParameterTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetTestFolder("", "parameters");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }

        private static object[] _testData = new object[]
        {
            new object[]
            {
                new Parameters()
                {
                    Algorithm = "KAS-FFC",
                    Function = new string[] {"function", "function2"},
                    Scheme = new Schemes()
                    {
                        FfcDhEphem = new FfcDhEphem()
                        {
                            KasRole = new string[]{ "initiator" },
                            KdfNoKc = new KdfNoKc()
                            {
                                KdfOption = new KdfOptions()
                                {
                                    Concatenation = "uPartyInfo||vPartyInfo"
                                },
                                ParameterSet = new ParameterSets()
                                {
                                    Fc = new Fc()
                                    {
                                        HashAlg = new string[] { "SHA2-512" },
                                        MacOption = new MacOptions()
                                        {
                                            HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                            {
                                                KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                                MacLen = 128
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        [Test]
        [TestCaseSource(nameof(_testData))]

        public void ShouldSerializeAndDeserializeIntoSameObject(Parameters parameters)
        {
            var jsonParameterFile = CreateRegistration(_testPath, parameters);

            ParameterParser<Parameters> parser = new ParameterParser<Parameters>();
            var result = parser.Parse(jsonParameterFile);

            var parsedParams = result.ParsedObject;

            Assume.That(result.Success);
            Assert.IsTrue(parameters.JsonCompare(parsedParams));
        }

        private string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringConverter(),
                    new DomainConverter()
                },
                Formatting = Formatting.Indented
            });
            string fileName = $"{targetFolder}\\{Guid.NewGuid()}.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}
