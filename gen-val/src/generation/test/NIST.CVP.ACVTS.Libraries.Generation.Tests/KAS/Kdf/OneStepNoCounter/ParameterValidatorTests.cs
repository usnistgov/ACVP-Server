using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Kdf.OneStepNoCounter
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {

        private readonly ParameterValidator _subject = new ParameterValidator();

        #region builders
        private class ParameterBuilder
        {
            private string _algorithm = "KDA";
            private string _mode = "OneStepNoCounter";
            private string _revision = "Sp800-56Cr2";
            private string _fixedInfoPattern = "uPartyInfo||vPartyInfo";
            private FixedInfoEncoding[] _fixedInfoEncoding = { FixedInfoEncoding.Concatenation };
            private MathDomain _z = new MathDomain().AddSegment(new ValueDomainSegment(512));

            private AuxFunctionNoCounter[] _auxFunctions =
            {
                new()
                {
                    L = 160,
                    AuxFunctionName = KdaOneStepAuxFunction.SHA1,
                    MacSaltMethods = new []{ MacSaltMethod.Default }
                }
            };

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

            public ParameterBuilder WithAuxFunctions(AuxFunctionNoCounter[] value)
            {
                _auxFunctions = value;
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
                    Z = _z,
                    AuxFunctions = _auxFunctions
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
        [TestCase("KDA", "OneStepNoCounter", "Sp800-56Cr2", true)]
        [TestCase("KDA", "OneStep", "Sp800-56Cr2", false)]
        public void ShouldValidateAlgoModeRevision(string algorithm, string mode, string revision, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithAlgoModeRevision(algorithm, mode, revision)
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("uPartyInfo||vPartyInfo", true)]
        [TestCase("vPartyInfo||uPartyInfo", true)]
        public void ShouldValidateFixedInfoPattern(string fixedInfoPattern, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithFixedInfoPattern(fixedInfoPattern)
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }

        [Test]
        [TestCase(null, false)]
        [TestCase(new[] { FixedInfoEncoding.None }, false)]
        [TestCase(new[] { FixedInfoEncoding.None, FixedInfoEncoding.Concatenation }, false)]
        [TestCase(new[] { FixedInfoEncoding.Concatenation }, true)]
        public void ShouldValidateEncoding(FixedInfoEncoding[] encoding, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithEncoding(encoding)
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }

        [Test]
        [TestCase(112, false)]
        [TestCase(224, true)]
        [TestCase(256, true)]
        [TestCase(260, false)]
        [TestCase(65536, true)]
        public void ShouldCheckSharedSecretLength(int zLength, bool isSuccessful)
        {
            var param = new ParameterBuilder()
                .WithZ(new MathDomain().AddSegment(new ValueDomainSegment(zLength)))
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }

        [Test]
        [TestCase(KdaOneStepAuxFunction.None, 256, false)]

        // Mins
        [TestCase(KdaOneStepAuxFunction.SHA1, 112, true)]

        [TestCase(KdaOneStepAuxFunction.SHA2_D224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D256, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D384, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T256, 112, true)]

        [TestCase(KdaOneStepAuxFunction.SHA3_D224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D256, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D384, 112, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D512, 112, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA1, 112, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D256, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D384, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T256, 112, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D224, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D256, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D384, 112, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D512, 112, true)]

        [TestCase(KdaOneStepAuxFunction.KMAC_128, 112, true)]
        [TestCase(KdaOneStepAuxFunction.KMAC_256, 112, true)]

        // Maxes
        [TestCase(KdaOneStepAuxFunction.SHA1, 160, true)]

        [TestCase(KdaOneStepAuxFunction.SHA2_D224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D256, 256, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D384, 384, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512, 512, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T256, 256, true)]

        [TestCase(KdaOneStepAuxFunction.SHA3_D224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D256, 256, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D384, 384, true)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D512, 512, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA1, 160, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D256, 256, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D384, 384, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512, 512, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T256, 256, true)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D224, 224, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D256, 256, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D384, 384, true)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D512, 512, true)]

        [TestCase(KdaOneStepAuxFunction.KMAC_128, 2048, true)]
        [TestCase(KdaOneStepAuxFunction.KMAC_256, 2048, true)]

        // Over max
        [TestCase(KdaOneStepAuxFunction.SHA1, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.SHA2_D224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D256, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D384, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T256, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.SHA3_D224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D256, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D384, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D512, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA1, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D256, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D384, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T256, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D224, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D256, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D384, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D512, 4096, false)]

        [TestCase(KdaOneStepAuxFunction.KMAC_128, 4096, false)]
        [TestCase(KdaOneStepAuxFunction.KMAC_256, 4096, false)]

        public void ShouldCheckAuxFunctionDkmLength(KdaOneStepAuxFunction auxFunction, int l, bool isSuccessful)
        {
            var auxFunctionNoCounter = new AuxFunctionNoCounter()
            {
                L = l,
                AuxFunctionName = auxFunction,
                MacSaltMethods = new[] { MacSaltMethod.Default }
            };

            var param = new ParameterBuilder()
                .WithAuxFunctions(new[] { auxFunctionNoCounter })
                .Build();

            Assert.AreEqual(isSuccessful, _subject.Validate(param).Success);
        }
    }
}
