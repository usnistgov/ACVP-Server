using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
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
            var checkValue = new BigInteger(0);

            _subject.DomainParams = null;

            Assert.AreEqual(checkValue, _subject.P, nameof(_subject.P));
            Assert.AreEqual(checkValue, _subject.Q, nameof(_subject.Q));
            Assert.AreEqual(checkValue, _subject.G, nameof(_subject.G));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BigInteger(1);
            var q = new BigInteger(2);
            var g = new BigInteger(3);
            _subject.DomainParams = new FfcDomainParameters(p, q, g);

            Assert.AreEqual(p, _subject.P, nameof(_subject.P));
            Assert.AreEqual(q, _subject.Q, nameof(_subject.Q));
            Assert.AreEqual(g, _subject.G, nameof(_subject.G));
        }
    }
}
