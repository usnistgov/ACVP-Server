using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using System.Collections.Generic;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TupleHash.Tests
{
    [TestFixture, FastCryptoTest]
    public class TupleHashTests
    {
        [Test]
        [TestCase(1, "c5d8786c1afb9b82111ab34b65b2c0048fa64e6d48e263264ce1707d3ffc8ed1", "")]
        [TestCase(1, "75cdb20ff4db1154e841d758e24160c54bae86eb8c13e7f5f40eb35588e96dfb", "My Tuple App")]
        [TestCase(2, "e60f202c89a2631eda8d4c588ca5fd07f39e5151998deccf973adb3804bb6e84", "My Tuple App")]
        public void ShouldTupleHash128HashCorrectly(int testTupleId, string outputHex, string customization)
        {
            var tuples = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashFunction(256, 256, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuples);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, "cfb7058caca5e668f81a12a20a2195ce97a925f1dba3e7449a56f82201ec607311ac2696b1ab5ea2352df1423bde7bd4bb78c9aed1a853c78672f9eb23bbe194", "")]
        [TestCase(1, "147c2191d5ed7efd98dbd96d7ab5a11692576f5fe2a5065f3e33de6bba9f3aa1c4e9a068a289c61c95aab30aee1e410b0b607de3620e24a4e3bf9852a1d4367e", "My Tuple App")]
        [TestCase(2, "45000be63f9b6bfd89f54717670f69a9bc763591a4f05c50d68891a744bcc6e7d6d5b5e82c018da999ed35b0bb49c9678e526abd8e85c13ed254021db9e790ce", "My Tuple App")]
        public void ShouldTupleHash256HashCorrectly(int testTupleId, string outputHex, string customization)
        {
            var tuples = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashFunction(512, 512, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuples);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, "2f103cd7c32320353495c68de1a8129245c6325f6f2a3d608d92179c96e68488", "")]
        [TestCase(1, "3fc8ad69453128292859a18b6c67d7ad85f01b32815e22ce839c49ec374e9b9a", "My Tuple App")]
        [TestCase(2, "900fe16cad098d28e74d632ed852f99daab7f7df4d99e775657885b4bf76d6f8", "My Tuple App")]
        public void ShouldTupleHashXOF128HashCorrectly(int testTupleId, string outputHex, string customization)
        {
            var tuples = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashXOFFunction(256, 256, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuples);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, "03ded4610ed6450a1e3f8bc44951d14fbc384ab0efe57b000df6b6df5aae7cd568e77377daf13f37ec75cf5fc598b6841d51dd207c991cd45d210ba60ac52eb9", "")]
        [TestCase(1, "6483cb3c9952eb20e830af4785851fc597ee3bf93bb7602c0ef6a65d741aeca7e63c3b128981aa05c6d27438c79d2754bb1b7191f125d6620fca12ce658b2442", "My Tuple App")]
        [TestCase(2, "0c59b11464f2336c34663ed51b2b950bec743610856f36c28d1d088d8a2446284dd09830a6a178dc752376199fae935d86cfdee5913d4922dfd369b66a53c897", "My Tuple App")]
        public void ShouldTupleHashXOF256HashCorrectly(int testTupleId, string outputHex, string customization)
        {
            var tuples = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashXOFFunction(512, 512, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuples);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetTupleHashFunction(int digestSize, int capacity, string customization)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = false
            };
        }

        private HashFunction GetTupleHashXOFFunction(int digestSize, int capacity, string customization)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = true,
                Customization = customization
            };
        }

        private List<BitString> GetTestTuple(int id)
        {
            BitString[] array = null;
            switch (id)
            {
                case 1:
                    array = new BitString[] { new BitString("000102"), new BitString("101112131415") };
                    break;
                case 2:
                    array = new BitString[] { new BitString("000102"), new BitString("101112131415"), new BitString("202122232425262728") };
                    break;
            } 
            return new List<BitString>(array);
        }
    }
}
