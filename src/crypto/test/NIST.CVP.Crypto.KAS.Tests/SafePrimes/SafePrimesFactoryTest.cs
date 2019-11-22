using System;
using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Crypto.KAS.SafePrimes;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.SafePrimes
{
    [TestFixture, FastCryptoTest]
    public class SafePrimesFactoryTest
    {
        private ISafePrimesFactory _subject = new SafePrimesFactory();
        
        [Test]
        public void ShouldHaveCorrectRelationshipForQFromP()
        {
            Assert.Multiple(() =>
            {
                foreach (var key in EnumHelpers.GetEnumsWithoutDefault<SafePrime>())
                {
                    var value = SafePrimesFactory.SafePrimeDomainParameters.FirstOrDefault(w => w.Key == key)
                        .Value;

                    var calculatedQ = (value.P - 1) / 2;

                    if (value.Q != calculatedQ)
                        Console.WriteLine($"{key}: {calculatedQ.ToHex()}");
                    
                    Assert.AreEqual(value.Q, calculatedQ, key.ToString());
                }
            });
        }

        [Test]
        public void ShouldThrowWhenInvalidSafePrimeGroup()
        {
            Assert.Throws<ArgumentException>(() => _subject.GetSafePrime(SafePrime.None));
        }

        [Test]
        public void ShouldReturnDomainParametersForEachValidSafePrime()
        {
            Assert.Multiple(() =>
            {
                foreach (var key in EnumHelpers.GetEnumsWithoutDefault<SafePrime>())
                {
                    var result = _subject.GetSafePrime(key);
                    
                    Assert.NotNull(result, nameof(key));
                }
            });
        }
    }
}