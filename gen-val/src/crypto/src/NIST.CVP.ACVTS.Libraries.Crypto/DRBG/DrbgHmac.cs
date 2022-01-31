using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG
{
    public class DrbgHmac : DrbgBase
    {
        private readonly IHmac _hmac;
        protected DrbgHashAttributes HashAttributes;
        protected BitString V = null;
        protected BitString Key = null;

        public DrbgHmac(IEntropyProvider entropyProvider, IHmac hmac, DrbgParameters drbgParameters) : base(entropyProvider, drbgParameters)
        {
            _hmac = hmac;
            HashAttributes = DrbgAttributesHelper.GetHashDrbgAttributes(drbgParameters.Mode);
        }

        protected override DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString)
        {
            // 1
            var seedMaterial = entropyInput
                .ConcatenateBits(nonce)
                .ConcatenateBits(personalizationString);

            // 2
            Key = BitString.Zeroes(HashAttributes.OutputLength);

            // 3
            V = GetBitStringWithRepeatedBytes(0x01, HashAttributes.OutputLength);

            // 4
            Update(seedMaterial);

            // 5
            ReseedCounter = 1;

            // 6
            return DrbgStatus.Success;
        }

        protected override DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput)
        {
            // 1
            var seedMaterial = entropyInput.ConcatenateBits(additionalInput);

            // 2
            Update(seedMaterial);

            // 3
            ReseedCounter = 1;

            // 4
            return DrbgStatus.Success;
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
                Update(additionalInput);
            }

            // 3
            var temp = new BitString(0);

            // 4
            while (temp.BitLength < requestedNumberOfBits)
            {
                // 4.1
                V = _hmac.Generate(Key, V).Mac;

                // 4.2
                temp = temp.ConcatenateBits(V);
            }

            // 5
            var returnedBits = temp.GetMostSignificantBits(requestedNumberOfBits);

            // 6
            Update(additionalInput);

            // 7
            ReseedCounter++;

            // 8
            return new DrbgResult(returnedBits);
        }

        private void Update(BitString providedData)
        {
            // 1
            var dataToHmac = V.ConcatenateBits(new BitString("00")).ConcatenateBits(providedData);
            Key = _hmac.Generate(Key, dataToHmac).Mac;

            // 2
            V = _hmac.Generate(Key, V).Mac;

            // 3
            if (providedData.BitLength == 0)
            {
                return;
            }

            // 4
            dataToHmac = V.ConcatenateBits(new BitString("01")).ConcatenateBits(providedData);
            Key = _hmac.Generate(Key, dataToHmac).Mac;

            // 5
            V = _hmac.Generate(Key, V).Mac;

            // 6
            return;
        }

        private BitString GetBitStringWithRepeatedBytes(byte byteToConcatenate, int hashFunctionBlockSize)
        {
            BitString bitString = new BitString(0);

            while (bitString.BitLength < hashFunctionBlockSize)
            {
                bitString = bitString.ConcatenateBits(new BitString(new byte[] { byteToConcatenate }));
            }

            return bitString;
        }
    }
}
