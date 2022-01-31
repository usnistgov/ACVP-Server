using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders
{
    public class MacParametersBuilder : IMacParametersBuilder
    {
        private KeyAgreementMacType _keyAgreementMacType;
        private int _keyLength;
        private int _macLength;
        private BitString _aesCcmNonce;

        public IMacParametersBuilder WithKeyAgreementMacType(KeyAgreementMacType value)
        {
            _keyAgreementMacType = value;
            return this;
        }

        public IMacParametersBuilder WithKeyLength(int value)
        {
            _keyLength = value;
            return this;
        }

        public IMacParametersBuilder WithMacLength(int value)
        {
            _macLength = value;
            return this;
        }

        public IMacParametersBuilder WithNonce(BitString value)
        {
            _aesCcmNonce = value;
            return this;
        }

        public MacParameters Build()
        {
            if (_keyAgreementMacType == KeyAgreementMacType.AesCcm)
            {
                return new MacParameters(_keyAgreementMacType, _keyLength, _macLength, _aesCcmNonce);
            }

            return new MacParameters(_keyAgreementMacType, _keyLength, _macLength);
        }
    }
}
