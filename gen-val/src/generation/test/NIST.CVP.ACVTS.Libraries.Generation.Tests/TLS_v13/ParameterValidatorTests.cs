using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS_v13
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private readonly ParameterValidator _subject = new ParameterValidator();

        [Test]
        public void ShouldSucceedOnValidAlgorithm()
        {
            var p = new ParameterBuilder().Build();

            Assert.That(_subject.Validate(p).Success, Is.True);
        }

        [Test]
        [TestCase("tlsv1.3", "", "1.0")]
        [TestCase("TLS-v1.0", "KDF", "1.0")]
        public void ShouldErrorOnInvalidAlgorithm(string algorithm, string mode, string revision)
        {
            var p = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMode(mode)
                .WithRevision(revision)
                .Build();

            Assert.That(_subject.Validate(p).Success, Is.False);
        }

        [Test]
        public void ShouldFailWIthAllHashAlgsIncludingDefault()
        {
            var p = new ParameterBuilder()
                .WithHashAlgs(EnumHelpers.GetEnums<HashFunctions>())
                .Build();

            Assert.That(_subject.Validate(p).Success, Is.False);
        }

        [Test]
        [TestCase(true, new[] { HashFunctions.Sha2_d256, HashFunctions.Sha2_d384 })]
        [TestCase(true, new[] { HashFunctions.Sha2_d256 })]
        [TestCase(true, new[] { HashFunctions.Sha2_d384 })]
        [TestCase(false, new[] { HashFunctions.Sha2_d384, HashFunctions.Sha2_d512 })]
        [TestCase(false, new[] { HashFunctions.Sha2_d512, HashFunctions.Sha3_d512 })]
        [TestCase(false, new[] { HashFunctions.Sha2_d384, HashFunctions.None })]
        [TestCase(false, new[] { HashFunctions.Sha2_d384, HashFunctions.None, HashFunctions.None })]
        public void ShouldErrorOnInvalidHashAlgorithms(bool shouldPass, HashFunctions[] hashFunctions)
        {
            var p = new ParameterBuilder()
                .WithHashAlgs(hashFunctions)
                .Build();

            Assert.That(_subject.Validate(p).Success, Is.EqualTo(shouldPass));
        }

        [Test]
        [TestCase(true, new[] { TlsModes1_3.DHE, TlsModes1_3.PSK, TlsModes1_3.PSK_DHE })]
        [TestCase(false, new[] { TlsModes1_3.DHE, TlsModes1_3.None })]
        [TestCase(false, new[] { TlsModes1_3.DHE, TlsModes1_3.None, TlsModes1_3.None })]
        public void ShouldErrorOnInvalidRunningModes(bool shouldPass, TlsModes1_3[] runningModes)
        {
            var p = new ParameterBuilder()
                .WithRunningMode(runningModes)
                .Build();

            Assert.That(_subject.Validate(p).Success, Is.EqualTo(shouldPass));
        }
    }
}
