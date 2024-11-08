﻿using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS
{
    public class TlsKdfv12 : ITlsKdf
    {
        private readonly IHmac _hmac;

        protected virtual string MasterSecretLabel() => "master secret";

        public TlsKdfv12(IHmac hmac)
        {
            _hmac = hmac;
        }

        public TlsKdfResult DeriveKey(BitString preMasterSecret, BitString clientHelloRandom, BitString serverHelloRandom, BitString clientRandom, BitString serverRandom, int keyBlockLength)
        {
            var masterSecret = Prf(preMasterSecret, MasterSecretLabel(), clientHelloRandom.ConcatenateBits(serverHelloRandom), 384);
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
