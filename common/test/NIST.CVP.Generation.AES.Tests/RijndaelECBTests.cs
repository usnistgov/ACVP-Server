using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES.Tests
{
    [TestFixture]
    public class RijndaelECBTests
    {
        Mock<IRijndaelInternals> _mockIRijndaelInternals;
        RijndaelECB _sut;

        [SetUp]
        public void Setup()
        {
            _mockIRijndaelInternals = new Mock<IRijndaelInternals>();
            _sut = new RijndaelECB(_mockIRijndaelInternals.Object);
        }

        static object[] encryptSingleBlockTestData = new object[]
        {
            new object[]
            {
                new Cipher()
                {
                    BlockLength = 8
                },
                new Key(),
                new byte[] { 1 },
                1 // byte length * 8 / blockLength
            },
            new object[]
            {
                new Cipher()
                {
                    BlockLength = 128
                },
                new Key(),
                new byte[] { 1,2,3,4,5,6,7,8,9,10 },
                0
            },
            new object[]
            {
                new Cipher()
                {
                    BlockLength = 8
                },
                new Key(),
                new byte[] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32 },
                32
            },
        };

        [Test]
        [TestCaseSource(nameof(encryptSingleBlockTestData))]
        public void ShouldEncryptSingleBlockForEachBlock(Cipher cipher, Key key, byte[] plainText, int expectedInvokes)
        {
            var result = _sut.BlockEncrypt(cipher, key, plainText, plainText.Length * 8);

            _mockIRijndaelInternals
                .Verify(v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()), 
                Times.Exactly(expectedInvokes), 
                nameof(_mockIRijndaelInternals.Object.EncryptSingleBlock));
        }
    }
}
