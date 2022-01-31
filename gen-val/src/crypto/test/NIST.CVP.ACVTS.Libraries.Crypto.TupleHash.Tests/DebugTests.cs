using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TupleHash.Tests
{
    [TestFixture]
    public class DebugTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldTupleHash(bool xof)
        {
            var tuples = new List<BitString>()
            {
                new BitString("0102030405"),
                new BitString("06070809"),
            };

            var tupleHash = new TupleHashFactory().GetTupleHash(new HashFunction(256, 256, xof));

            var result = tupleHash.HashMessage(tuples, 256, 256, xof);

            Assert.True(true);
        }

        [Test]
        public void MctInitalInnerLoop()
        {
            var mct = new TupleHash_MCT(new TupleHash(new TupleHashFactory()));
            var random = new Random800_90();

            var hashFunction = new HashFunction(256, 256, true);

            var result = mct.MCTHash(
                hashFunction,
                new List<BitString>() { new BitString("703B1C24E0119940AB77774F0BA0A38D") },
                new MathDomain().AddSegment(new RangeDomainSegment(random, 16, 65536, 1)),
                false, true);

            Assert.True(true);
        }
    }
}
