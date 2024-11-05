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

            Assert.That(_subject.D, Is.Null, nameof(_subject.D));
            Assert.That(_subject.Qx, Is.Null, nameof(_subject.Qx));
            Assert.That(_subject.Qy, Is.Null, nameof(_subject.Qy));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var d = new BigInteger(1);
            var qx = new BigInteger(2);
            var qy = new BigInteger(3);
            _subject.KeyPair = new EccKeyPair(new EccPoint(qx, qy), d);

            Assert.That(_subject.D.ToPositiveBigInteger(), Is.EqualTo(d), nameof(_subject.D));
            Assert.That(_subject.Qx.ToPositiveBigInteger(), Is.EqualTo(qx), nameof(_subject.Qx));
            Assert.That(_subject.Qy.ToPositiveBigInteger(), Is.EqualTo(qy), nameof(_subject.Qy));
        }
    }
}
