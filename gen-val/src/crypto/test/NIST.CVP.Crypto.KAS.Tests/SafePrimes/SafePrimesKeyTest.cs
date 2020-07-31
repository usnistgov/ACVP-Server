using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.SafePrimes
{
    [TestFixture, FastCryptoTest]
    public class SafePrimesKeyTest
    {
        private readonly ISafePrimesGroupFactory _safePrimesGroupFactory = new SafePrimesFactory();
        private readonly IDsaFfcFactory _dsaFactory = new DsaFfcFactory(new ShaFactory());
        
        [Test]
        public void ShouldGenKeysForEachGroup()
        {
            var groups = EnumHelpers.GetEnumsWithoutDefault<SafePrime>();
            var dsa = _dsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            
            Assert.Multiple(() =>
            {
                foreach (var group in groups)
                {
                    var safePrime = _safePrimesGroupFactory.GetSafePrime(group);
                    var key = dsa.GenerateKeyPair(new FfcDomainParameters(safePrime.P, safePrime.Q, safePrime.G));
                    
                    Assert.IsTrue(key.Success, nameof(group));
                }
            });
        }
        
        [Test]
        public void ShouldGenVerForEachGroup()
        {
            var groups = EnumHelpers.GetEnumsWithoutDefault<SafePrime>();
            var dsa = _dsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            
            Assert.Multiple(() =>
            {
                foreach (var group in groups)
                {
                    var safePrime = _safePrimesGroupFactory.GetSafePrime(group);
                    var key = dsa.GenerateKeyPair(new FfcDomainParameters(safePrime.P, safePrime.Q, safePrime.G));

                    var ver = dsa.ValidateKeyPair(safePrime, key.KeyPair);
                    Assert.IsTrue(ver.Success);
                }
            });
        }
    }
}