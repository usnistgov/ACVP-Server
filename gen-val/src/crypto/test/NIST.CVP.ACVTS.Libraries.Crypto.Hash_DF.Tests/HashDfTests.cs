using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Hash_DF.Tests
{
    [TestFixture, FastCryptoTest]
    public class HashDfTests
    {
        [Test]
        // Tests were self-generated
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "ABCD", "DCC2E26A049C3066DD2A740A6C1E387729E0F1E2")]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, "ABCD", "F9C15344E34F99E03B3CC505C4A2B1961ECCF84F586E4928453175F2")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "ABCD", "25EC67B3EB8E1F58BB49F42CEF9231B94E5155EE7581A90DD5104409BE00230C")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "ABCD", "2D7466759C6F179D3FF0850C4C452B3FB87B71D35E3705EA20C99E08D61DEFF538CF5F165EA0C997352FEC1DF92329FA")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "ABCD", "1F449C58998F7B10613EC98A480105059B454181D33F4A8B2CEB6BE580740A7F6CD21EE04F61BEBD29DF31F0054C52A7D4CAB495E61ECB1C37578614A3732617")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, "ABCD", "159E8FA9E6AD7C82A4E50DA8A560575E2AA842470D192C25871BBE3E")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "ABCD", "BF1080F4D431E527DBA59C3DC1F5BDB81368DE8D5E0FF7779B69F5253DE9E5FF")]

        //[TestCase(ModeValues.SHA3, DigestSizes.d224, "ABCD", "")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d256, "ABCD", "")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d384, "ABCD", "")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d512, "ABCD", "")]
        public void ShouldComputeCorrectResult(ModeValues mode, DigestSizes digestSize, string inputHex, string outputHex)
        {
            var hashFunction = new HashFunction(mode, digestSize);
            var input = new BitString(inputHex);
            var expectedOutput = new BitString(outputHex);

            var factory = new HashConditioningComponentFactory(new DrbgFactory(new NativeShaFactory(), new HmacFactory(new NativeShaFactory())));
            var subject = factory.GetInstance(hashFunction);

            var result = subject.DerivationFunction(input, hashFunction.OutputLen);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Bits.ToHex(), Is.EqualTo(expectedOutput.ToHex()));
        }
    }
}
