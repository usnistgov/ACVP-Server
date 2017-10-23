using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class BigIntegerExtensionTests
    {
        [Test]
        public void PositiveModuloShouldAlwaysBePositive()
        {
            var rand = new Random800_90();

            for (var i = 0; i < 100; i++)
            {
                var a = rand.GetRandomBigInteger(BigInteger.Pow(2, 1024));
                var b = rand.GetRandomBigInteger(BigInteger.Pow(2, 1024) + 1, BigInteger.Pow(2, 2048));
                var c = rand.GetRandomBigInteger(BigInteger.Pow(2, 2048));

                // b is always greater than a
                var result = (a - b).PosMod(c);
                var negativeResult = (a - b) % c;

                Assert.GreaterOrEqual(result, BigInteger.Zero);

                // result - negativeResult should be a multiple of c. 
                Assert.AreEqual(BigInteger.Zero, (result + BigInteger.Abs(negativeResult)) % c);
            }
        }
    }
}
