﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pkcs
{
    public class PkcsPadderWithModifiedMessage : PkcsPadder
    {
        public PkcsPadderWithModifiedMessage(ISha sha) : base(sha) { }

        public override (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            return (key, BitString.BitStringAddition(message, BitString.Two()), nlen);
        }
    }
}