using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.DRBG.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DRBG
{
    public abstract class DrbgBase : IDrbg
    {
        private readonly DrbgState _drbgState = new DrbgState();
        private int _securityStrength = -1;
        private int _maxPersonalizationStringLength = -1;
        private long _maxAdditionalInputLength = -1;
        private int _minEntropyLength = -1;
        private int _maxEntropyLength = -1;
        private int _seedLength = -1;
        private int _outputLength = -1;
        private long _reseedInterval = -1;
        private long _maxNumberOfBitsPerRequest = -1;

        protected const int DefaultEntropyLength = 256;
        protected IEntropyProvider EntropyProvider;
        protected DrbgParameters DrbgParameters;
        protected bool IsInstantiated = false;
        protected int MaxSecurityStrength = 256;
        protected int ReseedCounter;

        #region Properties
        /// <summary>
        /// The current DRBG State
        /// </summary>
        public DrbgState DrbgState { get { return _drbgState; } }
        protected int SecurityStrength
        {
            get
            {
                Debug.Assert(_securityStrength > 0);
                return _securityStrength;
            }
            set { _securityStrength = value; }
        }

        protected int MaxPersonalizationStringLength
        {
            get
            {
                Debug.Assert(_maxPersonalizationStringLength > 0);
                return _maxPersonalizationStringLength;
            }
            set { _maxPersonalizationStringLength = value; }
        }

        public long MaxAdditionalInputLength
        {
            get
            {
                Debug.Assert(_maxAdditionalInputLength > 0);
                return _maxAdditionalInputLength;
            }
            set { _maxAdditionalInputLength = value; }
        }

        protected int MinEntropyLength
        {
            get
            {
                Debug.Assert(_minEntropyLength > 0);
                return _minEntropyLength;
            }
            set { _minEntropyLength = value; }
        }

        protected int MaxEntropyLength
        {
            get
            {
                Debug.Assert(_maxEntropyLength > 0);
                return _maxEntropyLength;
            }
            set { _maxEntropyLength = value; }
        }

        protected int SeedLength
        {
            get
            {
                Debug.Assert(_seedLength > 0);
                return _seedLength;
            }
            set { _seedLength = value; }
        }

        protected int OutputLength
        {
            get
            {
                Debug.Assert(_outputLength > 0);
                return _outputLength;
            }
            set { _outputLength = value; }
        }

        protected long ReseedInterval
        {
            get
            {
                Debug.Assert(_reseedInterval > 0);
                return _reseedInterval;
            }
            set { _reseedInterval = value; }
        }

        protected long MaxNumberOfBitsPerRequest
        {
            get
            {
                Debug.Assert(_maxNumberOfBitsPerRequest > 0);
                return _maxNumberOfBitsPerRequest;
            }
            set { _maxNumberOfBitsPerRequest = value; }
        }
        #endregion Properties

        protected DrbgBase(IEntropyProvider entropyProvider, DrbgParameters drbgParameters)
        {
            EntropyProvider = entropyProvider;
            DrbgParameters = drbgParameters;

            MaxPersonalizationStringLength = (1 << 24);
            MaxAdditionalInputLength = ((long)1 << 35);
            MaxNumberOfBitsPerRequest = (1 << 19);
            ReseedInterval = ((long)1 << 48);
        }

        public DrbgStatus Instantiate(int requestedSecurityStrength, BitString personalizationString)
        {
            // 1. check security strength
            if (requestedSecurityStrength > MaxSecurityStrength)
            {
                return DrbgStatus.RequestedSecurityStrengthTooHigh;
            }

            // 3. personalization string
            if (personalizationString.BitLength > MaxPersonalizationStringLength)
            {
                return DrbgStatus.PersonalizationStringTooLong;
            }

            // 4. Set security_strength to the nearest security strength greater
            // than or equal to requested_instantiation_security_strength
            SetSecurityStrengths(requestedSecurityStrength);

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
            InstantiateAlgorithm(entropyInput, nonce, personalizationString);

            IsInstantiated = true;

            return DrbgStatus.Success;
        }

        public DrbgStatus Reseed(BitString additionalInput)
        {
            BitString entropyInput = EntropyProvider.GetEntropy(DrbgParameters.EntropyInputLen);
            
            DrbgState.LastEntropy = entropyInput;

            ReseedAlgorithm(entropyInput, additionalInput);

            return DrbgStatus.Success;
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
            if (requestedNumberOfBits > MaxNumberOfBitsPerRequest)
            {
                ThisLogger.Debug($"{nameof(requestedNumberOfBits)} > {nameof(MaxNumberOfBitsPerRequest)}");
                return new DrbgResult(DrbgStatus.Error);
            }

            // 3. If requested_security_strength > security_strength indicated in the
            // internal state, then return an ERROR_FLAG
            // NOTE: this implementation does not support this paramter.  The securityMaxAdditionalInputLength
            // sterngth set at instantiation is what is used

            // 4. If the length of the additional_input > max_additional_input_length,
            // then return an ERROR_FLAG
            if (additionalInput.BitLength > MaxAdditionalInputLength)
            {
                ThisLogger.Debug($"{nameof(additionalInput)} > {nameof(MaxAdditionalInputLength)}");
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

        protected abstract void SetSecurityStrengths(int requestedSecurityStrength);
        protected abstract DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString);
        protected abstract DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput);
        protected abstract DrbgResult GenerateAlgorithm(int requestedNumberOfBits, BitString additionalInput);

        protected Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
