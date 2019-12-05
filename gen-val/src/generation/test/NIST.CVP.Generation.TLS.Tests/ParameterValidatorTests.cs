using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KDF_Components.v1_0.TLS;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests
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
        public void ShouldReturnErrorWithInvalidHashAlg(string testCaseLabel, string[] tweakMode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(tweakMode)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidHashWithV12()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithVersion(new[] {"v1.2"})
                    .WithHashAlg(new[] {"sha2-224", "sha2-256"})
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }
    }
}
