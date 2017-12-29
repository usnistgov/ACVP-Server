using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.TLS
{
    public class TlsKdfv12 : ITlsKdf
    {
        private readonly IHmac _hmac;

        public TlsKdfv12(IHmac hmac)
        {
            _hmac = hmac;
        }

        public TlsKdfResult DeriveKey(BitString preMasterSecret, BitString clientHelloRandom, BitString serverHelloRandom, BitString clientRandom, BitString serverRandom, int keyBlockLength)
        {
            var masterSecret = Prf(preMasterSecret, "master secret", clientHelloRandom.ConcatenateBits(serverHelloRandom), 384);
            var keyBlock = Prf(masterSecret, "key expansion", serverRandom.ConcatenateBits(clientRandom), keyBlockLength);
            return new TlsKdfResult(masterSecret, keyBlock);
        }

        private BitString Prf(BitString secret, string label, BitString seed, int outputLength)
        {
            var labelBytes = new BitString(Encoding.ASCII.GetBytes(label));
            var prfSeed = labelBytes.ConcatenateBits(seed);
            var numBlocks = outputLength.CeilingDivide(_hmac.OutputLength);

            var A = prfSeed.GetDeepCopy();
            var output = new BitString(0);
            for (var i = 0; i < numBlocks; i++)
            {
                A = _hmac.Generate(secret, A).Mac;
                output = output.ConcatenateBits(_hmac.Generate(secret, A.ConcatenateBits(prfSeed)).Mac);
            }

            return output.GetMostSignificantBits(outputLength);
        }
    }
}
