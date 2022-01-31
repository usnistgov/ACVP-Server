using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.SigGen
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
            _subject.Key = new FfcKeyPair(0, 0);

            Assert.IsNull(_subject.P, nameof(_subject.P));
            Assert.IsNull(_subject.Q, nameof(_subject.Q));
            Assert.IsNull(_subject.G, nameof(_subject.G));

            Assert.IsNull(_subject.X, nameof(_subject.X));
            Assert.IsNull(_subject.Y, nameof(_subject.Y));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BigInteger(1);
            var q = new BigInteger(2);
            var g = new BigInteger(3);

            var x = new BigInteger(4);
            var y = new BigInteger(5);

            _subject.DomainParams = new FfcDomainParameters(p, q, g);
            _subject.Key = new FfcKeyPair(x, y);

            Assert.AreEqual(p, _subject.P.ToPositiveBigInteger(), nameof(_subject.P));
            Assert.AreEqual(q, _subject.Q.ToPositiveBigInteger(), nameof(_subject.Q));
            Assert.AreEqual(g, _subject.G.ToPositiveBigInteger(), nameof(_subject.G));

            Assert.AreEqual(x, _subject.X.ToPositiveBigInteger(), nameof(_subject.X));
            Assert.AreEqual(y, _subject.Y.ToPositiveBigInteger(), nameof(_subject.Y));
        }
    }
}
