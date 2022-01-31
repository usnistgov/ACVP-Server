using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.XTS
{
    public class XtsBlockCipher : IXtsModeBlockCipher
    {
        private readonly IBlockCipherEngine _engine;

        public XtsBlockCipher(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public SymmetricCipherResult ProcessPayload(IXtsModeBlockCipherParameters param)
        {
            var xtsKey = new XtsKey(param.Key);

            if (param.DataUnitLength < _engine.BlockSizeBits)
            {
                return new SymmetricCipherResult($"{nameof(param.DataUnitLength)} must be at least equal to the AES block size");
            }

            return param.Direction switch
            {
                BlockCipherDirections.Encrypt => EncryptDataStream(xtsKey, param.Payload, param.Tweak, param.DataUnitLength),
                BlockCipherDirections.Decrypt => DecryptDataStream(xtsKey, param.Payload, param.Tweak, param.DataUnitLength),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private SymmetricCipherResult EncryptDataStream(XtsKey key, BitString plainText, BitString tweak, int dataUnitLength)
        {
            var dataUnits = ChunkifyDataStream(plainText, dataUnitLength);
            var result = new BitString(0);

            foreach (var dataUnit in dataUnits)
            {
                // Encrypt
                var dataUnitResult = Encrypt(key, dataUnit, tweak);

                // Increment tweak
                tweak = BitString.BitStringAddition(tweak, BitString.One());

                // Record results
                if (dataUnitResult.Success)
                {
                    result = result.ConcatenateBits(dataUnitResult.Result);
                }
                else
                {
                    return dataUnitResult;
                }
            }

            return new SymmetricCipherResult(result);
        }

        private SymmetricCipherResult DecryptDataStream(XtsKey key, BitString cipherText, BitString tweak, int dataUnitLength)
        {
            var dataUnits = ChunkifyDataStream(cipherText, dataUnitLength);
            var result = new BitString(0);

            foreach (var dataUnit in dataUnits)
            {
                // Decrypt
                var dataUnitResult = Decrypt(key, dataUnit, tweak);

                // Increment tweak
                tweak = BitString.BitStringAddition(tweak, BitString.One());

                // Record results
                if (dataUnitResult.Success)
                {
                    result = result.ConcatenateBits(dataUnitResult.Result);
                }
                else
                {
                    return dataUnitResult;
                }
            }

            return new SymmetricCipherResult(result);
        }

        private SymmetricCipherResult Encrypt(XtsKey key, BitString plainText, BitString i)
        {
            // 1
            var P = ChunkifyDataUnit(plainText);
            var m = P.Length - 1;
            var C = new BitString[m + 1];

            for (var q = 0; q <= m - 2; q++)
            {
                C[q] = new BitString(EncryptBlock(key, P[q].ToBytes(), i.ToBytes(), q));
            }

            // 2
            var b = plainText.BitLength % 128;

            // 3
            if (b == 0)
            {
                C[m - 1] = new BitString(EncryptBlock(key, P[m - 1].ToBytes(), i.ToBytes(), m - 1));
                C[m] = new BitString(0);
            }
            // 4
            else
            {
                var CC = new BitString(EncryptBlock(key, P[m - 1].ToBytes(), i.ToBytes(), m - 1));
                C[m] = CC.MSBSubstring(0, b);
                var CP = CC.MSBSubstring(b, 128 - b);
                var PP = BitString.ConcatenateBits(P[m], CP);
                C[m - 1] = new BitString(EncryptBlock(key, PP.ToBytes(), i.ToBytes(), m));
            }

            // 5
            return new SymmetricCipherResult(BuildResult(C));
        }

        private SymmetricCipherResult Decrypt(XtsKey key, BitString cipherText, BitString i)
        {
            // 1
            var C = ChunkifyDataUnit(cipherText);
            var m = C.Length - 1;
            var P = new BitString[m + 1];

            for (var q = 0; q <= m - 2; q++)
            {
                P[q] = new BitString(DecryptBlock(key, C[q].ToBytes(), i.ToBytes(), q));
            }

            // 2
            var b = cipherText.BitLength % 128;

            // 3
            if (b == 0)
            {
                P[m - 1] = new BitString(DecryptBlock(key, C[m - 1].ToBytes(), i.ToBytes(), m - 1));
                P[m] = new BitString(0);
            }
            // 4
            else
            {
                var PP = new BitString(DecryptBlock(key, C[m - 1].ToBytes(), i.ToBytes(), m));
                P[m] = PP.MSBSubstring(0, b);
                var CP = PP.MSBSubstring(b, 128 - b);
                var CC = BitString.ConcatenateBits(C[m], CP);
                P[m - 1] = new BitString(DecryptBlock(key, CC.ToBytes(), i.ToBytes(), m - 1));
            }

            return new SymmetricCipherResult(BuildResult(P));
        }

        private byte[] EncryptBlock(XtsKey key, byte[] plainText, byte[] i, int j)
        {
            // 1
            var tOutBuffer = GetOutputBuffer(_engine.BlockSizeBits);
            _engine.Init(new EngineInitParameters(BlockCipherDirections.Encrypt, key.Key2.ToBytes()));
            _engine.ProcessSingleBlock(i, tOutBuffer, 0);
            var T = MultiplyByAlpha(tOutBuffer, j);

            // 2
            var pp = GetOutputBuffer(_engine.BlockSizeBits);
            for (var idx = 0; idx < _engine.BlockSizeBytes; idx++)
            {
                pp[idx] = (byte)(plainText[idx] ^ T[idx]);
            }

            // 3
            var ccOutBuffer = GetOutputBuffer(_engine.BlockSizeBits);
            _engine.Init(new EngineInitParameters(BlockCipherDirections.Encrypt, key.Key1.ToBytes()));
            _engine.ProcessSingleBlock(pp, ccOutBuffer, 0);

            // 4
            var c = GetOutputBuffer(_engine.BlockSizeBits);
            for (var idx = 0; idx < _engine.BlockSizeBytes; idx++)
            {
                c[idx] = (byte)(ccOutBuffer[idx] ^ T[idx]);
            }

            return c;
        }

        private byte[] DecryptBlock(XtsKey key, byte[] cipherText, byte[] i, int j)
        {
            // 1
            var tOutBuffer = GetOutputBuffer(_engine.BlockSizeBits);
            _engine.Init(new EngineInitParameters(BlockCipherDirections.Encrypt, key.Key2.ToBytes()));
            _engine.ProcessSingleBlock(i, tOutBuffer, 0);
            var T = MultiplyByAlpha(tOutBuffer, j);

            // 2
            var cc = GetOutputBuffer(_engine.BlockSizeBits);
            for (var idx = 0; idx < _engine.BlockSizeBytes; idx++)
            {
                cc[idx] = (byte)(cipherText[idx] ^ T[idx]);
            }

            // 3
            var ppOutBuffer = GetOutputBuffer(_engine.BlockSizeBits);
            _engine.Init(new EngineInitParameters(BlockCipherDirections.Decrypt, key.Key1.ToBytes()));
            _engine.ProcessSingleBlock(cc, ppOutBuffer, 0);

            // 4
            var p = GetOutputBuffer(_engine.BlockSizeBits);
            for (var idx = 0; idx < _engine.BlockSizeBytes; idx++)
            {
                p[idx] = (byte)(ppOutBuffer[idx] ^ T[idx]);
            }

            return p;
        }

        private byte[] MultiplyByAlpha(byte[] a_i, int j)
        {
            for (var i = 0; i < j; i++)
            {
                var a_ip1 = GetOutputBuffer(_engine.BlockSizeBits);

                // Safe to convert to a byte, will never exceed [0, 255]
                a_ip1[0] = Convert.ToByte((2 * (a_i[0] % 128)) ^ (135 * (a_i[15] / 128)));

                for (var k = 1; k < 16; k++)
                {
                    a_ip1[k] = Convert.ToByte((2 * (a_i[k] % 128)) ^ (a_i[k - 1] / 128));
                }

                a_i = a_ip1;
            }

            return a_i;
        }

        private BitString[] ChunkifyDataStream(BitString bs, int chunkSize)
        {
            var blocks = bs.BitLength.CeilingDivide(chunkSize);
            var result = new BitString[blocks];

            for (var i = 0; i < blocks; i++)
            {
                if (i < blocks - 1)
                {
                    result[i] = bs.MSBSubstring(i * chunkSize, chunkSize);
                }
                else
                {
                    result[i] = bs.MSBSubstring(i * chunkSize, bs.BitLength - i * chunkSize);
                }
            }

            return result;
        }

        // Similar to ChunkifyDataStream, but if the input is a multiple of 128, adds an empty block at the end. XTS algorithm depends on this
        private BitString[] ChunkifyDataUnit(BitString bs)
        {
            var blocks = (bs.BitLength / 128) + 1;
            var result = new BitString[blocks];

            for (var i = 0; i < blocks; i++)
            {
                if (i == blocks - 1)
                {
                    // For the last block, grab all remaining bits
                    result[i] = bs.MSBSubstring(i * 128, bs.BitLength - i * 128);
                }
                else
                {
                    result[i] = bs.MSBSubstring(i * 128, 128);
                }
            }

            return result;
        }

        private BitString BuildResult(BitString[] array)
        {
            var result = new BitString(0);
            foreach (var bs in array)
            {
                result = result.ConcatenateBits(bs);
            }

            return result;
        }

        private int GetNumberOfBlocks(int outputLengthBits)
        {
            var numberOfBlocks = outputLengthBits / (_engine.BlockSizeBytes * 8);

            // In cases of partial block, add an additional block for processing.
            if (outputLengthBits % _engine.BlockSizeBits != 0)
            {
                numberOfBlocks++;
            }

            return numberOfBlocks;
        }

        private byte[] GetOutputBuffer(int outputLengthInBits)
        {
            var byteLength = GetNumberOfBlocks(outputLengthInBits) * _engine.BlockSizeBytes;

            return new byte[byteLength];
        }
    }
}
