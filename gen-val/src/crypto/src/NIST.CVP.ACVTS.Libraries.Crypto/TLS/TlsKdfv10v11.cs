using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS
{
    public class TlsKdfv10v11 : ITlsKdf
    {
        private readonly IHmac _shaHmac;
        private readonly IHmac _md5Hmac;

        public TlsKdfv10v11(IHmac shaHmac, IHmac md5Hmac)
        {
            _shaHmac = shaHmac;
            _md5Hmac = md5Hmac;
        }

        public TlsKdfResult DeriveKey(BitString preMasterSecret, BitString clientHelloRandom, BitString serverHelloRandom, BitString clientRandom, BitString serverRandom, int keyBlockLength)
        {
            var masterSecretResult = Prf(preMasterSecret, "master secret", clientHelloRandom.ConcatenateBits(serverHelloRandom), 384);
            if (!string.IsNullOrEmpty(masterSecretResult.error))
            {
                return new TlsKdfResult(masterSecretResult.error);
            }

            var masterSecret = masterSecretResult.output;
            var keyBlockResult = Prf(masterSecret, "key expansion", serverRandom.ConcatenateBits(clientRandom), keyBlockLength);
            if (!string.IsNullOrEmpty(keyBlockResult.error))
            {
                return new TlsKdfResult(keyBlockResult.error);
            }

            return new TlsKdfResult(masterSecret, keyBlockResult.output);
        }


        private (BitString output, string error) Prf(BitString secret, string label, BitString seed, int outputLength)
        {
            int lengthToGenerate = outputLength.CeilingDivide(_shaHmac.OutputLength * _md5Hmac.OutputLength) * (_shaHmac.OutputLength * _md5Hmac.OutputLength);

            if (secret.BitLength % 8 != 0)
            {
                return (null, "Invalid secret, must be whole byte");
            }

            var secretBytes = secret.BitLength / 8;
            var halfSecretLen = secretBytes.CeilingDivide(2) * 8;

            var firstHalf = secret.GetMostSignificantBits(halfSecretLen);
            var secondHalf = secret.GetLeastSignificantBits(halfSecretLen);

            var labelBytes = new BitString(Encoding.ASCII.GetBytes(label));
            var md5Out = HmacPrf(_md5Hmac, firstHalf, labelBytes.ConcatenateBits(seed), lengthToGenerate);
            var shaOut = HmacPrf(_shaHmac, secondHalf, labelBytes.ConcatenateBits(seed), lengthToGenerate);

            if (md5Out.BitLength != shaOut.BitLength)
            {
                return (null, "Invalid PRF output, lenghts must be equal");
            }

            var xorResult = md5Out.XOR(shaOut);

            return (xorResult.GetMostSignificantBits(outputLength), "");
        }

        private BitString HmacPrf(IHmac hmac, BitString secret, BitString seed, int outputLength)
        {
            var hmacLen = hmac.OutputLength;
            var numBlocks = outputLength.CeilingDivide(hmacLen);

            var A = seed.GetDeepCopy();
            var output = new BitString(0);
            for (var i = 0; i < numBlocks; i++)
            {
                A = hmac.Generate(secret, A).Mac;
                output = output.ConcatenateBits(hmac.Generate(secret, A.ConcatenateBits(seed)).Mac);
            }

            return output;
        }
    }
}
