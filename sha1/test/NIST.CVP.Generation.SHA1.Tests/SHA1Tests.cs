using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class SHA1Tests
    {
        [Test]
        [TestCase(
            "6162 63",      // "abc" in hex
            "A999 3E36 4706 816A BA3E 2571 7850 C26C 9CD0 D89D"
        )]
        [TestCase(
            "CAFE BABE FACE DAD0 DECA F888",
            "E6F0 CCF9 9E46 DE75 236E 9A1E E121 EAD4 913F CE61"
        )]
        [TestCase(
            "0000 1111 2222 3333 4444 5555 6666 7777 8888 9999 AAAA BBBB CCCC DDDD EEEE FFFF",
            "460D 008D B9AA 9FFA DB2C 6F2B 1F74 201A 87F0 881A"
        )]
        [TestCase(
            "",             // Check null message also
            "DA39 A3EE 5E6B 4B0D 3255 BFEF 9560 1890 AFD8 0709"
        )]
        public void ShouldHashSuccessfully(string message, string digest)
        {
            var messageBS = new BitString(message);
            var digestBS = new BitString(digest);
            var subject = new SHA1(new SHAFactory());

            var results = subject.HashMessage(messageBS);

            Assert.AreEqual(digestBS, results.Digest);
        }
    }
}
