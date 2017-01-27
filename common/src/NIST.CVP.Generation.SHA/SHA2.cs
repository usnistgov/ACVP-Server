using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using System.Numerics;
using NLog;

namespace NIST.CVP.Generation.SHA
{
    public class SHA2 : SHA
    {
        // Internals that change depending on which HashFunction is used
        private int _blockSize;
        private int _appendedLength;
        private int _resultInclusion;
        private int _rounds;
        private int _bits;
        private int[,] _sumShifts;
        private int[,] _sigmaShifts;
        private string[] _kHex;
        private string[] _hHex;
        
        // Internal state variables
        private BitString[] _k;
        private BitString[] _h;
        private BitString[] _w;
        private BitString[] _temps;

        public override BitString HashMessage(HashFunction hashFunction, BitString message)
        {
            DeclareValues(hashFunction);
            var paddedMessage = PreProcessing(message);
            var chunks = Chunkify(paddedMessage);

            foreach(var chunk in chunks)
            {
                DivideChunk(chunk);
                ProcessBlock();
            }

            return BuildResult();
        }

        private void DeclareValues(HashFunction hashFunction)
        {
            // This gets kinda gross very quickly...
            switch (hashFunction.DigestSize)
            {
                case DigestSizes.d160:
                    throw new Exception("Improper digest size. 160 is reserved for SHA1.");

                case DigestSizes.d224:
                    _blockSize = 512;
                    _appendedLength = 64;
                    _resultInclusion = 7;
                    _rounds = 64;
                    _bits = 32;
                    _sumShifts = new int[,] { { 2, 13, 22 }, { 6, 11, 25 } };
                    _sigmaShifts = new int[,] { { 7, 18, 3 }, { 17, 19, 10 } };
                    _hHex = new string[] { "c1059ed8", "367cd507", "3070dd17", "f70e5939", "ffc00b31", "68581511", "64f98fa7", "befa4fa4" };
                    _kHex = new string[] { "428a2f98", "71374491", "b5c0fbcf", "e9b5dba5", "3956c25b", "59f111f1", "923f82a4", "ab1c5ed5",
                                           "d807aa98", "12835b01", "243185be", "550c7dc3", "72be5d74", "80deb1fe", "9bdc06a7", "c19bf174",
                                           "e49b69c1", "efbe4786", "0fc19dc6", "240ca1cc", "2de92c6f", "4a7484aa", "5cb0a9dc", "76f988da",
                                           "983e5152", "a831c66d", "b00327c8", "bf597fc7", "c6e00bf3", "d5a79147", "06ca6351", "14292967",
                                           "27b70a85", "2e1b2138", "4d2c6dfc", "53380d13", "650a7354", "766a0abb", "81c2c92e", "92722c85",
                                           "a2bfe8a1", "a81a664b", "c24b8b70", "c76c51a3", "d192e819", "d6990624", "f40e3585", "106aa070",
                                           "19a4c116", "1e376c08", "2748774c", "34b0bcb5", "391c0cb3", "4ed8aa4a", "5b9cca4f", "682e6ff3",
                                           "748f82ee", "78a5636f", "84c87814", "8cc70208", "90befffa", "a4506ceb", "bef9a3f7", "c67178f2" };
                    break;

                case DigestSizes.d256:
                    _blockSize = 512;
                    _appendedLength = 64;
                    _resultInclusion = 8;
                    _rounds = 64;
                    _bits = 32;
                    _sumShifts = new int[,] { { 2, 13, 22 }, { 6, 11, 25 } };
                    _sigmaShifts = new int[,] { { 7, 18, 3 }, { 17, 19, 10 } };
                    _hHex = new string[] { "6a09e667", "bb67ae85", "3c6ef372", "a54ff53a", "510e527f", "9b05688c", "1f83d9ab", "5be0cd19" };
                    _kHex = new string[] { "428a2f98", "71374491", "b5c0fbcf", "e9b5dba5", "3956c25b", "59f111f1", "923f82a4", "ab1c5ed5",
                                           "d807aa98", "12835b01", "243185be", "550c7dc3", "72be5d74", "80deb1fe", "9bdc06a7", "c19bf174",
                                           "e49b69c1", "efbe4786", "0fc19dc6", "240ca1cc", "2de92c6f", "4a7484aa", "5cb0a9dc", "76f988da",
                                           "983e5152", "a831c66d", "b00327c8", "bf597fc7", "c6e00bf3", "d5a79147", "06ca6351", "14292967",
                                           "27b70a85", "2e1b2138", "4d2c6dfc", "53380d13", "650a7354", "766a0abb", "81c2c92e", "92722c85",
                                           "a2bfe8a1", "a81a664b", "c24b8b70", "c76c51a3", "d192e819", "d6990624", "f40e3585", "106aa070",
                                           "19a4c116", "1e376c08", "2748774c", "34b0bcb5", "391c0cb3", "4ed8aa4a", "5b9cca4f", "682e6ff3",
                                           "748f82ee", "78a5636f", "84c87814", "8cc70208", "90befffa", "a4506ceb", "bef9a3f7", "c67178f2" };
                    break;

                case DigestSizes.d384:
                    _blockSize = 1024;
                    _appendedLength = 128;
                    _resultInclusion = 6;
                    _rounds = 80;
                    _bits = 64;
                    _sumShifts = new int[,] { { 28, 34, 39 }, { 14, 18, 41 } };
                    _sigmaShifts = new int[,] { { 1, 8, 7 }, { 19, 61, 6 } };
                    _hHex = new string[] { "cbbb9d5dc1059ed8", "629a292a367cd507", "9159015a3070dd17", "152fecd8f70e5939", "67332667ffc00b31", "8eb44a8768581511", "db0c2e0d64f98fa7", "47b5481dbefa4fa4" };
                    _kHex = new string[] { "428a2f98d728ae22", "7137449123ef65cd", "b5c0fbcfec4d3b2f", "e9b5dba58189dbbc", "3956c25bf348b538",
                                           "59f111f1b605d019", "923f82a4af194f9b", "ab1c5ed5da6d8118", "d807aa98a3030242", "12835b0145706fbe",
                                           "243185be4ee4b28c", "550c7dc3d5ffb4e2", "72be5d74f27b896f", "80deb1fe3b1696b1", "9bdc06a725c71235",
                                           "c19bf174cf692694", "e49b69c19ef14ad2", "efbe4786384f25e3", "0fc19dc68b8cd5b5", "240ca1cc77ac9c65",
                                           "2de92c6f592b0275", "4a7484aa6ea6e483", "5cb0a9dcbd41fbd4", "76f988da831153b5", "983e5152ee66dfab",
                                           "a831c66d2db43210", "b00327c898fb213f", "bf597fc7beef0ee4", "c6e00bf33da88fc2", "d5a79147930aa725",
                                           "06ca6351e003826f", "142929670a0e6e70", "27b70a8546d22ffc", "2e1b21385c26c926", "4d2c6dfc5ac42aed",
                                           "53380d139d95b3df", "650a73548baf63de", "766a0abb3c77b2a8", "81c2c92e47edaee6", "92722c851482353b",
                                           "a2bfe8a14cf10364", "a81a664bbc423001", "c24b8b70d0f89791", "c76c51a30654be30", "d192e819d6ef5218",
                                           "d69906245565a910", "f40e35855771202a", "106aa07032bbd1b8", "19a4c116b8d2d0c8", "1e376c085141ab53",
                                           "2748774cdf8eeb99", "34b0bcb5e19b48a8", "391c0cb3c5c95a63", "4ed8aa4ae3418acb", "5b9cca4f7763e373",
                                           "682e6ff3d6b2b8a3", "748f82ee5defb2fc", "78a5636f43172f60", "84c87814a1f0ab72", "8cc702081a6439ec",
                                           "90befffa23631e28", "a4506cebde82bde9", "bef9a3f7b2c67915", "c67178f2e372532b", "ca273eceea26619c",
                                           "d186b8c721c0c207", "eada7dd6cde0eb1e", "f57d4f7fee6ed178", "06f067aa72176fba", "0a637dc5a2c898a6",
                                           "113f9804bef90dae", "1b710b35131c471b", "28db77f523047d84", "32caab7b40c72493", "3c9ebe0a15c9bebc",
                                           "431d67c49c100d4c", "4cc5d4becb3e42b6", "597f299cfc657e2a", "5fcb6fab3ad6faec", "6c44198c4a475817" };
                    break;

                case DigestSizes.d512:
                    _blockSize = 1024;
                    _appendedLength = 128;
                    _resultInclusion = 8;
                    _rounds = 80;
                    _bits = 64;
                    _sumShifts = new int[,] { { 28, 34, 39 }, { 14, 18, 41 } };
                    _sigmaShifts = new int[,] { { 1, 8, 7 }, { 19, 61, 6 } };
                    _hHex = new string[] { "6a09e667f3bcc908", "bb67ae8584caa73b", "3c6ef372fe94f82b", "a54ff53a5f1d36f1", "510e527fade682d1", "9b05688c2b3e6c1f", "1f83d9abfb41bd6b", "5be0cd19137e2179" };
                    _kHex = new string[] { "428a2f98d728ae22", "7137449123ef65cd", "b5c0fbcfec4d3b2f", "e9b5dba58189dbbc", "3956c25bf348b538",
                                           "59f111f1b605d019", "923f82a4af194f9b", "ab1c5ed5da6d8118", "d807aa98a3030242", "12835b0145706fbe",
                                           "243185be4ee4b28c", "550c7dc3d5ffb4e2", "72be5d74f27b896f", "80deb1fe3b1696b1", "9bdc06a725c71235",
                                           "c19bf174cf692694", "e49b69c19ef14ad2", "efbe4786384f25e3", "0fc19dc68b8cd5b5", "240ca1cc77ac9c65",
                                           "2de92c6f592b0275", "4a7484aa6ea6e483", "5cb0a9dcbd41fbd4", "76f988da831153b5", "983e5152ee66dfab",
                                           "a831c66d2db43210", "b00327c898fb213f", "bf597fc7beef0ee4", "c6e00bf33da88fc2", "d5a79147930aa725",
                                           "06ca6351e003826f", "142929670a0e6e70", "27b70a8546d22ffc", "2e1b21385c26c926", "4d2c6dfc5ac42aed",
                                           "53380d139d95b3df", "650a73548baf63de", "766a0abb3c77b2a8", "81c2c92e47edaee6", "92722c851482353b",
                                           "a2bfe8a14cf10364", "a81a664bbc423001", "c24b8b70d0f89791", "c76c51a30654be30", "d192e819d6ef5218",
                                           "d69906245565a910", "f40e35855771202a", "106aa07032bbd1b8", "19a4c116b8d2d0c8", "1e376c085141ab53",
                                           "2748774cdf8eeb99", "34b0bcb5e19b48a8", "391c0cb3c5c95a63", "4ed8aa4ae3418acb", "5b9cca4f7763e373",
                                           "682e6ff3d6b2b8a3", "748f82ee5defb2fc", "78a5636f43172f60", "84c87814a1f0ab72", "8cc702081a6439ec",
                                           "90befffa23631e28", "a4506cebde82bde9", "bef9a3f7b2c67915", "c67178f2e372532b", "ca273eceea26619c",
                                           "d186b8c721c0c207", "eada7dd6cde0eb1e", "f57d4f7fee6ed178", "06f067aa72176fba", "0a637dc5a2c898a6",
                                           "113f9804bef90dae", "1b710b35131c471b", "28db77f523047d84", "32caab7b40c72493", "3c9ebe0a15c9bebc",
                                           "431d67c49c100d4c", "4cc5d4becb3e42b6", "597f299cfc657e2a", "5fcb6fab3ad6faec", "6c44198c4a475817" };
                    break;

                default:
                    throw new Exception("Improper digest size. Enum values not detected.");
            }

            // Turn hex strings into BitStrings
            _h = new BitString[8];
            for(var i = 0; i < 8; i++)
            {
                _h[i] = new BitString(_hHex[i]);
            }

            _k = new BitString[_rounds];
            for(var i = 0; i < _rounds; i++)
            {
                _k[i] = new BitString(_kHex[i]);
            }
        }

        private BitString PreProcessing(BitString message)
        {
            var messageLength = message.BitLength;
            message = BitString.ConcatenateBits(message, new BitString("80"));

            var bitsNeeded = ((((_blockSize - _appendedLength) - message.BitLength) % _blockSize) + _blockSize) % _blockSize;
            message = BitString.ConcatenateBits(message, new BitString(bitsNeeded));

            var messageLengthBS = new BitString(new BigInteger(messageLength), _appendedLength);
            message = BitString.ConcatenateBits(message, messageLengthBS);

            return message;
        }

        private BitString[] Chunkify(BitString paddedMessage)
        {
            // Split padded message into 512-bit chunks
            var numChunks = paddedMessage.BitLength / _blockSize;
            var chunks = new BitString[numChunks];

            for (var i = 0; i < numChunks; i++)
            {
                chunks[i] = paddedMessage.Substring((numChunks - i - 1) * _blockSize, _blockSize);
            }

            return chunks;
        }

        private void DivideChunk(BitString chunk)
        {
            _w = new BitString[_rounds];

            // Split each chunk into 16, 32-bit words
            for (var i = 0; i < 16; i++)
            {
                _w[i] = chunk.Substring((16 - i - 1) * _bits, _bits);
            }

            // The rest of the words are an expansion of the previous
            for (var i = 16; i < _rounds; i++)
            {
                var sigma0 = BitString.XOR(
                            BitString.XOR(
                                RightRotate(_w[i - 15], _sigmaShifts[0,0]), 
                                RightRotate(_w[i - 15], _sigmaShifts[0,1])
                            ),
                            RightShift(_w[i - 15], _sigmaShifts[0,2])
                         );

                var sigma1 = BitString.XOR(
                            BitString.XOR(
                                RightRotate(_w[i - 2], _sigmaShifts[1, 0]), 
                                RightRotate(_w[i - 2], _sigmaShifts[1, 1])
                            ),
                            RightShift(_w[i - 2], _sigmaShifts[1, 2])
                         );

                _w[i] = BitString.AddWithModulo(_w[i - 16], sigma0, _bits);
                _w[i] = BitString.AddWithModulo(_w[i], _w[i - 7], _bits);
                _w[i] = BitString.AddWithModulo(_w[i], sigma1, _bits);
            }
        }

        private void ProcessBlock()
        {
            // would be a-h but h is already used...
            _temps = new BitString[8];
            for(var i = 0; i < 8; i++)
            {
                _temps[i] = _h[i].GetDeepCopy();
            }

            for (var i = 0; i < _rounds; i++)
            {
                // (e rightrotate 6) ^ (e rightrotate 13) ^ (e rightrotate 25)
                var sum1 = BitString.XOR(
                              BitString.XOR(
                                 RightRotate(_temps[4], _sumShifts[1, 0]),
                                 RightRotate(_temps[4], _sumShifts[1, 1])
                              ),
                              RightRotate(_temps[4], _sumShifts[1, 2])
                           );

                // (e & f) ^ (!e and g)
                var ch = BitString.XOR(
                            BitString.AND(_temps[4], _temps[5]),
                            BitString.AND(
                                BitString.NOT(_temps[4]),
                                _temps[6]
                            )
                         );

                // h + s1 + ch + k[i] + w[i]
                var temp1 = BitString.AddWithModulo(_temps[7], sum1, _bits);
                temp1 = BitString.AddWithModulo(temp1, ch, _bits);
                temp1 = BitString.AddWithModulo(temp1, _k[i], _bits);
                temp1 = BitString.AddWithModulo(temp1, _w[i], _bits);

                // (a rightrotate 2) ^ (a rightrotate 13) ^ (a rightrotate 22)
                var sum0 = BitString.XOR(
                            BitString.XOR(
                                 RightRotate(_temps[0], _sumShifts[0, 0]),
                                 RightRotate(_temps[0], _sumShifts[0, 1])
                            ),
                            RightRotate(_temps[0], _sumShifts[0, 2])
                         );

                // (a & b) ^ (a & c) ^ (b & c)
                var maj = BitString.XOR(
                             BitString.XOR(
                                BitString.AND(_temps[0], _temps[1]),
                                BitString.AND(_temps[0], _temps[2])
                             ),
                             BitString.AND(_temps[1], _temps[2])
                          );

                // s0 + maj
                var temp2 = BitString.AddWithModulo(sum0, maj, _bits);

                _temps[7] = _temps[6].GetDeepCopy();
                _temps[6] = _temps[5].GetDeepCopy();
                _temps[5] = _temps[4].GetDeepCopy();
                _temps[4] = BitString.AddWithModulo(_temps[3], temp1, _bits);
                _temps[3] = _temps[2].GetDeepCopy();
                _temps[2] = _temps[1].GetDeepCopy();
                _temps[1] = _temps[0].GetDeepCopy();
                _temps[0] = BitString.AddWithModulo(temp1, temp2, _bits);
            }

            for(var i = 0; i < 8; i++)
            {
                _h[i] = BitString.AddWithModulo(_h[i], _temps[i], _bits);
            }
        }

        private BitString BuildResult()
        {
            var result = new BitString(0);
            for (var i = 0; i < _resultInclusion; i++)
            {
                result = BitString.ConcatenateBits(result, _h[i]);
            }

            return result;
        }

        private BitString RightRotate(BitString bStr, int distance)
        {
            // This works via MSB shift when we want a LSB shift
            return BitString.CircularShiftMSB(bStr, bStr.BitLength - distance);
        }

        private BitString RightShift(BitString bStr, int distance)
        {
            var circleShift = BitString.CircularShiftMSB(bStr, bStr.BitLength - distance);
            for(var i = 0; i < distance; i++)
            {
                circleShift.Set(bStr.BitLength-i-1, false);
            }

            return circleShift;
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
