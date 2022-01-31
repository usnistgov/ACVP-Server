using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_KC;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.KeyConfirmation
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private readonly ParameterValidator _subject = new();
        #region Builders for testing
        private class ParameterBuilder
        {
            private string _algo = "KAS-KC";
            private string _mode;
            private string _revision = "Sp800-56";
            private KeyAgreementRole[] _keyAgreementRoles =
            {
                KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV
            };
            private KeyConfirmationMethod _keyConfirmationMethod = new KeyConfirmationMethodBuilder().Build();

            public ParameterBuilder WithAlgoModeRevision(string algo, string mode, string revision)
            {
                _algo = algo;
                _mode = mode;
                _revision = revision;
                return this;
            }

            public ParameterBuilder WithKeyAgreementRole(KeyAgreementRole[] value)
            {
                _keyAgreementRoles = value;
                return this;
            }

            public ParameterBuilder WithKeyConfirmationMethod(KeyConfirmationMethod value)
            {
                _keyConfirmationMethod = value;
                return this;
            }

            public Parameters Build()
            {
                return new()
                {
                    Algorithm = _algo,
                    Mode = _mode,
                    Revision = _revision,
                    KasRole = _keyAgreementRoles,
                    KeyConfirmationMethod = _keyConfirmationMethod
                };
            }
        }

        private class KeyConfirmationMethodBuilder
        {
            private KeyConfirmationDirection[] _keyConfirmationDirections =
            {
                KeyConfirmationDirection.Unilateral, KeyConfirmationDirection.Bilateral
            };
            private KeyConfirmationRole[] _keyConfirmationRoles =
            {
                KeyConfirmationRole.Provider,
                KeyConfirmationRole.Recipient
            };
            private MacMethods _macMethods = new()
            {
                HmacSha1 = new()
                {
                    KeyLen = 160,
                    MacLen = 160,
                }
            };

            public KeyConfirmationMethodBuilder WithKeyConfirmationDirections(KeyConfirmationDirection[] value)
            {
                _keyConfirmationDirections = value;
                return this;
            }

            public KeyConfirmationMethodBuilder WithKeyConfirmationRoles(KeyConfirmationRole[] value)
            {
                _keyConfirmationRoles = value;
                return this;
            }

            public KeyConfirmationMethodBuilder WithMacMethods(MacMethods value)
            {
                _macMethods = value;
                return this;
            }

            public KeyConfirmationMethod Build()
            {
                return new()
                {
                    MacMethods = _macMethods,
                    KeyConfirmationDirections = _keyConfirmationDirections,
                    KeyConfirmationRoles = _keyConfirmationRoles,
                };
            }
        }
        #endregion Builders for testing

        [Test]
        public void ShouldSucceedWithBuilderDefaults()
        {
            Assert.IsTrue(_subject.Validate(new ParameterBuilder().Build()).Success);
        }

        [Test]
        [TestCase(new[] { KeyAgreementRole.InitiatorPartyU })]
        [TestCase(new[] { KeyAgreementRole.ResponderPartyV })]
        [TestCase(new[] { KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV })]
        public void ShouldAcceptValidKasRoles(KeyAgreementRole[] roles)
        {
            var param = new ParameterBuilder()
                .WithKeyAgreementRole(roles)
                .Build();

            Assert.IsTrue(_subject.Validate(param).Success);
        }

        [Test]
        [TestCase(null)]
        [TestCase(new[] { KeyAgreementRole.None })]
        [TestCase(new[] { KeyAgreementRole.None, KeyAgreementRole.InitiatorPartyU })]
        [TestCase(new[] { KeyAgreementRole.None, KeyAgreementRole.ResponderPartyV })]
        [TestCase(new[] { KeyAgreementRole.None, KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV })]
        public void ShouldRejectInvalidKasRoles(KeyAgreementRole[] roles)
        {
            var param = new ParameterBuilder()
                .WithKeyAgreementRole(roles)
                .Build();

            Assert.IsFalse(_subject.Validate(param).Success);
        }

        [Test]
        [TestCase(new[] { KeyConfirmationDirection.Unilateral })]
        [TestCase(new[] { KeyConfirmationDirection.Bilateral })]
        [TestCase(new[] { KeyConfirmationDirection.Unilateral, KeyConfirmationDirection.Bilateral })]
        public void ShouldAcceptValidKeyConfirmationDirections(KeyConfirmationDirection[] directions)
        {
            var param = new ParameterBuilder()
                .WithKeyConfirmationMethod(
                    new KeyConfirmationMethodBuilder()
                        .WithKeyConfirmationDirections(directions)
                        .Build())
                .Build();

            Assert.IsTrue(_subject.Validate(param).Success);
        }

        [Test]
        [TestCase(null)]
        [TestCase(new[] { KeyConfirmationDirection.None })]
        [TestCase(new[] { KeyConfirmationDirection.None, KeyConfirmationDirection.Unilateral })]
        [TestCase(new[] { KeyConfirmationDirection.None, KeyConfirmationDirection.Bilateral })]
        [TestCase(new[] { KeyConfirmationDirection.None, KeyConfirmationDirection.Unilateral, KeyConfirmationDirection.Bilateral })]
        public void ShouldRejectInvalidKeyConfirmationDirections(KeyConfirmationDirection[] directions)
        {
            var param = new ParameterBuilder()
                .WithKeyConfirmationMethod(
                    new KeyConfirmationMethodBuilder()
                        .WithKeyConfirmationDirections(directions)
                        .Build())
                .Build();

            Assert.IsFalse(_subject.Validate(param).Success);
        }

        [Test]
        [TestCase(new[] { KeyConfirmationRole.Provider })]
        [TestCase(new[] { KeyConfirmationRole.Provider })]
        [TestCase(new[] { KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient })]
        public void ShouldAcceptValidKeyConfirmationDirections(KeyConfirmationRole[] roles)
        {
            var param = new ParameterBuilder()
                .WithKeyConfirmationMethod(
                    new KeyConfirmationMethodBuilder()
                        .WithKeyConfirmationRoles(roles)
                        .Build())
                .Build();

            Assert.IsTrue(_subject.Validate(param).Success);
        }

        [Test]
        [TestCase(null)]
        [TestCase(new[] { KeyConfirmationRole.None })]
        [TestCase(new[] { KeyConfirmationRole.None, KeyConfirmationRole.Provider })]
        [TestCase(new[] { KeyConfirmationRole.None, KeyConfirmationRole.Recipient })]
        [TestCase(new[] { KeyConfirmationRole.None, KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient })]
        public void ShouldRejectInvalidKeyConfirmationRoles(KeyConfirmationRole[] roles)
        {
            var param = new ParameterBuilder()
                .WithKeyConfirmationMethod(
                    new KeyConfirmationMethodBuilder()
                        .WithKeyConfirmationRoles(roles)
                        .Build())
                .Build();

            Assert.IsFalse(_subject.Validate(param).Success);
        }

        private static IEnumerable<object> _testMacMethods = new List<object>()
        {
            new object[]
            {
                "All valid macMethods",
                new MacMethods()
                {
                    Cmac = new MacOptionCmac()
                    {
                        KeyLen = 256,
                        MacLen = 128,
                    },
                    Kmac128 = new MacOptionKmac128()
                    {
                        KeyLen = 256,
                        MacLen = 512,
                    },
                    Kmac256 = new MacOptionKmac256()
                    {
                        KeyLen = 256,
                        MacLen = 512,
                    },
                    HmacSha1 = new MacOptionHmacSha1()
                    {
                        KeyLen = 160,
                        MacLen = 160,
                    },
                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    {
                        KeyLen = 224,
                        MacLen = 224,
                    },
                    HmacSha2_D256 = new MacOptionHmacSha2_d256()
                    {
                        KeyLen = 256,
                        MacLen = 256,
                    },
                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    {
                        KeyLen = 384,
                        MacLen = 384,
                    },
                    HmacSha2_D512 = new MacOptionHmacSha2_d512()
                    {
                        KeyLen = 512,
                        MacLen = 512,
                    },
                    HmacSha2_D512_T224 = new MacOptionHmacSha2_d512_t224()
                    {
                        KeyLen = 224,
                        MacLen = 224,
                    },
                    HmacSha2_D512_T256 = new MacOptionHmacSha2_d512_t256()
                    {
                        KeyLen = 256,
                        MacLen = 256,
                    },
                    HmacSha3_D224 = new MacOptionHmacSha3_d224()
                    {
                        KeyLen = 224,
                        MacLen = 224,
                    },
                    HmacSha3_D256 = new MacOptionHmacSha3_d256()
                    {
                        KeyLen = 256,
                        MacLen = 256,
                    },
                    HmacSha3_D384 = new MacOptionHmacSha3_d384()
                    {
                        KeyLen = 384,
                        MacLen = 384,
                    },
                    HmacSha3_D512 = new MacOptionHmacSha3_d512()
                    {
                        KeyLen = 512,
                        MacLen = 512,
                    },
                },
                true
            },
            new object[]
            {
                "Some valid, some invalid",
                new MacMethods()
                {
                    HmacSha1 = new MacOptionHmacSha1()
                    {
                        KeyLen = 160,
                        MacLen = 160,
                    },
                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    {
                        KeyLen = 224,
                        MacLen = 256, // too high for the output of the specified function
					},
                },
                false
            },
            new object[]
            {
                "Invalid key length cmac",
                new MacMethods()
                {
                    Cmac = new MacOptionCmac()
                    {
                        KeyLen = 112,
                        MacLen = 128
                    }
                },
                false
            },
            new object[]
            {
                "Invalid mac length cmac",
                new MacMethods()
                {
                    Cmac = new MacOptionCmac()
                    {
                        KeyLen = 112,
                        MacLen = 192
                    }
                },
                false
            },
            new object[]
            {
                "Invalid not mod 8 mac length cmac",
                new MacMethods()
                {
                    Cmac = new MacOptionCmac()
                    {
                        KeyLen = 112,
                        MacLen = 121
                    }
                },
                false
            },
            new object[]
            {
                "HMAC below minimum mac",
                new MacMethods()
                {
                    HmacSha1 = new MacOptionHmacSha1()
                    {
                        KeyLen = 112,
                        MacLen = 56
                    }
                },
                false
            },
            new object[]
            {
                "KMAC above max mac",
                new MacMethods()
                {
                    HmacSha1 = new MacOptionHmacSha1()
                    {
                        KeyLen = 112,
                        MacLen = 1024
                    }
                },
                false
            },
        };

        [Test]
        [TestCaseSource(nameof(_testMacMethods))]
        public void ShouldValidateMacMethodsProperly(string label, MacMethods macMethods, bool expectedPass)
        {
            var param = new ParameterBuilder()
                .WithKeyConfirmationMethod(new KeyConfirmationMethodBuilder().WithMacMethods(macMethods).Build())
                .Build();

            Assert.AreEqual(expectedPass, _subject.Validate(param).Success);
        }
    }
}
