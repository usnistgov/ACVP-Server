using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pss
{
    public class PssPadderWithModifiedPublicExponent : PssPadder
    {
        public PssPadderWithModifiedPublicExponent(ISha sha, IMaskFunction mask, IEntropyProvider entropy, int saltLength) : base(sha, mask, entropy, saltLength) { }

        public PssPadderWithModifiedPublicExponent(ISha sha, IMaskFunction mask, IEntropyProvider entropy, int saltLength, int outputLen) : base(sha, mask, entropy, saltLength, outputLen) { }
        public override (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            var newKey = new KeyPair { PubKey = key.PubKey };
            if (key.PrivKey is PrivateKey privKey)
            {
                newKey.PrivKey = new PrivateKey
                {
                    D = privKey.D + 2,
                    P = privKey.P,
                    Q = privKey.Q
                };
            }
            else if (key.PrivKey is CrtPrivateKey crtKey)
            {
                newKey.PrivKey = new CrtPrivateKey
                {
                    DMP1 = crtKey.DMP1 + 2,
                    DMQ1 = crtKey.DMQ1,
                    IQMP = crtKey.IQMP,
                    P = crtKey.P,
                    Q = crtKey.Q
                };
            }
            else
            {
                throw new Exception("Bad key format");
            }

            return (newKey, message, nlen);
        }
    }
}
