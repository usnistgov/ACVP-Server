using System;
using System.Diagnostics;
using System.Linq;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Crypto.DRBG.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG
{
    public abstract class DrbgCounterBase : DrbgBase
    {
        protected BitString V = null;
        protected BitString Key = null;
        protected DrbgCounterAttributes CounterAttributes;

        protected DrbgCounterBase(IEntropyProvider entropyProvider, DrbgParameters drbgParameters) : base(entropyProvider, drbgParameters)
        {
            CounterAttributes = DrbgAttributesHelper.GetCounterDrbgAttributes(drbgParameters.Mode);
        }

        protected override DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString)
        {
            if (DrbgParameters.DerFuncEnabled)
            {
                return InstantiateDf(entropyInput, nonce, personalizationString);
            }
            else
            {
                return InstantiateNoDf(entropyInput, personalizationString);
            }
        }

        protected override DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput)
        {
            BitString seedMaterial;
            additionalInput = additionalInput.GetDeepCopy();

            if (!DrbgParameters.DerFuncEnabled)
            {
                // 1. temp = len(additional_input)
                int temp = additionalInput.BitLength;

                // 2. If (temp < seedlen) then additional_input =
                // additional_input || 0^(seedlen - temp)
                if (temp < CounterAttributes.SeedLength)
                {
                    additionalInput = additionalInput
                        .ConcatenateBits(new BitString(CounterAttributes.SeedLength - temp)); // Concatenate with bitstring made up of 0s;
                }

                if (additionalInput.BitLength != CounterAttributes.SeedLength)
                {
                    ThisLogger.Debug($"{nameof(additionalInput.BitLength)} != {nameof(CounterAttributes.SeedLength)}");
                    return DrbgStatus.Error;
                }

                // 3. seed_material = entropy_input xor additional_input
                seedMaterial = entropyInput.XOR(additionalInput);
            }
            else
            {
                // 1. seed_material = entropy_input || additional_input
                seedMaterial = entropyInput.ConcatenateBits(additionalInput);

                // 2. seed_material = Block_Cipher_df(seed_material, seedlen)
                var blockCipherDf = BlockCipherDf(seedMaterial, CounterAttributes.SeedLength);
                if (blockCipherDf.Success)
                {
                    seedMaterial = blockCipherDf.Bits;
                }
                else
                {
                    ThisLogger.Debug("blockCipherDf");
                    return DrbgStatus.Error;
                }
                
            }

            // 3./4. (Key, V) = Update(seed_material, Key, V)
            Update(seedMaterial);

            // 4./5. reseed_counter = 1
            ReseedCounter = 1;

            // 5./6. Return V, Key, and reseed_counter as the new_working_state
            // NOTE: m_V, m_Key, and m_reseed_counter hold the new working state
            return DrbgStatus.Success;
        }

        protected override DrbgResult GenerateAlgorithm(int requestedNumberOfBits, BitString additionalInput)
        {
            if (DrbgParameters.DerFuncEnabled)
            {
                return GenerateAlgorithmDf(requestedNumberOfBits, additionalInput);
            }
            else
            {
                return GenerateAlgorithmNoDf(requestedNumberOfBits, additionalInput);
            }
        }
        
        protected abstract BitString BlockEncrypt(BitString K, BitString X);
        
        private DrbgStatus InstantiateDf(BitString entropyInput, BitString nonce, BitString personalizationString)
        {
            Debug.Assert(personalizationString.BitLength <= Attributes.MaxPersonalizationStringLength);

            // 1. seed_material = entropy_input || nonce || personalization_string
            BitString seedMaterial = entropyInput
                .ConcatenateBits(nonce)
                .ConcatenateBits(personalizationString);

            // 2. seed_material = Block_Cipher_df(seed_material, seedlen)
            // Comment: Ensure that the length of the seed material is exactly seedlen
            // bits
            var blockCipherDf = BlockCipherDf(seedMaterial, CounterAttributes.SeedLength);
            if (blockCipherDf.Success)
            {
                seedMaterial = blockCipherDf.Bits;
            }
            else
            {
                ThisLogger.Debug("blockCipherDf");
                return DrbgStatus.Error;
            }

            Debug.Assert(seedMaterial.BitLength == CounterAttributes.SeedLength);

            // 3. Key = 0^keylen
            // Comment: keylen bits of zeros
            Key = new BitString(CounterAttributes.KeyLength);

            // 4. V = 0^outlen
            // Comment: outlen bits of zeros
            V = new BitString(CounterAttributes.OutputLength);

            // 5. (Key, V) = Update(seed_material, Key, V)
            Update(seedMaterial);

            // 6. reseed_counter = 1
            ReseedCounter = 1;

            // 7. Return V, Key, and reseed_counter as the initial_working_state
            // V, Key, ReseedCounter hold the inital working state
            return DrbgStatus.Success;
        }

        private DrbgStatus InstantiateNoDf(BitString entropyInput, BitString personalizationString)
        {
            // 1. temp = len(personalization_string)
            int temp = personalizationString.BitLength;

            // 2. If (temp < seedlen) then personalization_string =
            // personalization_string || 0^(seedlen - temp)
            if (temp < CounterAttributes.SeedLength)
            {
                personalizationString = personalizationString
                    .ConcatenateBits(new BitString(CounterAttributes.SeedLength - temp)); // Add zeroes to bitstring to make it equal the SeedLength
            }

            // 3. seed_material = entropy_input xor personalization_string
            BitString seedMaterial = entropyInput.XOR(personalizationString);

            // 4. Key = 0^keylen
            Key = new BitString(CounterAttributes.KeyLength);

            // 5. V = 0^outlen
            V = new BitString(CounterAttributes.OutputLength);

            // 6. (Key, V) = Update(seed_material, Key, V)
            // NOTE: update uses member variables m_Key and m_V
            // for input and output of Key and V
            Update(seedMaterial);

            ReseedCounter = 1;

            return DrbgStatus.Success;
        }

        private DrbgResult GenerateAlgorithmDf(int requestedNumberOfBits, BitString additionalInput)
        {
            additionalInput = additionalInput.GetDeepCopy();
            
            // 1. If reseed_counter > reseed_interval, then return an indication that
            // a reseed is required
            if (ReseedCounter > Attributes.MaxNumberOfRequestsBetweenReseeds)
            {
                return new DrbgResult(DrbgStatus.ReseedRequired);
            }

            // 2. If (additional_input != Null), then
            if (additionalInput.BitLength != 0)
            {
                // 2.1 additional_input = Block_Cipher_df(additional_input, seedlen)
                var blockCipherDf = BlockCipherDf(additionalInput, CounterAttributes.SeedLength);

                if (blockCipherDf.Success)
                {
                    additionalInput = blockCipherDf.Bits;
                }
                else
                {
                    ThisLogger.Debug("BlockCipherDf");
                    return new DrbgResult(DrbgStatus.Error);
                }

                // 2.2 (Key, V) = Update(additional_input, Key, V)
                Update(additionalInput);
            }
            else
            {
                // 2 (cont) Else additional_input = 0^seedlen
                additionalInput = new BitString(CounterAttributes.SeedLength);
            }

            // 3. temp = Null
            BitString temp = new BitString(0);

            // 4. While (len(temp) < requested_number_of_bits) do:
            while (temp.BitLength < requestedNumberOfBits)
            {
                // 4.1 V = (V + 1) mod 2^outlen
                V = V.BitStringAddition(BitString.One()).GetLeastSignificantBits(CounterAttributes.OutputLength);
                
                // 4.2 output_block = Block_Encrypt(Key, V)
                BitString outputBlock = BlockEncrypt(Key, V);
                // 4.3 temp = temp || output_block
                temp = temp.ConcatenateBits(outputBlock);
            }

            // 5. returned_bits = Leftmost requested_number_of_bits of temp
            var returnedBits = temp.GetMostSignificantBits(requestedNumberOfBits);

            // 6. (Key, V) = Update(additional_input, Key, V)
            // Comment: Update for backtracking resistance
            Update(additionalInput);

            // 7. reseed_counter = reseed_counter + 1
            ++ReseedCounter;

            // 8. Return SUCCESS and returned bits; also return Key, V and
            // reseed_counter as the new_working_state
            // NOTE: returned_bits is a function parameter passed by non-const
            // value.  m_Key, m_V, and m_reseed_counter hold the new working state
            return new DrbgResult(returnedBits);
        }

        private DrbgResult GenerateAlgorithmNoDf(int requestedNumberOfBits, BitString additionalInput)
        {
            additionalInput = additionalInput.GetDeepCopy();

            // 1. If reseed_counter > reseed_interval, then return an indication that
            // a reseed is required
            if (ReseedCounter > Attributes.MaxNumberOfRequestsBetweenReseeds)
            {
                return new DrbgResult(DrbgStatus.ReseedRequired);
            }

            // 2. If (additional_input != Null), then
            if (additionalInput.BitLength != 0)
            {
                // 2.1 temp = len(additional_input)
                int tempLen = additionalInput.BitLength;
                // 2.2 If (temp < seedlen), then
                //     additional_input = additional_input || 0^(seedlen - temp)
                if (tempLen < CounterAttributes.SeedLength)
                {
                    additionalInput = additionalInput.ConcatenateBits(new BitString(CounterAttributes.SeedLength - tempLen));
                }

                // 2.3 (Key, V) = Update(additional_input, Key, V)
                Update(additionalInput);
            }
            else
            {
                // 2 (cont) Else additional_input = 0^seedlen
                additionalInput = new BitString(CounterAttributes.SeedLength);
            }

            // 3. temp = Null
            BitString temp = new BitString(0);

            // 4. While (len(temp) < requested_number_of_bits) do:
            while (temp.BitLength < requestedNumberOfBits)
            {
                // 4.1 V = (V + 1) mod 2^outlen
                V = V
                    .BitStringAddition(BitString.One())
                    .GetLeastSignificantBits(CounterAttributes.OutputLength);
                
                // 4.2 output_block = Block_Encrypt(Key, V)
                BitString outputBlock = BlockEncrypt(Key, V);

                // 4.3 temp = temp || output_block
                temp = temp.ConcatenateBits(outputBlock);
            }

            // 5. returned_bits = Leftmost requested_number_of_bits of temp
            var returnedBits = temp.GetMostSignificantBits(requestedNumberOfBits);

            // 6. (Key, V) = Update(additional_input, Key, V)
            // Comment: Update for backtracking resistance
            Update(additionalInput);

            // 7. reseed_counter = reseed_counter + 1
            ++ReseedCounter;

            // 8. Return SUCCESS and returned bits; also return Key, V and
            // reseed_counter as the new_working_state
            // NOTE: returned_bits is a function parameter passed by non-const
            // value.  m_Key, m_V, and m_reseed_counter hold the new working state
            return new DrbgResult(returnedBits);
        }
        
        private DrbgResult BlockCipherDf(BitString seedMaterial, int numberOfBitsToReturn)
        {
            int maxNumberOfBits = 512;

            // Check that input string is a multiple of 8 bits
            if (seedMaterial.BitLength % 8 != 0)
            {
                ThisLogger.Debug($"{nameof(seedMaterial)} not mod 8");
                return new DrbgResult(DrbgStatus.Error);
            }

            // 1. If (no_of_bits_to_return > max_number_of_bits) then return
            // and ERROR_FLAG
            if (numberOfBitsToReturn > maxNumberOfBits)
            {
                ThisLogger.Debug($"{nameof(seedMaterial)} gt {nameof(maxNumberOfBits)}");
                return new DrbgResult(DrbgStatus.Error);
            }

            // 2. L = len(input_string)/8
            // Comment: L is the bitstring representation of the integer resulting
            // from len(input_string)/8. L shall be represented as a 32-bit integer.
            //BitString l = new BitString(new BigInteger(seedMaterial.BitLength / 8)).GetLeastSignificantBits(4 * 8);
            BitString l = new BitString(BitConverter.GetBytes(seedMaterial.BitLength / 8).Reverse().ToArray());
           
            // 3. N = no_of_bits_to_return/8
            // Comment: N is the bitstring representation of the integer resulting
            // from number_of_bits_to_return/8.  N shall be represented as a 32-bit
            // integer
            //BitString n = new BitString(new BigInteger(numberOfBitsToReturn / 8)).GetLeastSignificantBits(4 * 8);
            BitString n = new BitString(BitConverter.GetBytes(numberOfBitsToReturn / 8).Reverse().ToArray());

            // 3. S = L || N || input_string || 0x80
            // Comment: Prepend the string length and the requested length of the
            // output to the input_string
            // NOTE: SP800-90 has step 3 twice
            BitString s = l
                .ConcatenateBits(n)
                .ConcatenateBits(seedMaterial)
                .ConcatenateBits(new BitString("80"));

            // 4. While (len(S) mod outlen) != 0, S = S || 0x00
            // Comment: Pad S with zeros if necessary
            while ((s.BitLength % CounterAttributes.OutputLength) != 0)
            {
                s = s
                    .ConcatenateBits(BitString.Zero());
            }

            // 5. temp = the Null string
            // Comment: compute the starting value
            BitString temp = new BitString(0);

            // 6. i = 0
            // Comment: i shall be represented as a 32-bit integer, i.e., len(i) = 32
            int i = 0;

            byte[] bt = new byte[32];
            for (int iterator = 0; iterator < 32; iterator++)
            {
                bt[iterator] = (byte) iterator;
            }

            // 7. Key = leftmost keylen bits of 0x000102030405
            BitString k = new BitString(bt).GetMostSignificantBits(CounterAttributes.KeyLength);

            // 8. While len(temp)< keylen + outlen, do:
            while (temp.BitLength < (CounterAttributes.KeyLength + CounterAttributes.OutputLength))
            {
                // 8.1 IV = i || 0^(outlen - len(i))
                BitString iv = new BitString(BitConverter.GetBytes(i).Reverse().ToArray())
                    .ConcatenateBits(new BitString(CounterAttributes.OutputLength - 32));
                // 8.2 temp = temp || BCC(Key, (IV || S))
                temp = temp
                    .ConcatenateBits(BCC(k, iv.ConcatenateBits(s)));

                i++;
            }

            // 9. Key = Leftmost keylen bits of temp
            k = temp.GetMostSignificantBits(CounterAttributes.KeyLength);

            // 10. X = Next outlen bits of temp
            int istart = temp.BitLength - CounterAttributes.KeyLength - CounterAttributes.OutputLength;
            BitString x = temp.Substring(istart, CounterAttributes.OutputLength);

            // 11. temp = the Null string
            temp = new BitString(0);

            // 12. While len(temp) < number_of_bits_to_return, do:
            while (temp.BitLength < numberOfBitsToReturn)
            {
                // 12.1 X = Block_Encrypt(Key, X)
                x = BlockEncrypt(k, x);
                // 12.2 temp = temp || X
                temp = temp.ConcatenateBits(x);
            }

            // 13. requested_bits = Leftmost no_of_bits_to_return of temp
            return new DrbgResult(temp.GetMostSignificantBits(numberOfBitsToReturn));
        }

        private BitString BCC(BitString key, BitString data)
        {
            // 1. chaining_value = 0^outlen
            // Comment: set the first chaining value to outlen zeros
            BitString chainingValue = new BitString(CounterAttributes.OutputLength);

            // 2. n = len(data)/outlen
            int n = data.BitLength / CounterAttributes.OutputLength;

            // 3. Starting with the leftmost bits of data, split the data into n
            // blocks of outlen bits each forming block_1 to block_n

            // 4. For i = 1 to n do:
            int iStart = data.BitLength - CounterAttributes.OutputLength;
            for (int i = 0; i < n; ++i, iStart -= CounterAttributes.OutputLength)
            {
                // 4.1 input_block = chaining_value xor block(i)
                BitString inputBlock = chainingValue.XOR(data.Substring(iStart, CounterAttributes.OutputLength));
                // 4.2 chaining_value = Block_Encrypt(Key, input_block)
                chainingValue = BlockEncrypt(key, inputBlock);
            }

            // 5. output_block = chaining_value;
            BitString outputBlock = chainingValue;

            // 6. Return output_block
            return outputBlock;
        }

        private void Update(BitString seedMaterial)
        {
            BitString v = V.GetDeepCopy();
            BitString key = Key.GetDeepCopy();

            // 1. temp = Null
            BitString temp = new BitString(0);

            // 2. While (len(temp)<seedlen) do:
            while (temp.BitLength < CounterAttributes.SeedLength)
            {
                v = v
                    .BitStringAddition(BitString.One())
                    .ConcatenateBits(new BitString(CounterAttributes.OutputLength - v.BitLength)); // Add zeroes to bitstring to make it the length of the OutputLength

                BitString outputBlock = BlockEncrypt(key, v);

                temp = temp.ConcatenateBits(outputBlock);
            }

            // 3. temp = Leftmost seedlen bits of temp
            temp = temp.GetMostSignificantBits(CounterAttributes.SeedLength);

            // 4. temp = temp xor provided_data
            Debug.Assert(temp.BitLength == seedMaterial.BitLength);
            temp = temp.XOR(seedMaterial);

            // 5. Key = Leftmost keylen bits of temp
            key = temp.GetMostSignificantBits(CounterAttributes.KeyLength);

            // 6. V = Rightmost outlen bits of temp
            v = temp.GetLeastSignificantBits(CounterAttributes.OutputLength);

            // 7. Return new values of Key and V
            Key = key.GetDeepCopy();
            V = v.GetDeepCopy();
        }
    }
}
