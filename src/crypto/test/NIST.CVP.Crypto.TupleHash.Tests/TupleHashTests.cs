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
        [TestCase(1, 256, "b5ec70d703cc815b643bf50608b63cb8a424f739f693a2be8d6b72f0fa42df38", "My Function")]
        [TestCase(2, 256, "e60f202c89a2631eda8d4c588ca5fd07f39e5151998deccf973adb3804bb6e84", "My Tuple App")]
        [TestCase(1, 1607, "468e78a1302265b2de571d7c99f3aa4a1dbf07a901383af9f202070ddf10564e3e3f3556a960a4ba3e70a068433ebba80a646c334796fa6f812ea91a1cb58c83c1205d4b24d361b1b4b5bab0e346e0772535eaab06ca8a6a02fdceb45dfeec8d50ab78f70008d49e822ea5d45ed50292d6106243f2830f38a250578a3d95ea88068d0c32d6e9c14657bb25e36c52b24dcdef2bf574adc9a847d9bbe61882fc24eae573ac53cdc47198978b1018bce755ccc95c50d1cda01058bbcaac3ba170d2c88456bf5222da3753", "My Function")]
        [TestCase(1, 35, "1d68223b04", "My Function")]
        [TestCase(3, 256, "c0cb5084d954a407b2eb6f3c826b343909a04b6112e1307ecba225be254e7182", "My Function")]
        [TestCase(4, 256, "9757bca16d3dfd035e1a586f1e8a34b8f6b26f241695527f6395385a0305030c", "My Function")]
        [TestCase(5, 256, "443799edc7adba7ff5df756f7e392ac93a552055e452f336b7a6e76a255232ba", "My Function")]
        public void ShouldTupleHash128HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashFunction(outputLength, 256);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, 256, "c5d8786c1afb9b82111ab34b65b2c0048fa64e6d48e263264ce1707d3ffc8ed1", "")]
        [TestCase(1, 256, "75cdb20ff4db1154e841d758e24160c54bae86eb8c13e7f5f40eb35588e96dfb", "4d79205475706c6520417070")]
        public void ShouldTupleHashHexCustomizationCorrectly(int testTupleId, int outputLength, string outputHex, string customizationHex)
        {
            var tuple = GetTestTuple(testTupleId);
            var customization = new BitString(customizationHex);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashFunction(outputLength, 256);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(1, 512, "cfb7058caca5e668f81a12a20a2195ce97a925f1dba3e7449a56f82201ec607311ac2696b1ab5ea2352df1423bde7bd4bb78c9aed1a853c78672f9eb23bbe194", "")]
        [TestCase(1, 512, "147c2191d5ed7efd98dbd96d7ab5a11692576f5fe2a5065f3e33de6bba9f3aa1c4e9a068a289c61c95aab30aee1e410b0b607de3620e24a4e3bf9852a1d4367e", "My Tuple App")]
        [TestCase(2, 512, "45000be63f9b6bfd89f54717670f69a9bc763591a4f05c50d68891a744bcc6e7d6d5b5e82c018da999ed35b0bb49c9678e526abd8e85c13ed254021db9e790ce", "My Tuple App")]
        [TestCase(1, 1607, "156408acc04508d50ed83fd67060a108061e93f85cb6b923cbccd76388d5e4e0441283fb31095c831ac4de13793286957385625a00f2e78b03e681f42f12f772935a5c4dc16e99b69ab9bfc65f7a6f30dc7e4d723ce6eda552b116d6fb33da4c976bd9616c0f97cafc338e2c5a50b7692c95f65038446f83670846a70c5ad51a242db3e53922024f85d39352cc67cdac101c7ed7793c9b37a33d1027c628a682736fc268b5aeb1c8548b38099abf3a1e08c845246fe358d902842bff77dc1ae142aa8443c544aac124", "My Function")]
        [TestCase(1, 35, "745800a905", "My Function")]
        [TestCase(3, 256, "429e9236c1e153a2c8feec211b0fbfbab13642a4f4e982369863a13b727c075c", "My Function")]
        [TestCase(4, 256, "d763974ed5328c7b1bd00fb02c5480a90a5945b95e98b90f14b5581aecd0a426", "My Function")]
        public void ShouldTupleHash256HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashFunction(outputLength, 512);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple, customization);

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
        [TestCase(1, 1607, "a513a8a944047fdcfc441b7981f85b34e7b921d64e5cd30e5a8493600049124ffe004e4fa07a839c91baeb80ecb6582b47b0d40e169bca2de6232068f1e1c7e9e9cd35b61abdc00e92ecfa700f9933d6b14f6e0beccbec6fceca3a5c1b96e5221ff28e108db490b4ff14f3cda0f03bfbec4f0dca1e26cbb7709f0d73d43ab3668107cff8039386927994a6064828830653e9a9a6026e60b4f3e4445b3ea07b3857f2428c3904852f6a0cc4a477d95b774169a8fbedb6c3be2d7bbb60d1da09f904f396600e67f0ed31", "My Function")]
        [TestCase(1, 35, "a513a8a904", "My Function")]
        [TestCase(3, 256, "8524b653cfcdf36fa4c2885c91aaca20c152454dd9d98ed2299897cc7fa6561c", "My Function")]
        [TestCase(4, 256, "ddc0561b588045ca71b728231b811d4d37287ec56cd6f57cd7316bf25fe8a64e", "My Function")]
        public void ShouldTupleHashXOF128HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashXOFFunction(outputLength, 256);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple, customization);

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
        [TestCase(1, 1607, "f292449dae5bbde3eaefe6ddc3fe657c317530cc7072ba451d3a844060b8517ebad72f6048fd93cc284d5a6f0d55c2aac444c11cd3ec10ed382dba2c367de9b6a5b371e8e108f1d1cc2b8462f4253e35b6f3bfd1f7c551e9aac6f2676c325a24edacf5f2db604446b1e5a3446f8b5f1f434c8fa6a3d69ff298a5988321be9420e666701de14e5d0e871a7c8c1ca4f5072209182119eda7b709db96b317803b4c91e4875271ef00d46bb365d8138353b6c5b3580ce5711f9983fcbfb2dc9c7ef00bcd2803555399392a", "My Function")]
        [TestCase(1, 35, "f292449d06", "My Function")]
        [TestCase(3, 256, "e56e3fd886eb48f61d304f4fbe3b505c12b20c22fd728e08a43cb863598dff0e", "My Function")]
        [TestCase(4, 256, "4d1c388652ec4f07c552518dd1cbeecf081554aca4800c6a80f0302cecaa0527", "My Function")]
        public void ShouldTupleHashXOF256HashCorrectly(int testTupleId, int outputLength, string outputHex, string customization)
        {
            var tuple = GetTestTuple(testTupleId);

            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetTupleHashXOFFunction(outputLength, 512);

            var subject = new TupleHash();
            var result = subject.HashMessage(hashFunction, tuple, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetTupleHashFunction(int digestLength, int capacity)
        {
            return new HashFunction(digestLength, capacity, false);
        }

        private HashFunction GetTupleHashXOFFunction(int digestLength, int capacity)
        {
            return new HashFunction(digestLength, capacity, true);
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
                    array = new BitString[] { new BitString("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", 2407, false), new BitString("1011121314", 35, false) };
                    break;
                case 4:
                    array = new BitString[] { new BitString(""), new BitString("1011121314151617"), new BitString(""), new BitString(""), new BitString(""), new BitString(""), new BitString("") };
                    break;
                case 5:
                    array = new BitString[] { new BitString("000102", 17, false), new BitString("1011121314", 35, false) };
                    break;
            } 
            return new List<BitString>(array);
        }
    }
}
