using System.Diagnostics;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Crypto.DRBG
{
    public abstract class DrbgBase : IDrbg
    {
        protected const int DefaultEntropyLength = 256;
        protected IEntropyProvider EntropyProvider;
        protected DrbgParameters DrbgParameters;
        protected bool IsInstantiated = false;
        protected int ReseedCounter;

        protected DrbgAttributes Attributes;

        /// <summary>
        /// The current DRBG State
        /// </summary>
        public DrbgState DrbgState { get; } = new DrbgState();

        protected DrbgBase(IEntropyProvider entropyProvider, DrbgParameters drbgParameters)
        {
            EntropyProvider = entropyProvider;
            DrbgParameters = drbgParameters;
            Attributes = DrbgAttributesHelper.GetDrbgAttributes(drbgParameters.Mechanism, drbgParameters.Mode, drbgParameters.DerFuncEnabled);
        }

        public DrbgStatus Instantiate(int requestedSecurityStrength, BitString personalizationString)
        {
            // 1. check security strength
            if (requestedSecurityStrength > Attributes.MaxSecurityStrength)
            {
                return DrbgStatus.RequestedSecurityStrengthTooHigh;
            }

            // 3. personalization string
            if (personalizationString.BitLength > Attributes.MaxPersonalizationStringLength)
            {
                return DrbgStatus.PersonalizationStringTooLong;
            }

            // 4. Set security_strength to the nearest security strength greater
            // than or equal to requested_instantiation_security_strength

            // 5. Using security_strength, select appropriate DRBG mechanism parameters
            // Depricated Null step for preserving numbering
            
            // 6. (status, entropy_input) = Get_entropy_input( ... )
            BitString entropyInput = EntropyProvider.GetEntropy(DrbgParameters.EntropyInputLen);
            DrbgState.LastEntropy = entropyInput;

            // 7. If an ERROR is returned in step 6, return a CATASTROPHIC_ERROR_FLAG
            if (entropyInput.BitLength != DrbgParameters.EntropyInputLen)
            {
                ThisLogger.Debug($"{nameof(entropyInput)} length != {nameof(DrbgParameters.EntropyInputLen)}");
                return DrbgStatus.CatastrophicError;
            }

            // 8. Obtain a nonce
            // Comment: this step shall include any appropriate checks on the
            // acceptability of the nonce.  See Section 8.6.7 of SP 800-90
            BitString nonce = EntropyProvider.GetEntropy(DrbgParameters.NonceLen);
            DrbgState.LastNonce = nonce;

            // 9. Initial working state
            IsInstantiated = true;
            return InstantiateAlgorithm(entropyInput, nonce, personalizationString);
        }

        public DrbgStatus Reseed(BitString additionalInput)
        {
            BitString entropyInput = EntropyProvider.GetEntropy(DrbgParameters.EntropyInputLen);
            
            DrbgState.LastEntropy = entropyInput;

            return ReseedAlgorithm(entropyInput, additionalInput);
        }

        public DrbgResult Generate(int requestedNumberOfBits, BitString additionalInput)
        {
            BitString EmptyBitString = new BitString(0);
            additionalInput = additionalInput.GetDeepCopy();

            // 1. Using state_handle, obtain current internal state for the
            // instantiation.  If state_handle indicates an invalid or unused
            // internal state, then return an ERROR_FLAG
            // NOTE: class member m_s hold internal state

            // 2. If requested_number_of_bits > max_number_of_bits_per_request, then
            // return an ERROR_FLAG
            if (requestedNumberOfBits > Attributes.MaxNumberOfBitsPerRequest)
            {
                ThisLogger.Debug($"{nameof(requestedNumberOfBits)} > {nameof(Attributes.MaxNumberOfBitsPerRequest)}");
                return new DrbgResult(DrbgStatus.Error);
            }

            // 3. If requested_security_strength > security_strength indicated in the
            // internal state, then return an ERROR_FLAG
            // NOTE: this implementation does not support this paramter.  The securityMaxAdditionalInputLength
            // sterngth set at instantiation is what is used

            // 4. If the length of the additional_input > max_additional_input_length,
            // then return an ERROR_FLAG
            if (additionalInput.BitLength > Attributes.MaxAdditionalInputStringLength)
            {
                ThisLogger.Debug($"{nameof(additionalInput)} > {nameof(Attributes.MaxAdditionalInputStringLength)}");
                return new DrbgResult(DrbgStatus.Error);
            }

            // 5. If prediction_resistance_request is set and
            // prediction_resistance_flag is not set, then return an ERROR_FLAG
            // No request parameter is passed into generate; the flag passed into
            // instantiate is stored in a class member
            // NOTE: all implementations support prediction resistance

            // 6. Clear the reseed_required_flag
            bool reseedRequired = false;

            while (true)
            {
                // 7. If reseed_required_flag is set, or if prediction_resistance_request
                // is set, then
                if (DrbgParameters.PredResistanceEnabled || reseedRequired)
                {
                    // 7.1 status = Reseed_function(additional_input)
                    // NOTE: we have two versions of reseed; one takes an entropy
                    // input passed in as a parameter for the purposes of testing
                    // and verification
                    var reseed = Reseed(additionalInput);

                    // 7.2 If status indicates an ERROR, then return status
                    if (reseed != DrbgStatus.Success)
                    {
                        ThisLogger.Debug("Reseed failed");
                        return new DrbgResult(reseed);
                    }

                    // 7.3 Using state_handle, obtain the new internal state
                    // NOTE: internal state in class member m_s, updated in
                    // member function reseed
                    // 7.4 additional_input = the Null string
                    additionalInput = EmptyBitString.GetDeepCopy();
                    // 7.5 Clear the reseed_required_flag
                    reseedRequired = false;
                }

                // 8. (status, pseudorandom_bits, new_working_state) = Generate_algorithm(
                // working_state, requested_number_of_bits, additional_input)
                var result = GenerateAlgorithm(requestedNumberOfBits, additionalInput);

                // 9. If status indicates that a reseed is required before the requested
                // bits can be generated, then
                if (result.DrbgStatus == DrbgStatus.ReseedRequired)
                {
                    // 9.1 Set the reseed_required_flag
                    reseedRequired = true;
                    // 9.2 Go to step 7
                }
                else
                {
                    return result;
                }
            }
        }

        public void Uninstantiate()
        {
            ReseedCounter = 0;
            IsInstantiated = false;
        }

        //protected abstract void SetSecurityStrengths(int requestedSecurityStrength);
        protected abstract DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString);
        protected abstract DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput);
        protected abstract DrbgResult GenerateAlgorithm(int requestedNumberOfBits, BitString additionalInput);

        protected Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
