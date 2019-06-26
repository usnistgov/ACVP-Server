using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.Math;
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
            Assert.IsNull(_subject.P, nameof(_subject.P));
            Assert.IsNull(_subject.Q, nameof(_subject.Q));
            Assert.IsNull(_subject.G, nameof(_subject.G));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BitString(new BigInteger(1));
            var q = new BitString(new BigInteger(2));
            var g = new BitString(new BigInteger(3));
            _subject.DomainParams = new FfcDomainParameters(p, q, g);

            Assert.AreEqual(p, _subject.P, nameof(_subject.P));
            Assert.AreEqual(q, _subject.Q, nameof(_subject.Q));
            Assert.AreEqual(g, _subject.G, nameof(_subject.G));
        }
    }
}
