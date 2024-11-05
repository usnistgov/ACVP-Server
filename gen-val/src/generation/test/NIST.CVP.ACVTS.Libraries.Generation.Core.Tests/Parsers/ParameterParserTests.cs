using System.IO;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Parsers
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

            Assert.That(result.ParsedObject.Algorithm, Is.EqualTo(parameters.Algorithm), nameof(parameters.Algorithm));
            Assert.That(result.ParsedObject.Mode, Is.EqualTo(parameters.Mode), nameof(parameters.Mode));
            Assert.That(result.ParsedObject.Revision, Is.EqualTo(parameters.Revision), nameof(parameters.Revision));
            Assert.That(result.ParsedObject.IsSample, Is.EqualTo(parameters.IsSample), nameof(parameters.IsSample));
            Assert.That(result.ParsedObject.VectorSetId, Is.EqualTo(parameters.VectorSetId), nameof(parameters.VectorSetId));
        }
    }
}
