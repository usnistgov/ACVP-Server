using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.IKEv1.Tests
{
    [TestFixture, FastCryptoTest]
    public class IkeV1FactoryTests
    {
        private IkeV1Factory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new IkeV1Factory();
        }

        [Test]
        [TestCase(AuthenticationMethods.Dsa, typeof(DsaIkeV1))]
        [TestCase(AuthenticationMethods.Pke, typeof(PkeIkeV1))]
        [TestCase(AuthenticationMethods.Psk, typeof(PskIkeV1))]
        public void ShouldReturnCorrectIke(AuthenticationMethods authMode, Type expectedType)
        {
            var result = _subject.GetIkeV1Instance(authMode, new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            Assert.IsInstanceOf(expectedType, result);
        }
    }
}
