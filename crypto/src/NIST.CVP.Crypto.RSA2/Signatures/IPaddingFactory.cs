using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public interface IPaddingFactory
    {
        IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, IEntropyProvider entropyProvider = null, int saltLength = 0);
    }
}
