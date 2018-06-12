using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA3.Tests
{
    [TestFixture, FastCryptoTest]
    public class CSHAKETests
    {
        [Test]
        [TestCase(256, "00010203", "C1C36925B6409A04F1B504FCBCA9D82B4017277CB5ED2B2065FC1D3814D5AAF5", "", "Email Signature")]
        public void ShouldCSHAKE128HashCorrectly(int outputLength, string inputHex, string outputHex, string functionName, string customization)
        {
            var message = new BitString(inputHex, outputLength, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(outputLength, 256);

            var subject = new CSHAKE();
            var result = subject.HashMessage(hashFunction, message, functionName, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetCSHAKEHashFunction(int digestSize, int capacity)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                OutputType = Output.cXOF
            };
        }
    }

    
}
