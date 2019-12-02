using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class ParameterParserTests
    {
        private ParameterParser<FakeParameters> _subject = new ParameterParser<FakeParameters>();
        
        public void ShouldParseParametersCorrectly()
        {
            var parameters = new FakeParameters()
            {
                Algorithm = "test test",
                Mode = "also test",
                Revision = "Special Publication",
                IsSample = true,
                VectorSetId = 42,
            };

            var parametersJson = JsonConvert.SerializeObject(parameters);

            var result = _subject.Parse(parametersJson);
            
            Assert.AreEqual(parameters.Algorithm, result.ParsedObject.Algorithm, nameof(parameters.Algorithm));
            Assert.AreEqual(parameters.Mode, result.ParsedObject.Mode, nameof(parameters.Mode));
            Assert.AreEqual(parameters.Revision, result.ParsedObject.Revision, nameof(parameters.Revision));
            Assert.AreEqual(parameters.IsSample, result.ParsedObject.IsSample, nameof(parameters.IsSample));
            Assert.AreEqual(parameters.VectorSetId, result.ParsedObject.VectorSetId, nameof(parameters.VectorSetId));
        }
    }
}
