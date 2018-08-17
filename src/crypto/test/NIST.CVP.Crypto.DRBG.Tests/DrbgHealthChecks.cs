using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, FastCryptoTest]
    public class DrbgHealthChecks
    {
        private readonly IDrbgFactory _subject = new DrbgFactory();

        public static object[] _testData = new object[]
        {
            new object[]
            {
                // drbg mechanism
                DrbgMechanism.Hash,
                // drbg mode
                DrbgMode.SHA1,
                // predResist
                false,
                // reseed implemented
                false,
                // returned bits len
                320,
                // entropy 1
                new BitString("32126A0733B4CDBEA918C214287850C105E2BB34"),
                // nonce
                new BitString("8C730117D5757D17EE21B36B8F1027281E2A20F3"),
                // perso string
                new BitString("9ADCC49014EFD2FF0E9D94DACA9B8ADD45DBA3DB"),
                // addt input 1
                new BitString("9E40E545505686F6061CDF58419229A060BDE93F"),
                // entropy input 1
                new BitString(""),
                // addt input 2
                new BitString("B3A57AE5F24CD9245345618E45EF1316D6B4468B"),
                // entropy input 2
                new BitString(""),
                // expected bits
                new BitString("79172CC43FD3511BF0AD1D42F0FE21EE8A955326617987B175F12458BA8BFB9BB06C9E22CAB23CCD")
            },
            new object[]
            {
                // drbg mechanism
                DrbgMechanism.HMAC,
                // drbg mode
                DrbgMode.SHA1,
                // predResist
                false,
                // reseed implemented
                false,
                // returned bits len
                320,
                // entropy 1
                new BitString("32126A0733B4CDBEA918C214287850C105E2BB34"),
                // nonce
                new BitString("8C730117D5757D17EE21B36B8F1027281E2A20F3"),
                // perso string
                new BitString("9ADCC49014EFD2FF0E9D94DACA9B8ADD45DBA3DB"),
                // addt input 1
                new BitString("9E40E545505686F6061CDF58419229A060BDE93F"),
                // entropy input 1
                new BitString(""),
                // addt input 2
                new BitString("B3A57AE5F24CD9245345618E45EF1316D6B4468B"),
                // entropy input 2
                new BitString(""),
                // expected bits
                new BitString("e6ca2caa56d99e16b5150acd6139f50b88b9c2036fc9a700ff2296fa395977a94b31dda5d20d3150")
            },
            new object[]
            {
                // drbg mechanism
                DrbgMechanism.Hash,
                // drbg mode
                DrbgMode.SHA256,
                // predResist
                false,
                // reseed implemented
                false,
                // returned bits len
                1024,
                // entropy 1
                new BitString("02853AB9E80EE8BC624B043A864233CB6EBD1FCFEC20836B47E05DF175630D21"),
                // nonce
                new BitString("308EF74E08B801C21D91B422BF349A2A0DEB5516E6FDFF7088E05E2A8E917433"),
                // perso string
                new BitString("7839D8A629CB73E27FDFC5051EBE00FC674C64073A16BA536C432E8D5BB81EFD"),
                // addt input 1
                new BitString("7AC51A631A78393E382A6F394E0B50B45D16569E686CC856FB80909CF7E9CC70"),
                // entropy input 1
                new BitString(""),
                // addt input 2
                new BitString("3C98651763B9B1A1013EEACC56746D5C8CE177583DC8C2806CCA547B3AC99E81"),
                // entropy input 2
                new BitString(""),
                // expected bits
                new BitString("A3C5B1FA526B900D3BB58EEC23D6164CF87D96D021884E0695363E427544805402BE9164D51DE5EA071A1EA31A386A667886E664E68695FD1652777AE5066FDA1C9AF81026C5FA4AF9470A6C87E17C8EA3D5EAF7573A2587C5AB315AB1F4EA52A7B3644BD9080ECD5ADA2D4261BE8D54A34EEEF312097D6297CC941BE8EBEAF2")
            },
            //new object[]
            //{
            //    // drbg mechanism
            //    DrbgMechanism.HMAC,
            //    // drbg mode
            //    DrbgMode.SHA256,
            //    // predResist
            //    false,
            //    // reseed implemented
            //    false,
            //    // returned bits len
            //    1024,
            //    // entropy 1
            //    new BitString("02853AB9E80EE8BC624B043A864233CB6EBD1FCFEC20836B47E05DF175630D21"),
            //    // nonce
            //    new BitString("308EF74E08B801C21D91B422BF349A2A0DEB5516E6FDFF7088E05E2A8E917433"),
            //    // perso string
            //    new BitString("7839D8A629CB73E27FDFC5051EBE00FC674C64073A16BA536C432E8D5BB81EFD"),
            //    // addt input 1
            //    new BitString("7AC51A631A78393E382A6F394E0B50B45D16569E686CC856FB80909CF7E9CC70"),
            //    // entropy input 1
            //    new BitString(""),
            //    // addt input 2
            //    new BitString("3C98651763B9B1A1013EEACC56746D5C8CE177583DC8C2806CCA547B3AC99E81"),
            //    // entropy input 2
            //    new BitString(""),
            //    // expected bits
            //    new BitString("A3C5B1FA526B900D3BB58EEC23D6164CF87D96D021884E0695363E427544805402BE9164D51DE5EA071A1EA31A386A667886E664E68695FD1652777AE5066FDA1C9AF81026C5FA4AF9470A6C87E17C8EA3D5EAF7573A2587C5AB315AB1F4EA52A7B3644BD9080ECD5ADA2D4261BE8D54A34EEEF312097D6297CC941BE8EBEAF2")
            //},
            new object[]
            {
                // drbg mechanism
                DrbgMechanism.Hash,
                // drbg mode
                DrbgMode.SHA1,
                // predResist
                true,
                // reseed implemented
                false,
                // returned bits len
                320,
                // entropy 1
                new BitString("71249981E818EFD6764853238D7D04C3F7E7B969"),
                // nonce
                new BitString("6D305EF95F62D2BFB759BDDAA7444472F6AC903B"),
                // perso string
                new BitString("7E84F10CD809286C1300B56C250C7A09A5C1E02F"),
                // addt input 1
                new BitString("78AB5A2D59ADB1D907528AC9964A45BC253A1F38"),
                // entropy input 1
                new BitString("F8EC07CF5B41B66923EEB98D04B790F966B2D6C0"),
                // addt input 2
                new BitString("DF5E88872F34735C742DE809B849E666033A2046"),
                // entropy input 2
                new BitString("B4AA8D9BA0468C050408ADE7B5A9DDC6B29708C3"),
                // expected bits
                new BitString("8CB44F50C19CD95FA65CEEB9EA118C807BCBDEE2ED7BB24AD7D39B888AEA7697F08D057B52E2FE80")
            },
            //new object[]
            //{
            //    // drbg mechanism
            //    DrbgMechanism.HMAC,
            //    // drbg mode
            //    DrbgMode.SHA1,
            //    // predResist
            //    true,
            //    // reseed implemented
            //    false,
            //    // returned bits len
            //    320,
            //    // entropy 1
            //    new BitString("71249981E818EFD6764853238D7D04C3F7E7B969"),
            //    // nonce
            //    new BitString("6D305EF95F62D2BFB759BDDAA7444472F6AC903B"),
            //    // perso string
            //    new BitString("7E84F10CD809286C1300B56C250C7A09A5C1E02F"),
            //    // addt input 1
            //    new BitString("78AB5A2D59ADB1D907528AC9964A45BC253A1F38"),
            //    // entropy input 1
            //    new BitString("F8EC07CF5B41B66923EEB98D04B790F966B2D6C0"),
            //    // addt input 2
            //    new BitString("DF5E88872F34735C742DE809B849E666033A2046"),
            //    // entropy input 2
            //    new BitString("B4AA8D9BA0468C050408ADE7B5A9DDC6B29708C3"),
            //    // expected bits
            //    new BitString("8CB44F50C19CD95FA65CEEB9EA118C807BCBDEE2ED7BB24AD7D39B888AEA7697F08D057B52E2FE80")
            //},
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldReturnCorrectBits(
            DrbgMechanism mechanism, 
            DrbgMode mode, 
            bool predResist, 
            bool reseed, 
            int returnedBitLen, 
            BitString entropy,
            BitString nonce,
            BitString persoString,
            BitString addInput1,
            BitString entropyInput1,
            BitString addInput2,
            BitString entropyInput2,
            BitString expectedBits
        )
        {
            var entropyProvider = new TestableEntropyProvider();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = predResist,
                EntropyInputLen = entropy.BitLength,
                NonceLen = nonce.BitLength,
                PersoStringLen = persoString.BitLength,
                AdditionalInputLen = addInput1.BitLength,
                ReturnedBitsLen = returnedBitLen,
                DerFuncEnabled = false,
                Mechanism = mechanism,
                Mode = mode,
                ReseedImplemented = reseed
            };

            var subject = _subject.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(entropy); // Entropy Input
            entropyProvider.AddEntropy(nonce); // Nonce
            subject.Instantiate(0, persoString); // Perso string
            entropyProvider.AddEntropy(entropyInput1); // Entropy Input Reseed
            entropyProvider.AddEntropy(entropyInput2); // Entropy Input Reseed

            subject.Generate(parameters.ReturnedBitsLen, addInput1); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, addInput2); // Additional Input

            Assert.AreEqual(expectedBits, result.Bits);
        }
    }
}