using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class MacParametersBuilder
    {
        private KeyAgreementMacType _keyAgreementMacType;
        private int _macLength;
        private BitString _aesCcmNonce;

        public MacParametersBuilder WithKeyAgreementMacType(KeyAgreementMacType value)
        {
            _keyAgreementMacType = value;
            return this;
        }

        public MacParametersBuilder WithMacLength(int value)
        {
            _macLength = value;
            return this;
        }

        public MacParametersBuilder WithNonce(BitString value)
        {
            _aesCcmNonce = value;
            return this;
        }

        public MacParameters Build()
        {
            if (_keyAgreementMacType == KeyAgreementMacType.AesCcm)
            {
                return new MacParameters(_keyAgreementMacType, _macLength, _aesCcmNonce);
            }

            return new MacParameters(_keyAgreementMacType, _macLength);
        }
    }
}
