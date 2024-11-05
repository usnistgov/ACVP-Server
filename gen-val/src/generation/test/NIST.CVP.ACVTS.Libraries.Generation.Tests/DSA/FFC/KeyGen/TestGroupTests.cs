using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        private TestGroup _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroup();
        }

        [Test]
        public void KeyPropertiesReturnZeroWhenUnderlyingPropertyIsNull()
        {
            _subject.DomainParams = new FfcDomainParameters(0, 0, 0);

            Assert.That(_subject.P, Is.Null, nameof(_subject.P));
            Assert.That(_subject.Q, Is.Null, nameof(_subject.Q));
            Assert.That(_subject.G, Is.Null, nameof(_subject.G));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BigInteger(1);
            var q = new BigInteger(2);
            var g = new BigInteger(3);
            _subject.DomainParams = new FfcDomainParameters(p, q, g);

            Assert.That(_subject.P.ToPositiveBigInteger(), Is.EqualTo(p), nameof(_subject.P));
            Assert.That(_subject.Q.ToPositiveBigInteger(), Is.EqualTo(q), nameof(_subject.Q));
            Assert.That(_subject.G.ToPositiveBigInteger(), Is.EqualTo(g), nameof(_subject.G));
        }
    }
}
