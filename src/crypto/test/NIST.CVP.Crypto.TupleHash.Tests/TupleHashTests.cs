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
        [TestCase(1, 256, "c5d8786c1afb9b82111ab34b65b2c0048fa64e6d48e263264ce1707d3ffc8ed1", "")]
        [TestCase(1, 256, "75cdb20ff4db1154e841d758e24160c54bae86eb8c13e7f5f40eb35588e96dfb", "My Tuple App")]
        [TestCase(2, 256, "e60f202c89a2631eda8d4c588ca5fd07f39e5151998deccf973adb3804bb6e84", "My Tuple App")]
        [TestCase(1, 1607, "468e78a1302265b2de571d7c99f3aa4a1dbf07a901383af9f202070ddf10564e3e3f3556a960a4ba3e70a068433ebba80a646c334796fa6f812ea91a1cb58c83c1205d4b24d361b1b4b5bab0e346e0772535eaab06ca8a6a02fdceb45dfeec8d50ab78f70008d49e822ea5d45ed50292d6106243f2830f38a250578a3d95ea88068d0c32d6e9c14657bb25e36c52b24dcdef2bf574adc9a847d9bbe61882fc24eae573ac53cdc47198978b1018bce755ccc95c50d1cda01058bbcaac3ba170d2c88456bf5222da3753", "My Function")]
        [TestCase(1, 35, "1d68223b04", "My Function")]
        [TestCase(3, 256, "d7f108c25eb0c1ed5b9cdd004ead5afb72d807ed16c982a731b23a19c59f25e0", "My Function")]
        [TestCase(4, 256, "9757bca16d3dfd035e1a586f1e8a34b8f6b26f241695527f6395385a0305030c", "My Function")]
        public void ShouldTupleHash128HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashFunction(outputLength, 256, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple);
            System.Console.WriteLine(result.ErrorMessage);
            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, 512, "cfb7058caca5e668f81a12a20a2195ce97a925f1dba3e7449a56f82201ec607311ac2696b1ab5ea2352df1423bde7bd4bb78c9aed1a853c78672f9eb23bbe194", "")]
        [TestCase(1, 512, "147c2191d5ed7efd98dbd96d7ab5a11692576f5fe2a5065f3e33de6bba9f3aa1c4e9a068a289c61c95aab30aee1e410b0b607de3620e24a4e3bf9852a1d4367e", "My Tuple App")]
        [TestCase(2, 512, "45000be63f9b6bfd89f54717670f69a9bc763591a4f05c50d68891a744bcc6e7d6d5b5e82c018da999ed35b0bb49c9678e526abd8e85c13ed254021db9e790ce", "My Tuple App")]
        [TestCase(1, 1607, "156408acc04508d50ed83fd67060a108061e93f85cb6b923cbccd76388d5e4e0441283fb31095c831ac4de13793286957385625a00f2e78b03e681f42f12f772935a5c4dc16e99b69ab9bfc65f7a6f30dc7e4d723ce6eda552b116d6fb33da4c976bd9616c0f97cafc338e2c5a50b7692c95f65038446f83670846a70c5ad51a242db3e53922024f85d39352cc67cdac101c7ed7793c9b37a33d1027c628a682736fc268b5aeb1c8548b38099abf3a1e08c845246fe358d902842bff77dc1ae142aa8443c544aac124", "My Function")]
        [TestCase(1, 35, "745800a905", "My Function")]
        [TestCase(3, 256, "b95455cc71c4701f6e508c7d5a049d5baf192987b5f4e07e6b7d9944ff388997", "My Function")]
        [TestCase(4, 256, "d763974ed5328c7b1bd00fb02c5480a90a5945b95e98b90f14b5581aecd0a426", "My Function")]
        public void ShouldTupleHash256HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashFunction(outputLength, 512, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, 256, "2f103cd7c32320353495c68de1a8129245c6325f6f2a3d608d92179c96e68488", "")]
        [TestCase(1, 256, "3fc8ad69453128292859a18b6c67d7ad85f01b32815e22ce839c49ec374e9b9a", "My Tuple App")]
        [TestCase(2, 256, "900fe16cad098d28e74d632ed852f99daab7f7df4d99e775657885b4bf76d6f8", "My Tuple App")]
        [TestCase(1, 128, "2f103cd7c32320353495c68de1a81292", "")]
        [TestCase(1, 128, "3fc8ad69453128292859a18b6c67d7ad", "My Tuple App")]
        [TestCase(2, 128, "900fe16cad098d28e74d632ed852f99d", "My Tuple App")]
        //[TestCase(1, 1607, "468e78a1302265b2de571d7c99f3aa4a1dbf07a901383af9f202070ddf10564e3e3f3556a960a4ba3e70a068433ebba80a646c334796fa6f812ea91a1cb58c83c1205d4b24d361b1b4b5bab0e346e0772535eaab06ca8a6a02fdceb45dfeec8d50ab78f70008d49e822ea5d45ed50292d6106243f2830f38a250578a3d95ea88068d0c32d6e9c14657bb25e36c52b24dcdef2bf574adc9a847d9bbe61882fc24eae573ac53cdc47198978b1018bce755ccc95c50d1cda01058bbcaac3ba170d2c88456bf5222da3753", "My Function")]
        //[TestCase(1, 35, "1d68223b04", "My Function")]
        //[TestCase(3, 256, "d7f108c25eb0c1ed5b9cdd004ead5afb72d807ed16c982a731b23a19c59f25e0", "My Function")]
        //[TestCase(4, 256, "9757bca16d3dfd035e1a586f1e8a34b8f6b26f241695527f6395385a0305030c", "My Function")]
        public void ShouldTupleHashXOF128HashCorrectly(int testTupleId, int digestSize, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashXOFFunction(digestSize, 256, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, 512, "03ded4610ed6450a1e3f8bc44951d14fbc384ab0efe57b000df6b6df5aae7cd568e77377daf13f37ec75cf5fc598b6841d51dd207c991cd45d210ba60ac52eb9", "")]
        [TestCase(1, 512, "6483cb3c9952eb20e830af4785851fc597ee3bf93bb7602c0ef6a65d741aeca7e63c3b128981aa05c6d27438c79d2754bb1b7191f125d6620fca12ce658b2442", "My Tuple App")]
        [TestCase(2, 512, "0c59b11464f2336c34663ed51b2b950bec743610856f36c28d1d088d8a2446284dd09830a6a178dc752376199fae935d86cfdee5913d4922dfd369b66a53c897", "My Tuple App")]
        [TestCase(1, 256, "03ded4610ed6450a1e3f8bc44951d14fbc384ab0efe57b000df6b6df5aae7cd5", "")]
        [TestCase(1, 256, "6483cb3c9952eb20e830af4785851fc597ee3bf93bb7602c0ef6a65d741aeca7", "My Tuple App")]
        [TestCase(2, 256, "0c59b11464f2336c34663ed51b2b950bec743610856f36c28d1d088d8a244628", "My Tuple App")]
        //[TestCase(1, 1607, "156408acc04508d50ed83fd67060a108061e93f85cb6b923cbccd76388d5e4e0441283fb31095c831ac4de13793286957385625a00f2e78b03e681f42f12f772935a5c4dc16e99b69ab9bfc65f7a6f30dc7e4d723ce6eda552b116d6fb33da4c976bd9616c0f97cafc338e2c5a50b7692c95f65038446f83670846a70c5ad51a242db3e53922024f85d39352cc67cdac101c7ed7793c9b37a33d1027c628a682736fc268b5aeb1c8548b38099abf3a1e08c845246fe358d902842bff77dc1ae142aa8443c544aac124", "My Function")]
        //[TestCase(1, 35, "745800a905", "My Function")]
        //[TestCase(3, 256, "b95455cc71c4701f6e508c7d5a049d5baf192987b5f4e07e6b7d9944ff388997", "My Function")]
        //[TestCase(4, 256, "d763974ed5328c7b1bd00fb02c5480a90a5945b95e98b90f14b5581aecd0a426", "My Function")]
        public void ShouldTupleHashXOF256HashCorrectly(int testTupleId, int digestSize, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex);
            var hashFunction = GetTupleHashXOFFunction(digestSize, 512, customization);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetTupleHashFunction(int digestSize, int capacity, string customization)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = false,
                Customization = customization
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
                case 3:
                    array = new BitString[] { new BitString("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", 2407, true), new BitString("1011121314", 35, true) };
                    break;
                case 4:
                    array = new BitString[] { new BitString(""), new BitString("1011121314151617"), new BitString(""), new BitString(""), new BitString(""), new BitString(""), new BitString("") };
                    break;
            } 
            return new List<BitString>(array);
        }
    }
}
