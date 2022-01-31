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

            Assert.IsNull(_subject.P, nameof(_subject.P));
            Assert.IsNull(_subject.Q, nameof(_subject.Q));
            Assert.IsNull(_subject.G, nameof(_subject.G));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BigInteger(1);
            var q = new BigInteger(2);
            var g = new BigInteger(3);
            _subject.DomainParams = new FfcDomainParameters(p, q, g);

            Assert.AreEqual(p, _subject.P.ToPositiveBigInteger(), nameof(_subject.P));
            Assert.AreEqual(q, _subject.Q.ToPositiveBigInteger(), nameof(_subject.Q));
            Assert.AreEqual(g, _subject.G.ToPositiveBigInteger(), nameof(_subject.G));
        }
    }
}
