using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.Tests.Fakes
{
    public class FakeSha : ISha
    {
        private ISha _sha;

        public FakeSha(ISha sha)
        {
            _sha = sha;
        }

        public int Count { get; set; }

        public HashFunction HashFunction => _sha.HashFunction;

        public HashResult HashMessage(BitString message, int outlen = 0)
        {
            Count++;
            return _sha.HashMessage(message);
        }

        public HashResult HashNumber(BigInteger number)
        {
            Count++;
            return _sha.HashNumber(number);
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Update(byte[] message, int bitLength)
        {
            throw new NotImplementedException();
        }

        public void Update(int message, int bitLength)
        {
            throw new NotImplementedException();
        }

        public void Final(byte[] output, int outputBitLength = 0)
        {
            throw new NotImplementedException();
        }

        public HashResult HashLargeMessage(LargeBitString message)
        {
            throw new NotImplementedException();
        }
    }
}
