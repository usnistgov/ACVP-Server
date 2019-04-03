using System.Diagnostics;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.DRBG.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private bool _derFunctionEnabled;
        private bool _predResistEnabled;
        private bool _reseedImplemented;
        private MathDomain _entropyInputLen;
        private MathDomain _nonceLen;
        private MathDomain _persoStringLen;
        private MathDomain _additionalInputLen;
        private int _returnedBitsLen;

        private DrbgMechanism _drbgMechanism;
        private DrbgMode _drbgMode;
        public int SeedLength { get; private set; }
        public int OutLength { get; private set; } = 128;
        public int KeyLength { get; private set; } = 0;
        public int SecurityStrength { get; private set; } = 0;

        /// <summary>
        /// Provides a valid (as of construction) set of parameters
        /// </summary>
        /// <param name="mechanism"></param>
        /// <param name="mode"></param>
        public ParameterBuilder(DrbgMechanism mechanism, DrbgMode mode)
        {
            SetMechanism(mechanism);
            SetMode(mode);
            SetMathRanges(mechanism, mode);
        }

        public ParameterBuilder WithAlgorithm(DrbgMechanism value)
        {
            SetMechanism(value);
            return this;
        }

        public ParameterBuilder WithMode(DrbgMode value)
        {
            SetMode(value);
            return this;
        }

        public ParameterBuilder WithDerFunctionEnabled(bool value)
        {
            _derFunctionEnabled = value;
            return this;
        }

        public ParameterBuilder WithPredResistEnabled(bool value)
        {
            _predResistEnabled = value;
            return this;
        }

        public ParameterBuilder WithReseedImplemented(bool value)
        {
            _reseedImplemented = value;
            return this;
        }

        public ParameterBuilder WithEntropyInputLen(MathDomain value)
        {
            _entropyInputLen = value;
            return this;
        }

        public ParameterBuilder WithNonceLen(MathDomain value)
        {
            _nonceLen = value;
            return this;
        }

        public ParameterBuilder WithPersoStringLen(MathDomain value)
        {
            _persoStringLen = value;
            return this;
        }

        public ParameterBuilder WithAdditionalInputLen(MathDomain value)
        {
            _additionalInputLen = value;
            return this;
        }

        public ParameterBuilder WithReturnedBitsLen(int value)
        {
            _returnedBitsLen = value;
            return this;
        }
        
        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = "1.0",
                ReseedImplemented = _reseedImplemented,
                PredResistanceEnabled = new []{_predResistEnabled},

                Capabilities = new []
                {
                    new Capability
                    {
                        Mode = _mode,
                        DerFuncEnabled = _derFunctionEnabled,
                        EntropyInputLen = _entropyInputLen,
                        NonceLen = _nonceLen,
                        PersoStringLen = _persoStringLen,
                        AdditionalInputLen = _additionalInputLen,
                        ReturnedBitsLen = _returnedBitsLen,
                    }
                }
            };
        }
        
        private void SetMode(DrbgMode mode)
        {
            _drbgMode = mode;

            switch (mode)
            {
                case DrbgMode.AES128:
                    _mode = "AES-128";
                    break;
                case DrbgMode.AES192:
                    _mode = "AES-192";
                    break;
                case DrbgMode.AES256:
                    _mode = "AES-256";
                    break;
            }
        }

        private void SetMechanism(DrbgMechanism mechanism)
        {
            _drbgMechanism = mechanism;

            switch (mechanism)
            {
                case DrbgMechanism.Counter:
                    _algorithm = "ctrDRBG";
                    break;
                default:
                    _algorithm = "invalid";
                    break;
            }
        }
        
        private void SetMathRanges(DrbgMechanism mechanism, DrbgMode mode)
        {
            switch (mode)
            {
                case DrbgMode.AES128:
                    KeyLength = 128;
                    SecurityStrength = 128;
                    break;
                case DrbgMode.AES192:
                    KeyLength = 192;
                    SecurityStrength = 192;
                    break;
                case DrbgMode.AES256:
                    KeyLength = 256;
                    SecurityStrength = 256;
                    break;
            }

            SeedLength = OutLength + KeyLength;
            
            _additionalInputLen = new MathDomain();
            _additionalInputLen.AddSegment(new ValueDomainSegment(SeedLength));

            _entropyInputLen = new MathDomain();
            _entropyInputLen.AddSegment(new ValueDomainSegment(SeedLength));

            _nonceLen = new MathDomain();
            _nonceLen.AddSegment(new ValueDomainSegment(SeedLength));

            _persoStringLen = new MathDomain();
            _persoStringLen.AddSegment(new ValueDomainSegment(SeedLength));
        }
    }
}
