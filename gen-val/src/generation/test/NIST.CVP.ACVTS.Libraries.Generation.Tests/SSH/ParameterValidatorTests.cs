using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        static object[] hashAlgTestCases =
        {
            new object[] {"null", null},
            new object[] {"empty", new string[] { }},
            new object[] {"Invalid value", new string[] {"notValid"}},
            new object[] {"Partially valid", new string[] {"sha-1", "notValid"}},
            new object[] {"Partially valid w/ null", new string[] {"sha2-256", null}}
        };

        [Test]
        [TestCaseSource(nameof(hashAlgTestCases))]
        public void ShouldReturnErrorWithInvalidHashAlg(string testCaseLabel, string[] hashMode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(hashMode)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] cipherTestCases =
        {
            new object[] {"null", null},
            new object[] {"empty", new string[] { }},
            new object[] {"Invalid value", new string[] {"notValid"}},
            new object[] {"Partially valid", new string[] {"aes-128", "notValid"}},
            new object[] {"Partially valid w/ null", new string[] {"tdes", null}}
        };

        [Test]
        [TestCaseSource(nameof(cipherTestCases))]
        public void ShouldReturnErrorWithInvalidCipher(string testCaseLabel, string[] cipherMode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCipher(cipherMode)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }
    }
}
