using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
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

            _subject.KeyPair = null;

            Assert.AreEqual(checkValue, _subject.D, nameof(_subject.D));
            Assert.AreEqual(checkValue, _subject.Qx, nameof(_subject.Qx));
            Assert.AreEqual(checkValue, _subject.Qy, nameof(_subject.Qy));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var d = new BigInteger(1);
            var qx = new BigInteger(2);
            var qy = new BigInteger(3);
            _subject.KeyPair = new EccKeyPair(new EccPoint(qx, qy), d);

            Assert.AreEqual(d, _subject.D, nameof(_subject.D));
            Assert.AreEqual(qx, _subject.Qx, nameof(_subject.Qx));
            Assert.AreEqual(qy, _subject.Qy, nameof(_subject.Qy));
        }
    }
}
