using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.KDA.TwoStep
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {

        private readonly ParameterValidator _subject = new ParameterValidator();

        #region builders
        private class ParameterBuilder
        {
            private string _algorithm = "KDA";
            private string _mode = "TwoStep";
            private string _revision = "Sp800-56Cr2";
            private int _l = 512;
            private MathDomain _z = new MathDomain().AddSegment(new ValueDomainSegment(512));
            private bool? _usesHybridSharedSecret = true;
            private MathDomain _auxSharedSecretLen = new MathDomain().AddSegment(new ValueDomainSegment(512));

            private TwoStepCapabilities[] _capabilities = new[]
            {
                new TwoStepCapabilities
                {
                    Encoding = new[] { FixedInfoEncoding.Concatenation },
                    FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                    MacSaltMethods = new[] { MacSaltMethod.Random, MacSaltMethod.Default },
                    CounterLength = new[] { 32 },
                    SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                    MacMode = new[] { MacModes.HMAC_SHA3_224, MacModes.HMAC_SHA512 },
                    KdfMode = KdfModes.Feedback,
                    FixedDataOrder = new[] { CounterLocations.AfterFixedData, CounterLocations.BeforeIterator },
                    SupportsEmptyIv = false
                }
            };
            
            /*private string _fixedInfoPattern = "uPartyInfo||vPartyInfo";
            private FixedInfoEncoding[] _fixedInfoEncoding = { FixedInfoEncoding.Concatenation };
            private HashFunctions[] _hmacAlg = { HashFunctions.Sha1 };
            private MacSaltMethod[] _macSaltMethods = { MacSaltMethod.Default };*/

            
            public ParameterBuilder WithAlgoModeRevision(string algorithm, string mode, string revision)
            {
                _algorithm = algorithm;
                _mode = mode;
                _revision = revision;
                return this;
            }

            /*public ParameterBuilder WithFixedInfoPattern(string value)
            {
                _fixedInfoPattern = value;
                return this;
            }*/

            /*public ParameterBuilder WithEncoding(FixedInfoEncoding[] value)
            {
                _fixedInfoEncoding = value;
                return this;
            }*/

            public ParameterBuilder WithZ(MathDomain value)
            {
                _z = value;
                return this;
            }
            
            public ParameterBuilder WithHybridSharedSecret(bool? value)
            {
                _usesHybridSharedSecret = value;
                return this;
            }
            
            public ParameterBuilder WithAuxSharedSecretLen(MathDomain value)
            {
                _auxSharedSecretLen = value;
                return this;
            }

            public Parameters Build()
            {
                return new()
                {
                    Algorithm = _algorithm,
                    Mode = _mode,
                    Revision = _revision,
                    L = _l,
                    Z = _z,
                    UsesHybridSharedSecret = _usesHybridSharedSecret,
                    AuxSharedSecretLen = _auxSharedSecretLen,
                    Capabilities = _capabilities,
                };
            }
        }
        #endregion builders

        [Test]
        public void ShouldConstructAndValidateSuccessfullyFromBaseBuilder()
        {
            Assert.That(_subject.Validate(new ParameterBuilder().Build()).Success, Is.True);
        }

        [Test]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true)]
        [TestCase("KDA", "OneStep", "Sp800-56Cr2", false)]
        public void ShouldValidateAlgoModeRevision(string algorithm, string mode, string revision, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithAlgoModeRevision(algorithm, mode, revision)
                .Build();

            Assert.That(_subject.Validate(param).Success, Is.EqualTo(isSuccessful));
        }
        
        [Test]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", null, 512,false)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", false, 512,false)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, 512,true)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, null,false)]
        public void ShouldValidateUsesHybridSharedSecretAuxSharedSecretLen(string algorithm, string mode, string revision, 
            bool? usesHybridSharedSecret, int? auxSharedSecretLen, bool isSuccessful)
        {
            MathDomain auxSSL;
            if (auxSharedSecretLen == null)
            {
                auxSSL = null;
            }
            else
            {
                auxSSL = new MathDomain().AddSegment(new ValueDomainSegment(auxSharedSecretLen.Value));
            }
            
            var param = new ParameterBuilder()
                .WithAlgoModeRevision(algorithm, mode, revision)
                .WithHybridSharedSecret(usesHybridSharedSecret)
                .WithAuxSharedSecretLen(auxSSL)
                .Build();

            Assert.That(_subject.Validate(param).Success, Is.EqualTo(isSuccessful));
        }
        
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, 100,false)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, 115,false)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, 512,true)]
        [TestCase("KDA", "TwoStep", "Sp800-56Cr2", true, 66536,false)]
        public void ShouldValidateUsesAuxSharedSecretLen(string algorithm, string mode, string revision, 
            bool? usesHybridSharedSecret, int? auxSharedSecretLen, bool isSuccessful)
        {
            MathDomain auxSSL;
            if (auxSharedSecretLen == null)
            {
                auxSSL = null;
            }
            else
            {
                auxSSL = new MathDomain().AddSegment(new ValueDomainSegment(auxSharedSecretLen.Value));
            }
            
            var param = new ParameterBuilder()
                .WithAlgoModeRevision(algorithm, mode, revision)
                .WithHybridSharedSecret(usesHybridSharedSecret)
                .WithAuxSharedSecretLen(auxSSL)
                .Build();

            Assert.That(_subject.Validate(param).Success, Is.EqualTo(isSuccessful));
        }
    }
}
