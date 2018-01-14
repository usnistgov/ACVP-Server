using System;
using System.Collections.Generic;
using System.Spatial;
using System.Text;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DRBG.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgHash : DrbgBase
    {
        private readonly ISha _sha;
        protected readonly DrbgHashAttributes HashAttributes;
        protected BitString V = null;
        protected BitString C = null;

        public DrbgHash(IEntropyProvider entropyProvider, ISha sha, DrbgParameters drbgParameters) : base(entropyProvider, drbgParameters)
        {
            _sha = sha;
            HashAttributes = DrbgAttributesHelper.GetHashDrbgAttriutes(drbgParameters.Mode);
        }

        protected override DrbgResult GenerateAlgorithm(int requestedNumberOfBits, BitString additionalInput)
        {
            // 1
            if (ReseedCounter > Attributes.MaxNumberOfRequestsBetweenReseeds)
            {
                return new DrbgResult(DrbgStatus.ReseedRequired);
            }

            // 2
            if (additionalInput.BitLength != 0)
            {
                // 2.1
                var dataToHash = new BitString("02")
                                        .ConcatenateBits(V)
                                        .ConcatenateBits(additionalInput);

                var w = _sha.HashMessage(dataToHash).Digest;

                // 2.2
                V = V.BitStringAddition(w).GetLeastSignificantBits(HashAttributes.SeedLength);
            }

            // 3
            var returnedBits = HashGen(V, requestedNumberOfBits);

            // 4
            var H = _sha.HashMessage(new BitString("03").ConcatenateBits(V)).Digest;

            // 5
            V = V.BitStringAddition(H);
            V = V.BitStringAddition(C);
            V = V.BitStringAddition(BitString.To32BitString(ReseedCounter));
            V = V.GetLeastSignificantBits(HashAttributes.SeedLength);

            // 6
            ReseedCounter++;

            // 7
            return new DrbgResult(returnedBits);
        }

        protected override DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString)
        {
            // 1
            var seedMaterial = entropyInput
                                .ConcatenateBits(nonce)
                                .ConcatenateBits(personalizationString);

            // 2
            var seed = Hash_Df(seedMaterial, HashAttributes.SeedLength);

            // 3
            V = seed.GetDeepCopy();

            // 4
            C = Hash_Df(BitString.Zeroes(8).ConcatenateBits(V), seed.BitLength).GetDeepCopy();

            // 5
            ReseedCounter = 1;

            // 6
            return DrbgStatus.Success;
        }

        protected override DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput)
        {
            // 1
            var seedMaterial = new BitString("01")
                                .ConcatenateBits(V)
                                .ConcatenateBits(entropyInput)
                                .ConcatenateBits(additionalInput);

            // 2
            var seed = Hash_Df(seedMaterial, HashAttributes.SeedLength);

            // 3
            V = seed.GetDeepCopy();

            // 4
            C = Hash_Df(BitString.Zeroes(8).ConcatenateBits(V), seed.BitLength).GetDeepCopy();

            // 5
            ReseedCounter = 1;

            // 6
            return DrbgStatus.Success;
        }

        private BitString Hash_Df(BitString data, int bitsToReturn)
        {
            // 0
            if (bitsToReturn > 255 * HashAttributes.OutputLength)
            {
                throw new ArgumentException("Requesting too many bits to return");
            }

            // 1
            var temp = new BitString(0);

            // 2
            // TODO ceiling divide
            var len = bitsToReturn / HashAttributes.OutputLength + (bitsToReturn % HashAttributes.OutputLength != 0 ? 1 : 0);

            // 3
            var counter = new BitString("01");

            // 4
            for (var i = 0; i < len; i++)
            {
                // 4.1
                var dataToHash = counter.ConcatenateBits(BitString.To32BitString(bitsToReturn)).ConcatenateBits(data);
                temp = temp.ConcatenateBits(_sha.HashMessage(dataToHash).Digest);

                // 4.2
                counter = counter.BitStringAddition(BitString.One());
            }

            // 5
            var requestedBits = temp.MSBSubstring(0, bitsToReturn);

            // 6
            return requestedBits;
        }

        private BitString HashGen(BitString v, int bitsToReturn)
        {
            // 1
            // TODO ceiling divide
            var m = bitsToReturn / HashAttributes.OutputLength + (bitsToReturn % HashAttributes.OutputLength != 0 ? 1 : 0);

            // 2
            var data = v.GetDeepCopy();

            // 3
            var W = new BitString(0);

            // 4
            for (var i = 0; i < m; i++)
            {
                // 4.1
                var w = _sha.HashMessage(data).Digest;

                // 4.2 
                W = W.ConcatenateBits(w);

                // 4.3
                data = data.BitStringAddition(BitString.One()).GetLeastSignificantBits(HashAttributes.SeedLength);
            }

            // 5
            var returnedBits = W.MSBSubstring(0, bitsToReturn);

            // 6
            return returnedBits;
        }
    }
}
