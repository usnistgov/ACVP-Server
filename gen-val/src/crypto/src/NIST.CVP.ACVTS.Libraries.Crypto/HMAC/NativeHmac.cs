using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HMAC
{
    public class NativeHmac : IHmac
    {
        private readonly ISha _sha;
        private readonly byte _iPad = 0x36;
        private readonly byte _oPad = 0x5c;

        public int OutputLength => _sha.HashFunction.OutputLen;
        private readonly int _blockSize;
        private readonly int _processingLen;

        private byte[] _k0;
        private byte[] _k0XorIPad;
        private byte[] _k0XorOPad;
        private byte[] _hashedStream;

        public NativeHmac(ISha sha)
        {
            _sha = sha;
            _blockSize = _sha.HashFunction.BlockSize;
            _processingLen = _sha.HashFunction.ProcessingLen;

            _k0 = new byte[_blockSize / 8];
            _k0XorIPad = new byte[_blockSize / 8];
            _k0XorOPad = new byte[_blockSize / 8];
            _hashedStream = new byte[_processingLen / 8];
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            Init(key.GetPaddedBytes());
            Update(message.GetPaddedBytes(), message.BitLength);

            var result = new byte[_processingLen / 8];
            Final(ref result, 0);

            if (macLength == 0 || macLength == OutputLength)
            {
                return new MacResult(new BitString(result));
            }

            return new MacResult(new BitString(result).GetMostSignificantBits(macLength));
        }

        public void Init(byte[] key)
        {
            // Prepare k0
            if (key.Length == _blockSize / 8)
            {
                Array.Copy(key, _k0, key.Length);
            }
            else if (key.Length > _blockSize / 8)
            {
                _sha.Init();
                _sha.Update(key, key.Length * 8);
                _sha.Final(_k0);
            }
            else
            {
                // Make sure the remainder of k0 is 0 bytes
                Array.Clear(_k0, 0, _k0.Length);
                Array.Copy(key, _k0, key.Length);
            }

            Array.Fill(_k0XorIPad, _iPad);
            Array.Fill(_k0XorOPad, _oPad);

            for (var i = 0; i < _k0.Length; i++)
            {
                _k0XorIPad[i] ^= _k0[i];
                _k0XorOPad[i] ^= _k0[i];
            }

            // Prepare SHA for a stream of updates
            _sha.Init();
            _sha.Update(_k0XorIPad, _blockSize);
        }

        /// <summary>
        /// Only use this if you have already used Init and you want to keep those values
        /// </summary>
        public void FastInit()
        {
            _sha.Init();
            _sha.Update(_k0XorIPad, _blockSize);
        }

        public void Update(byte[] message, int bitLength)
        {
            _sha.Update(message, bitLength);
        }

        public void Final(ref byte[] output, int macLength)
        {
            if (macLength == 0)
            {
                macLength = OutputLength;
            }

            // Obtain hashed stream
            _sha.Final(_hashedStream);

            // Hash final results
            _sha.Init();
            _sha.Update(_k0XorOPad, _blockSize);
            _sha.Update(_hashedStream, OutputLength);

            // Expand size of result to account for all the data coming out in the digest
            // For truncated modes, the amount of bits needed is bigger than the output length
            if (output.Length != _processingLen / 8)
            {
                Array.Resize(ref output, _processingLen / 8);
            }

            _sha.Final(output);

            if (output.Length != macLength)
            {
                Array.Resize(ref output, macLength / 8);
            }
        }
    }
}
