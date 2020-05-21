using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        [Test]
        [TestCase(128, "40058A9D25FEE7FC1A27C76F20241F26", "17A2829D857F4C748DAEC3F00B681C7705752EC949289F2EF1ECF2F1A55C721C42A01788AB63C8E236AB58D060401C2709F528A85EC3BD58D672AD249956B9840E1D97C05EED402FF0F8DCF0994E1E0FAE6D64615B2AA84FFF0E12E0245A0800D2AD4BC7DE58FBCA7F09ED9EEA247B321D761ADFFC8884187F2EE607732352AE838B5362051A5016AEE6A67A8441C3B1C29F15085024C63C65DDB48BB972E3F4C64F970E6EBB45EE6693DDF15B87BBE498C6D9BEFC484FB262C893097641F25091D8318A14D006A184E4331C0B900FAEF9763603512FDF5F57B7AC39B8CD13D212F38230694A24CE4F0737696EC478180D20D7DD78BB9F9B2EC7914B255FE1EAD2CBBF19828971D065590621E168A02DED6DBB41C087BF5B7B15D7037F23BD9157C89602EDD7270F3A51AEFE80E95CF47F52C27B7A9A3F5AFAFD222DCD18E9ECAA0887DFE69B4902A7CA44A214B3D2E7355F51B329E6E0B04E13BFBFCF86BFB0E69E2509942196609CAA0CCF545AFFA4587F7F7BCF4C35B001D67A70614EA0A3B21D097D21D87491F69851EBE94C805B3CD94BEFA8E69D76DD314E0E5492")]
        [TestCase(256, "93579F4B946D00BA41BF79010D5C3F92F88965451D435E31682EF6BD1B0757E9", "A5A0A917F0FE4C1DC671C22BAF904393D405D0A5A8FF4CDE9A5C01E5370F2DA35DDFE77F634B111911404D717AA107DE50B6975C3BC5BC6B611D1CBCE2510024BA5358086D9E71413B49DCAFAB708CF0CDCF187D126830B6F1692A7EB666647C74E802F62E3C90BF272197DD62F78E")]
        public void ShouldMonteCarloTestParallelHashForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new ParallelHash_MCT(new Crypto.ParallelHash.ParallelHash());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction(digestSize, digestSize * 2, false);

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 4096));
            
            var blockSizeDomain = new MathDomain();
            blockSizeDomain.AddSegment(new RangeDomainSegment(null, 1, 128));
            var result = subject.MCTHash(hashFunction, messageBitString, domain, blockSizeDomain, false, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);
            
            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            //Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex(), resultDigest.ToHex());
        }
    }
}
