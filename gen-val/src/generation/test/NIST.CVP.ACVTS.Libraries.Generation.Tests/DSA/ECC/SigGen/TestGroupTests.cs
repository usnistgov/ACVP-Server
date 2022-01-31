using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.SigGen
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
            _subject.KeyPair = new EccKeyPair(new EccPoint(0, 0), 0);

            Assert.IsNull(_subject.D, nameof(_subject.D));
            Assert.IsNull(_subject.Qx, nameof(_subject.Qx));
            Assert.IsNull(_subject.Qy, nameof(_subject.Qy));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var d = new BigInteger(1);
            var qx = new BigInteger(2);
            var qy = new BigInteger(3);
            _subject.KeyPair = new EccKeyPair(new EccPoint(qx, qy), d);

            Assert.AreEqual(d, _subject.D.ToPositiveBigInteger(), nameof(_subject.D));
            Assert.AreEqual(qx, _subject.Qx.ToPositiveBigInteger(), nameof(_subject.Qx));
            Assert.AreEqual(qy, _subject.Qy.ToPositiveBigInteger(), nameof(_subject.Qy));
        }
    }
}
