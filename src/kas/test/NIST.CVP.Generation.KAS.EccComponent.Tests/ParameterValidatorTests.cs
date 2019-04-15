using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private const string _INVALID = "invalid";
        private readonly ParameterValidator _subject = new ParameterValidator();
        private ParameterBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new ParameterBuilder();
        }

        [Test]
        public void ShouldValidateCorrectly()
        {
            var p = _builder.Build();
            var result = _subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldFailValidationAlgorithmName()
        {
            var p = _builder.WithAlgorithm(_INVALID).Build();
            var result = _subject.Validate(p);

            Assume.That(!result.Success, "success");
            Assert.IsTrue(result.ErrorMessage.ToLower().Contains("algorithm"));
        }

        [Test]
        public void ShouldFailValidationMode()
        {
            var p = _builder.WithMode(_INVALID).Build();
            var result = _subject.Validate(p);

            Assume.That(!result.Success, "success");
            Assert.IsTrue(result.ErrorMessage.ToLower().Contains("mode"));
        }

        private static object[] _curveTests = new object[]
        {
            new object[]
            {
                "valid",
                EnumHelpers.GetEnumDescriptions<Curve>().ToArray(),
                true
            },
            new object[]
            {
                "one invalid",
                new string[] { "p-192", _INVALID },
                false
            },
            new object[]
            {
                "all invalid",
                new string[] { _INVALID, "also invalid" },
                false
            },
        };

        [Test]
        [TestCaseSource(nameof(_curveTests))]
        
        public void ShouldFailWithInvalidCurves(string label, string[] curves, bool expectedSuccess)
        {
            var p = _builder.WithCurves(curves).Build();
            var result = _subject.Validate(p);

            Assert.AreEqual(expectedSuccess, result.Success);
        }
    }
}