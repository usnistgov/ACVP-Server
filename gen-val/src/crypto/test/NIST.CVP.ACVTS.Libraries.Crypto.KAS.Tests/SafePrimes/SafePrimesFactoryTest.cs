using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.SafePrimes
{
    [TestFixture, FastCryptoTest]
    public class SafePrimesFactoryTest
    {
        private ISafePrimesGroupFactory _subject = new SafePrimesFactory();

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

                    Assert.That(calculatedQ, Is.EqualTo(value.Q), key.ToString());
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

                    Assert.That(result, Is.Not.Null, nameof(key));
                }
            });
        }
    }
}
