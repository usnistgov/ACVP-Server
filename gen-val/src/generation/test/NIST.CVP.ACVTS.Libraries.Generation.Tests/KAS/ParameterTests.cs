using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS
{
    [TestFixture]
    public class ParameterTests
    {
        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\KAS");
            if (!Directory.Exists(_testPath))
            {
                Directory.CreateDirectory(_testPath);
            }
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }

        string _testPath;

        private static IEnumerable<object[]> _testData = new List<object[]>
        {
            new object[]
            {
                "test1",
                new Parameters
                {
                    Algorithm = "KAS-FFC",
                    Function = new[] {"function", "function2"},
                    Scheme = new Schemes
                    {
                        FfcDhEphem = new FfcDhEphem
                        {
                            KasRole = new[]{ "initiator" },
                            KdfNoKc = new KdfNoKc
                            {
                                KdfOption = new KdfOptions
                                {
                                    Concatenation = "uPartyInfo||vPartyInfo"
                                },
                                ParameterSet = new ParameterSets
                                {
                                    Fc = new Fc
                                    {
                                        HashAlg = new[] { "SHA2-512" },
                                        MacOption = new MacOptions
                                        {
                                            HmacSha2_D224 = new MacOptionHmacSha2_d224
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
        public void ShouldSerializeAndDeserializeIntoSameObject(string testLabel, Parameters parameters)
        {
            var jsonParameterFile = CreateRegistration(_testPath, parameters);

            ParameterParser<Parameters> parser = new ParameterParser<Parameters>();
            var result = parser.Parse(File.ReadAllText(jsonParameterFile));

            var parsedParams = result.ParsedObject;

            Assert.That(result.Success);
            Assert.That(parameters.JsonCompare(parsedParams), Is.True);
        }

        private string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BitstringConverter(),
                    new DomainConverter()
                },
                Formatting = Formatting.Indented
            });

            string fileName = Path.Combine(targetFolder, $"{Guid.NewGuid()}.json");
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}
