﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv2
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        static object[] hashAlgTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "sha-1", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "sha2-256", null } }
        };
        [Test]
        [TestCaseSource(nameof(hashAlgTestCases))]
        public void ShouldReturnErrorWithInvalidHashAlg(string testCaseLabel, string[] tweakMode)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(tweakMode)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        #region GetInvalidNonceLens
        static List<object[]> GetInvalidNonceLens()
        {
            var list = new List<object[]>
            {
                new object[]
                {
                    "No segments",
                    new MathDomain()
                },
                new object[]
                {
                    "Below minimum",
                    new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                0,
                                ParameterValidator.MAX_NONCE
                            )
                        )
                },
                new object[]
                {
                    "Above maximum",
                    new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                ParameterValidator.MIN_NONCE,
                                90000
                            )
                        )
                }
            };
            return list;
        }
        #endregion GetInvalidNonceLens
        [Test]
        [TestCaseSource(nameof(GetInvalidNonceLens))]
        public void ShouldReturnErrorWithInvalidNonceLen(string label, MathDomain ptLen)
        {
            Parameters p = new ParameterBuilder()
                .WithInitNonceLengths(ptLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }
    }
}
