using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.KDA.HKDF
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {

        private readonly ParameterValidator _subject = new ParameterValidator();

        #region builders
        private class ParameterBuilder
        {
            private string _algorithm = "KDA";
            private string _mode = "HKDF";
            private string _revision = "Sp800-56Cr2";
            private string _fixedInfoPattern = "uPartyInfo||vPartyInfo";
            private FixedInfoEncoding[] _fixedInfoEncoding = { FixedInfoEncoding.Concatenation };
            private HashFunctions[] _hmacAlg = { HashFunctions.Sha1 };
            private MacSaltMethod[] _macSaltMethods = { MacSaltMethod.Default };
            private MathDomain _saltLens = null;
            private int _l = 256;
            private MathDomain _z = new MathDomain().AddSegment(new ValueDomainSegment(512));
            private bool? _usesHybridSharedSecret = true;
            private MathDomain _auxSharedSecretLen = new MathDomain().AddSegment(new ValueDomainSegment(512));
            
            public ParameterBuilder WithAlgoModeRevision(string algorithm, string mode, string revision)
            {
                _algorithm = algorithm;
                _mode = mode;
                _revision = revision;
                return this;
            }

            public ParameterBuilder WithFixedInfoPattern(string value)
            {
                _fixedInfoPattern = value;
                return this;
            }

            public ParameterBuilder WithEncoding(FixedInfoEncoding[] value)
            {
                _fixedInfoEncoding = value;
                return this;
            }

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
            
            public ParameterBuilder WithHmacAlg(HashFunctions[] value)
            {
                _hmacAlg = value;
                return this;
            }
            
            public ParameterBuilder WithSaltLens(MathDomain value)
            {
                _saltLens = value;
                return this;
            }

            public Parameters Build()
            {
                return new()
                {
                    Algorithm = _algorithm,
                    Mode = _mode,
                    Revision = _revision,
                    FixedInfoPattern = _fixedInfoPattern,
                    Encoding = _fixedInfoEncoding,
                    HmacAlg = _hmacAlg,
                    MacSaltMethods = _macSaltMethods,
                    SaltLens = _saltLens,
                    L = _l,
                    Z = _z,
                    UsesHybridSharedSecret = _usesHybridSharedSecret,
                    AuxSharedSecretLen = _auxSharedSecretLen
                };
            }
        }
        #endregion builders

        [Test]
        public void ShouldConstructAndValidateSuccessfullyFromBaseBuilder()
        {
            Assert.IsTrue(_subject.Validate(new ParameterBuilder().Build()).Success);
        }

        [Test]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true)]
        [TestCase("KDA", "OneStep", "Sp800-56Cr2", false)]
        public void ShouldValidateAlgoModeRevision(string algorithm, string mode, string revision, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithAlgoModeRevision(algorithm, mode, revision)
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }
        
        [Test]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", null, 512,false)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", false, 512,false)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, 512,true)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, null,false)]
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

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }
        
        [Test]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, 100,false)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, 115,false)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, 512,true)]
        [TestCase("KDA", "HKDF", "Sp800-56Cr2", true, 66536,false)]
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

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }

        [Test]
        [TestCase("should pass", new HashFunctions[] { HashFunctions.Sha2_d512}, 8, 9, true)]
        [TestCase("has a saltLen < 8", new HashFunctions[] { HashFunctions.Sha2_d512}, 7, 9, false)]
        [TestCase("all of the saltLens exceed the input block size for one of the hashAlgs", new HashFunctions[] { HashFunctions.Sha2_d512, HashFunctions.Sha3_d512}, 600, 600, false)]
        [TestCase("largest saltLen is larger than the largest hash input block size", new HashFunctions[] { HashFunctions.Sha2_d512, HashFunctions.Sha3_d512}, 600, 1030, false)]
        public void ShouldValidateSaltLens(string description, HashFunctions[] hashAlg, int saltMin, int saltMax, bool isSuccessful)
        {
            var saltLens = new MathDomain()
                .AddSegment(new ValueDomainSegment(saltMin))
                .AddSegment(new ValueDomainSegment(saltMax));

            var param = new ParameterBuilder()
                .WithHmacAlg(hashAlg)
                .WithSaltLens(saltLens)
                .Build();
            
            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }
    }
}
