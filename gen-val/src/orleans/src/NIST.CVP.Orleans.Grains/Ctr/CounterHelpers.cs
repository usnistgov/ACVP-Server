using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Ctr
{
    public static class CounterHelpers
    {
        public static AesResult GetDeferredCounterTest(CounterParameters<AesParameters> param, IEntropyProvider entropyProvider)
        {
            var iv = GetStartingIv(param.Overflow, param.Incremental, entropyProvider, 128);

            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = entropyProvider.GetEntropy(param.Parameters.DataLength);
            var key = entropyProvider.GetEntropy(param.Parameters.KeyLength);

            return new AesResult
            {
                Key = key,
                Iv = iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : null
            };
        }

        public static TdesResult GetDeferredCounterTest(CounterParameters<TdesParameters> param, IEntropyProvider entropyProvider)
        {
            var iv = GetStartingIv(param.Overflow, param.Incremental, entropyProvider, 64);

            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = entropyProvider.GetEntropy(param.Parameters.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.Parameters.KeyingOption);

            return new TdesResult
            {
                Key = key,
                Iv = iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : null
            };
        }

        public static BitString GetStartingIv(bool overflow, bool incremental, IEntropyProvider entropyProvider, int ivLength)
        {
            BitString padding;

            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            //int randomBits = _isSample ? 6 : 9;
            int randomBits = 6;

            if (overflow == incremental)
            {
                padding = BitString.Ones(ivLength - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(ivLength - randomBits);
            }

            return BitString.ConcatenateBits(padding, entropyProvider.GetEntropy(randomBits));
        }
    }
}