using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public interface IPaddingScheme
    {
        PaddingResult Pad(int nlen, BitString message);
        PaddingResult PadWithModifiedTrailer(int nlen, BitString message);
        PaddingResult PadWithMovedIr(int nlen, BitString message);
        VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey);
        BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey);
    }
}
