using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.Tests.Fakes
{
    public class FakeSha : ISha
    {
        private ISha _sha;

        public int Count { get; set; }

        public FakeSha(ISha sha)
        {
            _sha = sha;
        }

        public HashFunction HashFunction => _sha.HashFunction;

        public HashResult HashMessage(BitString message)
        {
            Count++;
            return _sha.HashMessage(message);
        }

        public HashResult HashNumber(BigInteger number)
        {
            Count++;
            return _sha.HashNumber(number);
        }
    }
}
