using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture]
    public class AllProbablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        [TestCase(2048, "03", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };
            var subject = new AllProbablePrimesWithConditionsGenerator(hashFunction, EntropyProviderTypes.Random);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(1024, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(1024, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(2048, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(2048, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(3072, "0100000001", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        [TestCase(3072, "d70ff9", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };
            var subject = new AllProbablePrimesWithConditionsGenerator(hashFunction, EntropyProviderTypes.Random);
            subject.SetBitlens(208, 231, 144, 244);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }
    }
}
