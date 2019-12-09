using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.MD5;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.MD5
{
    public class Md5 : IMd5
    {
        private const int SizeOfChunk = 512;
        private uint[] _state;
        private BitString[] _fullMessage;

        private void Md5Init()
        {
            _state = new uint[] {0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476};
            _fullMessage = new BitString[1];
            _fullMessage[0] = new BitString(0);
        }

        private HashResult Md5Process(BitString message)
        {
            var lastIndex = _fullMessage.Length - 1;
            var spaceRemainingInLastBlock = SizeOfChunk - _fullMessage[lastIndex].BitLength;

            if (message.BitLength <= spaceRemainingInLastBlock)
            {
                // Message doesn't fill the block entirely, so add it and return success
                _fullMessage[lastIndex] = _fullMessage[lastIndex].ConcatenateBits(message);
                return new HashResult(new BitString(0));
            }
            else
            {
                // Fill out the last partial block
                _fullMessage[lastIndex] = _fullMessage[lastIndex].ConcatenateBits(message.GetMostSignificantBits(spaceRemainingInLastBlock));
                
                // Add an empty block to the end (which is always needed for padding information anyways)
                _fullMessage = _fullMessage.Append(new BitString(0)).ToArray();

                // Try to compress the last full block, if it fails, eject and fail
                var result = Md5Compress(_fullMessage[lastIndex]);
                if (!result.Success)
                {
                    return result;
                }

                // Figure out how many bits are left in the message and try to process them
                var trimmedMessage = message.GetLeastSignificantBits(message.BitLength - spaceRemainingInLastBlock);
                return Md5Process(trimmedMessage);
            }
        }

        private HashResult Md5Compress(BitString block)
        {
            if (block.BitLength != SizeOfChunk)
            {
                return new HashResult("Invalid block size, must be 512-bits");
            }

            var blockBytes = block.ToBytes(true);
            var W = new uint[16];

            for (var i = 15; i >= 0; i--)
            {
                W[i] = ((uint)blockBytes[(15 - i) * 4 + 0] & 255) << 24 |
                       ((uint)blockBytes[(15 - i) * 4 + 1] & 255) << 16 |
                       ((uint)blockBytes[(15 - i) * 4 + 2] & 255) << 8  |
                       ((uint)blockBytes[(15 - i) * 4 + 3] & 255);
            }

            var a = _state[0];
            var b = _state[1];
            var c = _state[2];
            var d = _state[3];

            FF(ref a, b, c, d, W[0], 7, 0xd76aa478);
            FF(ref d, a, b, c, W[1], 12, 0xe8c7b756);
            FF(ref c, d, a, b, W[2], 17, 0x242070db);
            FF(ref b, c, d, a, W[3], 22, 0xc1bdceee);
            FF(ref a, b, c, d, W[4], 7, 0xf57c0faf);
            FF(ref d, a, b, c, W[5], 12, 0x4787c62a);
            FF(ref c, d, a, b, W[6], 17, 0xa8304613);
            FF(ref b, c, d, a, W[7], 22, 0xfd469501);
            FF(ref a, b, c, d, W[8], 7, 0x698098d8);
            FF(ref d, a, b, c, W[9], 12, 0x8b44f7af);
            FF(ref c, d, a, b, W[10], 17, 0xffff5bb1);
            FF(ref b, c, d, a, W[11], 22, 0x895cd7be);
            FF(ref a, b, c, d, W[12], 7, 0x6b901122);
            FF(ref d, a, b, c, W[13], 12, 0xfd987193);
            FF(ref c, d, a, b, W[14], 17, 0xa679438e);
            FF(ref b, c, d, a, W[15], 22, 0x49b40821);
            GG(ref a, b, c, d, W[1], 5, 0xf61e2562);
            GG(ref d, a, b, c, W[6], 9, 0xc040b340);
            GG(ref c, d, a, b, W[11], 14, 0x265e5a51);
            GG(ref b, c, d, a, W[0], 20, 0xe9b6c7aa);
            GG(ref a, b, c, d, W[5], 5, 0xd62f105d);
            GG(ref d, a, b, c, W[10], 9, 0x02441453);
            GG(ref c, d, a, b, W[15], 14, 0xd8a1e681);
            GG(ref b, c, d, a, W[4], 20, 0xe7d3fbc8);
            GG(ref a, b, c, d, W[9], 5, 0x21e1cde6);
            GG(ref d, a, b, c, W[14], 9, 0xc33707d6);
            GG(ref c, d, a, b, W[3], 14, 0xf4d50d87);
            GG(ref b, c, d, a, W[8], 20, 0x455a14ed);
            GG(ref a, b, c, d, W[13], 5, 0xa9e3e905);
            GG(ref d, a, b, c, W[2], 9, 0xfcefa3f8);
            GG(ref c, d, a, b, W[7], 14, 0x676f02d9);
            GG(ref b, c, d, a, W[12], 20, 0x8d2a4c8a);
            HH(ref a, b, c, d, W[5], 4, 0xfffa3942);
            HH(ref d, a, b, c, W[8], 11, 0x8771f681);
            HH(ref c, d, a, b, W[11], 16, 0x6d9d6122);
            HH(ref b, c, d, a, W[14], 23, 0xfde5380c);
            HH(ref a, b, c, d, W[1], 4, 0xa4beea44);
            HH(ref d, a, b, c, W[4], 11, 0x4bdecfa9);
            HH(ref c, d, a, b, W[7], 16, 0xf6bb4b60);
            HH(ref b, c, d, a, W[10], 23, 0xbebfbc70);
            HH(ref a, b, c, d, W[13], 4, 0x289b7ec6);
            HH(ref d, a, b, c, W[0], 11, 0xeaa127fa);
            HH(ref c, d, a, b, W[3], 16, 0xd4ef3085);
            HH(ref b, c, d, a, W[6], 23, 0x04881d05);
            HH(ref a, b, c, d, W[9], 4, 0xd9d4d039);
            HH(ref d, a, b, c, W[12], 11, 0xe6db99e5);
            HH(ref c, d, a, b, W[15], 16, 0x1fa27cf8);
            HH(ref b, c, d, a, W[2], 23, 0xc4ac5665);
            II(ref a, b, c, d, W[0], 6, 0xf4292244);
            II(ref d, a, b, c, W[7], 10, 0x432aff97);
            II(ref c, d, a, b, W[14], 15, 0xab9423a7);
            II(ref b, c, d, a, W[5], 21, 0xfc93a039);
            II(ref a, b, c, d, W[12], 6, 0x655b59c3);
            II(ref d, a, b, c, W[3], 10, 0x8f0ccc92);
            II(ref c, d, a, b, W[10], 15, 0xffeff47d);
            II(ref b, c, d, a, W[1], 21, 0x85845dd1);
            II(ref a, b, c, d, W[8], 6, 0x6fa87e4f);
            II(ref d, a, b, c, W[15], 10, 0xfe2ce6e0);
            II(ref c, d, a, b, W[6], 15, 0xa3014314);
            II(ref b, c, d, a, W[13], 21, 0x4e0811a1);
            II(ref a, b, c, d, W[4], 6, 0xf7537e82);
            II(ref d, a, b, c, W[11], 10, 0xbd3af235);
            II(ref c, d, a, b, W[2], 15, 0x2ad7d2bb);
            II(ref b, c, d, a, W[9], 21, 0xeb86d391);

            _state[0] += a;
            _state[1] += b;
            _state[2] += c;
            _state[3] += d;

            return new HashResult(new BitString(0));
        }

        private void FF(ref uint a, uint b, uint c, uint d, uint M, int s, uint t)
        {
            var f = (d ^ (b & (c ^ d)));
            a += f + M + t;
            a = b + ((a << s) | (a >> (32 - s)));
        }

        private void GG(ref uint a, uint b, uint c, uint d, uint M, int s, uint t)
        {
            var f = c ^ (d & (c ^ b));
            a += f + M + t;
            a = b + ((a << s) | (a >> (32 - s)));
        }

        private void HH(ref uint a, uint b, uint c, uint d, uint M, int s, uint t)
        {
            var f = b ^ c ^ d;
            a += f + M + t;
            a = b + ((a << s) | (a >> (32 - s)));
        }

        private void II(ref uint a, uint b, uint c, uint d, uint M, int s, uint t)
        {
            var f = c ^ (b | (~d));
            a += f + M + t;
            a = b + ((a << s) | (a >> (32 - s)));
        }

        private HashResult Md5Final()
        {
            var originalLength = _fullMessage.Sum(block => block.BitLength);
            var lastBlock = _fullMessage[_fullMessage.Length - 1];

            // If the last block is full, we need to process and put a new one on the end
            if (lastBlock.BitLength == SizeOfChunk)
            {
                var result = Md5Compress(lastBlock);
                if (!result.Success)
                {
                    return result;
                }

                _fullMessage = _fullMessage.Append(new BitString(0)).ToArray();
                lastBlock = _fullMessage[_fullMessage.Length - 1];
            }

            lastBlock = lastBlock.ConcatenateBits(BitString.One());

            if (lastBlock.BitLength > SizeOfChunk)
            {
                return new HashResult("Invalid block size, must be 512-bits");
            }

            // If the last block has more than 448 bits, append 0s to fill it and process the full block
            if (lastBlock.BitLength > 448)
            {
                lastBlock = lastBlock.ConcatenateBits(BitString.Zeroes(SizeOfChunk - lastBlock.BitLength));
                var result = Md5Compress(lastBlock);
                if (!result.Success)
                {
                    return result;
                }

                _fullMessage = _fullMessage.Append(new BitString(0)).ToArray();
                lastBlock = _fullMessage[_fullMessage.Length - 1];
            }

            // Append zeroes to the last block until it has 448 bits and append 64 bits of original length
            lastBlock = lastBlock.ConcatenateBits(BitString.Zeroes(448 - lastBlock.BitLength));

            var lengthBytes = BitString.To64BitString(originalLength).ToBytes(true);
            lastBlock = lastBlock.ConcatenateBits(new BitString(lengthBytes));

            // Process the final block
            return Md5Compress(lastBlock);
        }

        public HashResult Hash(BitString message)
        {
            Md5Init();
            var processResult = Md5Process(message);
            if (!processResult.Success)
            {
                return processResult;
            }

            var finalResult = Md5Final();

            if (finalResult.Success)
            {
                // Reverse byte order
                var stateA = new BitString(_state[0], 32).ToBytes();
                stateA = MsbLsbConversionHelpers.ReverseByteOrder(stateA);

                var stateB = new BitString(_state[1], 32).ToBytes();
                stateB = MsbLsbConversionHelpers.ReverseByteOrder(stateB);

                var stateC = new BitString(_state[2], 32).ToBytes();
                stateC = MsbLsbConversionHelpers.ReverseByteOrder(stateC);

                var stateD = new BitString(_state[3], 32).ToBytes();
                stateD = MsbLsbConversionHelpers.ReverseByteOrder(stateD);
                
                // Build result
                var digest = new BitString(stateA)
                                .ConcatenateBits(new BitString(stateB))
                                .ConcatenateBits(new BitString(stateC))
                                .ConcatenateBits(new BitString(stateD));

                return new HashResult(digest);
            }
            else
            {
                return finalResult;
            }
        }
    }
}
