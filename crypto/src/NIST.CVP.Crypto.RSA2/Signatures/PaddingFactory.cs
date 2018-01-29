using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class PaddingFactory
    {
        public IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, IEntropyProvider entropyProvider = null, int saltLength = 0)
        {
            switch (sigMode)
            {
                case SignatureSchemes.Ansx931:
                    return new AnsxPadder(sha);

                case SignatureSchemes.Pkcs1v15:
                    return new PkcsPadder(sha);

                case SignatureSchemes.Pss:
                    return new PssPadder(sha, entropyProvider, saltLength);

                default:
                    throw new ArgumentException("Invalid signature scheme");
            }
        }
    }
}
