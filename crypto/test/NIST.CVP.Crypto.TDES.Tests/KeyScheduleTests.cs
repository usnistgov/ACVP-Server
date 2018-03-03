using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KeyScheduleTests
    {
        [Test]
        [TestCase(new byte[] {0x00, 0x00, 0x00, 0x00}, 0)]
        [TestCase(new byte[] { 0x00, 0x00, 0x00, 0x01 }, 1)]
        [TestCase(new byte[] { 0x00, 0x00, 0x01, 0x01 }, 257)]
        [TestCase(new byte[] { 0x00, 0x00, 0xFF, 0x01 }, 65281)]
        [TestCase(new byte[] { 0x00, 0x01, 0x01, 0x01 }, 65793)]
        [TestCase(new byte[] { 0x00, 0xFF, 0x01, 0x01 }, 16711937)]
        [TestCase(new byte[] { 0x01, 0x01, 0x01, 0x01 }, 16843009)]
        [TestCase(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, 0xFFFFFFFFL)]
        public void ShouldTakeFourBytesToLong(byte[] bytes, long expected)
        {
            var subject = GetSubject();
            var result = subject.FourBytesToLong(bytes, 0);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase( 0, new byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [TestCase( 1, new byte[] { 0x00, 0x00, 0x00, 0x01 })]
        [TestCase( 257, new byte[] { 0x00, 0x00, 0x01, 0x01 })]
        [TestCase( 65281, new byte[] { 0x00, 0x00, 0xFF, 0x01 })]
        [TestCase( 65793, new byte[] { 0x00, 0x01, 0x01, 0x01 })]
        [TestCase( 16711937, new byte[] { 0x00, 0xFF, 0x01, 0x01 })]
        [TestCase( 16843009, new byte[] { 0x01, 0x01, 0x01, 0x01 })]
        [TestCase( 0xFFFFFFFFL, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
        public void ShouldTakeLongToFourBytes(long l, byte[] expected)
        {
            var subject = GetSubject();
            var result = subject.LongToFourBytes(l);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(new long[] { 0, 1 }, new long[] { 128,0 }, 64)] // Input 64
        [TestCase(new long[] { 0, 2}, new long[] { 0, 128}, 63)] //63
        [TestCase(new long[] { 0, 4 }, new long[] { 32768, 0 }, 62)] //62
        [TestCase(new long[] { 0, 8 }, new long[] { 0, 32768 }, 61)] //61
        [TestCase(new long[] { 0, 16 }, new long[] { 8388608,0 }, 60)] //60
        [TestCase(new long[] { 0, 32 }, new long[] {0, 8388608 }, 59)] //59
        [TestCase(new long[] { 0, 64 }, new long[] { 2147483648, 0 }, 58)] //58
        [TestCase(new long[] { 0, 128 }, new long[] { 0, 2147483648 }, 57)] //57
        [TestCase(new long[] { 0, 256 }, new long[] { 64,0 }, 56)] //56
        [TestCase(new long[] { 0, 512 }, new long[] {0, 64 }, 55)] //55
        [TestCase(new long[] { 0, 1024 }, new long[] { 16384, 0 }, 54)] //54
        [TestCase(new long[] { 0, 2048 }, new long[] { 0, 16384 }, 53)] //53
        [TestCase(new long[] { 0, 4096 }, new long[] { 4194304, 0 }, 52)] //52
        [TestCase(new long[] { 0, 8192 }, new long[] { 0, 4194304 }, 51)] //51
        [TestCase(new long[] { 0, 16384 }, new long[] { 1073741824, 0 }, 50)] //50
        [TestCase(new long[] { 0, 32768 }, new long[] { 0, 1073741824 }, 49)] //49
        [TestCase(new long[] { 0, 65536 }, new long[] { 32, 0 }, 48)] //48
        [TestCase(new long[] { 0, 131072 }, new long[] { 0, 32 }, 47)] //47
        [TestCase(new long[] { 0, 262144 }, new long[] { 8192, 0 }, 46)] //46
        [TestCase(new long[] { 0, 524288 }, new long[] { 0, 8192 }, 45)] //45
        [TestCase(new long[] { 0, 1048576 }, new long[] { 2097152, 0 }, 44)] //44
        [TestCase(new long[] { 0, 2097152 }, new long[] { 0, 2097152 }, 43)] //43
        [TestCase(new long[] { 0, 4194304 }, new long[] { 536870912, 0 }, 42)] //42
        [TestCase(new long[] { 0, 8388608 }, new long[] { 0, 536870912 }, 41)] //41
        [TestCase(new long[] { 0, 16777216 }, new long[] { 16, 0 }, 40)] //40
        [TestCase(new long[] { 0, 33554432 }, new long[] { 0, 16 }, 39)] //39
        [TestCase(new long[] { 0, 67108864 }, new long[] { 4096, 0 }, 38)] //38
        [TestCase(new long[] { 0, 134217728 }, new long[] { 0, 4096 }, 37)] //37
        [TestCase(new long[] { 0, 268435456 }, new long[] { 1048576, 0 }, 36)] //36
        [TestCase(new long[] { 0, 536870912 }, new long[] { 0, 1048576 }, 35)] //35
        [TestCase(new long[] { 0, 1073741824 }, new long[] { 268435456, 0 }, 34)] //34
        [TestCase(new long[] { 0, 2147483648 }, new long[] { 0, 268435456 }, 33)] //33
        [TestCase(new long[] { 1,0 }, new long[] { 8, 0 }, 32)] // 32
        [TestCase(new long[] { 2,0 }, new long[] { 0, 8 }, 31)] //31
        [TestCase(new long[] { 4, 0 }, new long[] { 2048, 0 }, 30)] // 30
        [TestCase(new long[] { 8, 0 }, new long[] { 0, 2048 }, 29)] //29
        [TestCase(new long[] { 16, 0 }, new long[] { 524288, 0 }, 28)] // 28
        [TestCase(new long[] { 32, 0 }, new long[] { 0, 524288 }, 27)] //27
        [TestCase(new long[] { 64, 0 }, new long[] { 134217728, 0 }, 26)] // 26
        [TestCase(new long[] { 128, 0 }, new long[] { 0, 134217728 }, 25)] //25
        [TestCase(new long[] { 256, 0 }, new long[] { 4, 0 }, 24)] // 24
        [TestCase(new long[] { 512, 0 }, new long[] { 0, 4 }, 23)] //23
        [TestCase(new long[] { 1024, 0 }, new long[] { 1024, 0 }, 22)] // 22
        [TestCase(new long[] { 2048, 0 }, new long[] { 0, 1024 }, 21)] //21
        [TestCase(new long[] { 4096, 0 }, new long[] { 262144, 0 }, 20)] 
        [TestCase(new long[] { 8192, 0 }, new long[] { 0, 262144 }, 19)]
        [TestCase(new long[] { 16384, 0 }, new long[] { 67108864, 0 }, 18)]
        [TestCase(new long[] { 32768, 0 }, new long[] { 0, 67108864 }, 17)]
        [TestCase(new long[] { 65536, 0 }, new long[] { 2, 0 }, 16)] 
        [TestCase(new long[] { 131072, 0 }, new long[] { 0, 2 }, 15)] 
        [TestCase(new long[] { 262144, 0 }, new long[] { 512, 0 }, 14)] 
        [TestCase(new long[] { 524288, 0 }, new long[] { 0, 512 }, 13)] 
        [TestCase(new long[] { 1048576, 0 }, new long[] { 131072, 0 }, 12)]
        [TestCase(new long[] { 2097152, 0 }, new long[] { 0, 131072 }, 11)]
        [TestCase(new long[] { 4194304, 0 }, new long[] { 33554432, 0 }, 10)]
        [TestCase(new long[] { 8388608, 0 }, new long[] { 0, 33554432 }, 9)]
        [TestCase(new long[] { 16777216, 0 }, new long[] { 1, 0 },8)]
        [TestCase(new long[] { 33554432, 0 }, new long[] { 0, 1 }, 7)]
        [TestCase(new long[] { 67108864, 0 }, new long[] {256, 0 }, 6)]
        [TestCase(new long[] { 134217728, 0 }, new long[] { 0, 256 }, 5)]
        [TestCase(new long[] { 268435456, 0 }, new long[] { 65536, 0 }, 4)]
        [TestCase(new long[] { 536870912, 0 }, new long[] { 0, 65536 }, 3)]
        [TestCase(new long[] { 1073741824, 0 }, new long[] { 16777216, 0 }, 2)]
        [TestCase(new long[] { 2147483648, 0 }, new long[] { 0, 16777216 }, 1)]
        public void ShouldPermuteProperly(long[] input, long[] expected, int inputBitLocation1Based)
        {
            var subject = GetSubject();
            var result = subject.DoInitialPermutation(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(new long[] { 0, 1 }, new long[] { 33554432,0 }, 64)] 
        [TestCase(new long[] { 0, 2 }, new long[] { 131072,0 }, 63)] 
        [TestCase(new long[] { 0, 4 }, new long[] { 512, 0 }, 62)] 
        [TestCase(new long[] { 0, 8 }, new long[] { 2, 0}, 61)] 
        [TestCase(new long[] { 0, 16 }, new long[] {  0, 33554432 }, 60)] 
        [TestCase(new long[] { 0, 32 }, new long[] { 0, 131072 }, 59)] 
        [TestCase(new long[] { 0, 64 }, new long[] { 0,512 }, 58)] 
        [TestCase(new long[] { 0, 128 }, new long[] { 0, 2 }, 57)] 

        [TestCase(new long[] { 0, 256 }, new long[] { 134217728, 0 }, 56)] 
        [TestCase(new long[] { 0, 512 }, new long[] { 524288, 0 }, 55)] 
        [TestCase(new long[] { 0, 1024 }, new long[] { 2048, 0 }, 54)] 
        [TestCase(new long[] { 0, 2048 }, new long[] {8,0 }, 53)] 
        [TestCase(new long[] { 0, 4096 }, new long[] { 0, 134217728 }, 52)] 
        [TestCase(new long[] { 0, 8192 }, new long[] { 0, 524288 }, 51)] 
        [TestCase(new long[] { 0, 16384 }, new long[] {  0, 2048 }, 50)] 
        [TestCase(new long[] { 0, 32768 }, new long[] { 0, 8 }, 49)] 

        [TestCase(new long[] { 0, 65536 }, new long[] { 536870912, 0 }, 48)] 
        [TestCase(new long[] { 0, 131072 }, new long[] { 2097152, 0 }, 47)] 
        [TestCase(new long[] { 0, 262144 }, new long[] { 8192, 0 }, 46)] 
        [TestCase(new long[] { 0, 524288 }, new long[] {  32,0 }, 45)] 
        [TestCase(new long[] { 0, 1048576 }, new long[] { 0, 536870912 }, 44)] 
        [TestCase(new long[] { 0, 2097152 }, new long[] { 0, 2097152 }, 43)] 
        [TestCase(new long[] { 0, 4194304 }, new long[] {  0, 8192 }, 42)] 
        [TestCase(new long[] { 0, 8388608 }, new long[] { 0, 32}, 41)]

        [TestCase(new long[] { 0, 16777216 }, new long[] { 2147483648, 0 }, 40)] 
        [TestCase(new long[] { 0, 33554432 }, new long[] { 8388608, 0 }, 39)]
        [TestCase(new long[] { 0, 67108864 }, new long[] { 32768, 0 }, 38)] 
        [TestCase(new long[] { 0, 134217728 }, new long[] { 128,0 }, 37)]
        [TestCase(new long[] { 0, 268435456 }, new long[] { 0, 2147483648 }, 36)] 
        [TestCase(new long[] { 0, 536870912 }, new long[] { 0, 8388608 }, 35)] 
        [TestCase(new long[] { 0, 1073741824 }, new long[] { 0, 32768 }, 34)] 
        [TestCase(new long[] { 0, 2147483648 }, new long[] { 0, 128}, 33)]

        [TestCase(new long[] { 1, 0 }, new long[] { 16777216, 0 }, 32)] // 32
        [TestCase(new long[] { 2, 0 }, new long[] { 65536, 0 }, 31)] //31
        [TestCase(new long[] { 4, 0 }, new long[] { 256, 0 }, 30)] // 30
        [TestCase(new long[] { 8, 0 }, new long[] { 1,0 }, 29)] //29
        [TestCase(new long[] { 16, 0 }, new long[] { 0,16777216 }, 28)] // 28
        [TestCase(new long[] { 32, 0 }, new long[] { 0, 65536 }, 27)] //27
        [TestCase(new long[] { 64, 0 }, new long[] { 0,256 }, 26)] // 26
        [TestCase(new long[] { 128, 0 }, new long[] { 0, 1 }, 25)] //25

        [TestCase(new long[] { 256, 0 }, new long[] { 67108864, 0 }, 24)] 
        [TestCase(new long[] { 512, 0 }, new long[] { 262144, 0 }, 23)]
        [TestCase(new long[] { 1024, 0 }, new long[] { 1024, 0 }, 22)] 
        [TestCase(new long[] { 2048, 0 }, new long[] { 4, 0}, 21)] 
        [TestCase(new long[] { 4096, 0 }, new long[] { 0, 67108864 }, 20)]
        [TestCase(new long[] { 8192, 0 }, new long[] { 0, 262144 }, 19)]
        [TestCase(new long[] { 16384, 0 }, new long[] { 0,1024 }, 18)]
        [TestCase(new long[] { 32768, 0 }, new long[] { 0,4 }, 17)]

        [TestCase(new long[] { 65536, 0 }, new long[] { 268435456, 0 }, 16)]
        [TestCase(new long[] { 131072, 0 }, new long[] { 1048576, 0 }, 15)]
        [TestCase(new long[] { 262144, 0 }, new long[] { 4096, 0 }, 14)]
        [TestCase(new long[] { 524288, 0 }, new long[] { 16,0 }, 13)]
        [TestCase(new long[] { 1048576, 0 }, new long[] { 0, 268435456 }, 12)]
        [TestCase(new long[] { 2097152, 0 }, new long[] { 0, 1048576 }, 11)]
        [TestCase(new long[] { 4194304, 0 }, new long[] { 0, 4096 }, 10)]
        [TestCase(new long[] { 8388608, 0 }, new long[] { 0, 16 }, 9)]

        [TestCase(new long[] { 16777216, 0 }, new long[] { 1073741824, 0 }, 8)]
        [TestCase(new long[] { 33554432, 0 }, new long[] { 4194304, 0 }, 7)]
        [TestCase(new long[] { 67108864, 0 }, new long[] { 16384, 0 }, 6)]
        [TestCase(new long[] { 134217728, 0 }, new long[] { 64,0 }, 5)]
        [TestCase(new long[] { 268435456, 0 }, new long[] { 0, 1073741824 }, 4)]
        [TestCase(new long[] { 536870912, 0 }, new long[] { 0, 4194304 }, 3)]
        [TestCase(new long[] { 1073741824, 0 }, new long[] { 0, 16384 }, 2)]
        [TestCase(new long[] { 2147483648, 0 }, new long[] { 0, 64}, 1)]
        public void ShouldPermuteInverseProperly(long[] input, long[] expected, int inputBitLocation1Based)
        {
            var subject = GetSubject();
            var result = subject.DoPermutationInverse(input);
            Assert.AreEqual(expected, result);
        }

        private static KeySchedule GetSubject()
        {
            var subject = new KeySchedule(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, FunctionValues.Encryption,
                true);
            return subject;
        }
    }
}
