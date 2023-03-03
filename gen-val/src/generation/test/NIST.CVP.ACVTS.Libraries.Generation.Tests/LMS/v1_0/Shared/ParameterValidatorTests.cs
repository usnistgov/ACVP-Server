using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.v1_0.Shared
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private readonly ParameterValidator _subject = new();

        private class ParameterBuilder
        {
            private string _algo = "LMS";
            private string _mode = "KeyGen";
            private string _revision = "1.0";
            private GeneralCapabilities _generalCapabilities;
            private SpecificCapability[] _specificCapabilities;

            /// <summary>
            /// Constructs a builder for <see cref="Parameters"/>.  When <see cref="specificCapabilities"/>, the builder
            /// will be instantiated with valid "specific" capabilities, otherwise "general" capabilities will
            /// be used.
            /// </summary>
            /// <param name="specificCapabilities"></param>
            public ParameterBuilder(bool specificCapabilities)
            {
                if (specificCapabilities)
                {
                    _specificCapabilities = new SpecificCapability[] { new()
                    {
                        LmsMode = LmsMode.LMS_SHA256_M24_H5,
                        LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8
                    } };
                }
                else
                {
                    _generalCapabilities = new()
                    {
                        LmsModes = new[] { LmsMode.LMS_SHA256_M24_H5 },
                        LmOtsModes = new[] { LmOtsMode.LMOTS_SHA256_N24_W8 }
                    };
                }
            }

            public ParameterBuilder WithAlgoModeRevision(string algo, string mode, string revision)
            {
                _algo = algo;
                _mode = mode;
                _revision = revision;
                return this;
            }

            public ParameterBuilder WithGeneralCapabilities(GeneralCapabilities value)
            {
                _generalCapabilities = value;
                return this;
            }

            public ParameterBuilder WithSpecificCapabilities(SpecificCapability[] value)
            {
                _specificCapabilities = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters()
                {
                    Algorithm = _algo,
                    Mode = _mode,
                    Revision = _revision,
                    Capabilities = _generalCapabilities,
                    SpecificCapabilities = _specificCapabilities
                };
            }
        }

        [Test]
        public void WhenGivenDefaultParameterBuilder_ShouldPass()
        {
            var pb = new ParameterBuilder(false);
            var p = pb.Build();

            var result = _subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("LMS", "KeyGen", "1.0", true)]
        [TestCase("LMS", "SigGen", "1.0", true)]
        [TestCase("LMS", "SigVer", "1.0", true)]
        [TestCase("LMS", "NotAValidMode", "1.0", false)]
        [TestCase(null, null, null, false)]
        [TestCase(null, "KeyGen", "1.0", false)]
        [TestCase("LMS", null, "1.0", false)]
        [TestCase("LMS", "KeyGen", null, false)]
        public void WhenGivenAlgoModeRevision_ShouldVerifyOnlyValidCombinations(string algo, string mode,
            string revision, bool expectedSuccess)
        {
            var pb = new ParameterBuilder(false)
                .WithAlgoModeRevision(algo, mode, revision);
            var p = pb.Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(expectedSuccess, result.Success);
        }

        [Test]
        public void WhenGivenValidGeneralAndSpecificCapabilities_ShouldFail()
        {
            var pb = new ParameterBuilder(false)
                .WithGeneralCapabilities(new GeneralCapabilities()
                {
                    LmsModes = new[] { LmsMode.LMS_SHA256_M24_H5 },
                    LmOtsModes = new[] { LmOtsMode.LMOTS_SHA256_N24_W8 }
                })
                .WithSpecificCapabilities(new[]
                {
                    new SpecificCapability()
                    {
                        LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8
                    }
                });

            var p = pb.Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        private static readonly IEnumerable<object> _specificCapabilities = new List<object>()
        {
            new object[]
            {
                "All valid",
                new SpecificCapability[]
                {
                    #region SHA, 24 output

                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W8 },

                    #endregion SHA, 24 output

                    #region SHA, 32 output

                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W8 },

                    #endregion SHA, 32 output

                    #region SHAKE, 24 output

                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M24_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W8 },

                    #endregion SHAKE, 24 output

                    #region SHAKE, 32 output

                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H10, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H15, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H20, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W8 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W1 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W2 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W4 },
                    new() { LmsMode = LmsMode.LMS_SHAKE_M32_H25, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N32_W8 },

                    #endregion SHAKE, 32 output
                },
                true
            },
            new object[]
            {
                "Some valid, some invalid",
                new SpecificCapability[]
                {
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1 },
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1 },
                },
                false
            },
            new object[]
            {
                "mixing hash functions",
                new SpecificCapability[]
                {
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHAKE_N24_W1}
                },
                false
            },
            new object[]
            {
                "mixing output lengths",
                new SpecificCapability[]
                {
                    new() { LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.LMOTS_SHA256_N32_W1}
                },
                false
            },
            new object[]
            {
                "invalid lms type",
                new SpecificCapability[]
                {
                    new() {  LmsMode = LmsMode.Invalid, LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1}
                },
                false
            },
            new object[]
            {
                "invalid lm-ots type",
                new SpecificCapability[]
                {
                    new() {  LmsMode = LmsMode.LMS_SHA256_M24_H5, LmOtsMode = LmOtsMode.Invalid}
                },
                false
            },
        };

        [Test]
        [TestCaseSource(nameof(_specificCapabilities))]
        public void WhenGivenSpecificCapabilities_ShouldVerifyAsExpected(string label,
            SpecificCapability[] capabilities, bool expectedSuccess)
        {
            var pb = new ParameterBuilder(true)
                .WithSpecificCapabilities(capabilities);

            var p = pb.Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(expectedSuccess, result.Success);
        }
    }
}
