using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.DSA.v1_0.SigGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
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
            _subject.DomainParams = null;
            _subject.Key = null;

            Assert.IsNull(_subject.P, nameof(_subject.P));
            Assert.IsNull(_subject.Q, nameof(_subject.Q));
            Assert.IsNull(_subject.G, nameof(_subject.G));
            
            Assert.IsNull(_subject.X, nameof(_subject.X));
            Assert.IsNull(_subject.Y, nameof(_subject.Y));
        }

        [Test]
        public void KeyPropertiesReturnActualValueWhenProvided()
        {
            var p = new BitString((BigInteger)1);
            var q = new BitString((BigInteger)2);
            var g = new BitString((BigInteger)3);

            var x = new BitString((BigInteger)4);
            var y = new BitString((BigInteger)5);
            
            _subject.DomainParams = new FfcDomainParameters(p, q, g);
            _subject.Key = new FfcKeyPair(x, y);

            Assert.AreEqual(p, _subject.P, nameof(_subject.P));
            Assert.AreEqual(q, _subject.Q, nameof(_subject.Q));
            Assert.AreEqual(g, _subject.G, nameof(_subject.G));

            Assert.AreEqual(x, _subject.X, nameof(_subject.X));
            Assert.AreEqual(y, _subject.Y, nameof(_subject.Y));
        }
    }
}
