using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { -1 }, 0)]
        [TestCase(new int[] { 128, -1 }, 0)]
        [TestCase(new int[] { 128, -1, -2 }, 1)]
        [TestCase(new int[] { 128, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidKeyLength(int[] keyLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLen(keyLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        static object[] directionTestCases =
        {
            new object[] { "empty", new BlockCipherDirections[] { } }
        };
        [Test]
        [TestCaseSource(nameof(directionTestCases))]
        public void ShouldReturnErrorWithInvalidDirection(string testCaseLabel, BlockCipherDirections[] direction)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDirection(direction)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] tweakTestCases =
        {
            new object[] { "empty", new XtsTweakModes[] { } }
        };
        [Test]
        [TestCaseSource(nameof(tweakTestCases))]
        public void ShouldReturnErrorWithInvalidTweakMode(string testCaseLabel, XtsTweakModes[] tweakMode)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithTweakMode(tweakMode)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        #region GetInvalidPtLens
        static List<object[]> GetInvalidPtLens()
        {
            List<object[]> list = new List<object[]>()
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
                                ParameterValidator.MAXIMUM_PT_LEN
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
                                128,
                                90000
                            )
                        )
                }
            };

            return list;
        }
        #endregion GetInvalidPtLens

        [Test]
        [TestCaseSource(nameof(GetInvalidPtLens))]
        public void ShouldReturnErrorWithPtLenInvalid(string label, MathDomain ptLen)
        {
            Parameters p = new ParameterBuilder()
                .WithPtLen(ptLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidPtLens))]
        public void ShouldReturnErrorWithDataUnitLenInvalid(string label, MathDomain dataUnitLen)
        {
            Parameters p = new ParameterBuilder()
                .WithDataUnitLen(dataUnitLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithDataUnitLenNullAndMatchPayloadLen()
        {
            Parameters p = new ParameterBuilder()
                .WithDataUnitLen(null)
                .WithMatchPtLen(true)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        #region InvalidDataUnitLensPayloadLens
        static List<object> GetInvalidDuPtLens()
        {
            return new List<object>
            {
                new object[]
                {
                    "All payloads lower than data units -- Value",
                    new MathDomain().AddSegment(new ValueDomainSegment(2048)),
                    new MathDomain().AddSegment(new ValueDomainSegment(128))
                },
                new object[]
                {
                    "All payloads lower than data units -- Range",
                    new MathDomain().AddSegment(new RangeDomainSegment(null, 1024, 4096, 1)),
                    new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 512, 1))
                },
                new object[]
                {
                    "Weird specific values",
                    new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 65536, 128)),
                    new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 165, 37))
                }
            };
        }
        #endregion
        
        [Test]
        [TestCaseSource(nameof(GetInvalidDuPtLens))]
        public void ShouldReturnErrorWithInvalidDataUnitLenAndPayloadLen(string label, MathDomain dataUnitLen, MathDomain payloadLen)
        {
            Parameters p = new ParameterBuilder()
                .WithDataUnitLen(dataUnitLen)
                .WithPtLen(payloadLen)
                .WithMatchPtLen(false)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);
            
            Assert.IsFalse(result.Success);
        }
    }
}
