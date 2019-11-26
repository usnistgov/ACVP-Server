using System;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Builders
{
    public class SchemeBuilder : ISchemeBuilder
    {
        private SchemeParameters _schemeParameters;
        private ISecretKeyingMaterial _thisPartyKeyingMaterial;
        private IFixedInfoFactory _fixedInfoFactory;
        private FixedInfoParameter _fixedInfoParameter;
        private IKdfFactory _kdfFactory;
        private IKdfParameter _kdfParameter;
        private IKeyConfirmationFactory _keyConfirmationFactory;
        private MacParameters _keyConfirmationParameter;

        public ISchemeBuilder WithSchemeParameters(SchemeParameters value)
        {
            _schemeParameters = value;
            return this;
        }

        public ISchemeBuilder WithThisPartyKeyingMaterial(ISecretKeyingMaterial value)
        {
            _thisPartyKeyingMaterial = value;
            return this;
        }

        public ISchemeBuilder WithFixedInfo(IFixedInfoFactory factory, FixedInfoParameter parameter)
        {
            _fixedInfoFactory = factory;
            _fixedInfoParameter = parameter;
            return this;
        }

        public ISchemeBuilder WithKdf(IKdfFactory factory, IKdfParameter parameter)
        {
            _kdfFactory = factory;
            _kdfParameter = parameter;
            return this;
        }

        public ISchemeBuilder WithKeyConfirmation(IKeyConfirmationFactory factory, MacParameters parameter)
        {
            _keyConfirmationFactory = factory;
            _keyConfirmationParameter = parameter;
            return this;
        }

        public IScheme Build()
        {
            ValidateSchemeParameters();
            ValidateSecretKeyingMaterial();
            ValidateFixedInfo();
            ValidateKdf();
            ValidateKeyConfirmation();
            ValidateConsistentUnderlyingAlgorithm();
            
            throw new NotImplementedException();
        }

        private void ValidateSchemeParameters()
        {
            if (_schemeParameters == null)
                throw new ArgumentException("Scheme parameters were not provided to the builder.");
        }

        private void ValidateSecretKeyingMaterial()
        {
            if (_thisPartyKeyingMaterial == null)
                throw new ArgumentException("Keying material was not provided to the builder.");
        }

        private void ValidateFixedInfo()
        {
            if (new[] {KasMode.KdfKc, KasMode.KdfNoKc}.Contains(_schemeParameters.KasMode))
            {
                if (_fixedInfoFactory == null || _fixedInfoParameter == null)
                {
                    throw new ArgumentException("KAS configuration specified FixedInfo, but FixedInfo information was not provided to the builder.");
                }
            }
        }

        private void ValidateKdf()
        {
            if (new[] {KasMode.KdfKc, KasMode.KdfNoKc}.Contains(_schemeParameters.KasMode))
            {
                if (_kdfFactory == null || _kdfParameter == null)
                {
                    throw new ArgumentException("KAS configuration specified KDF, but KDF information was not provided to the builder.");
                }
            }
        }

        private void ValidateKeyConfirmation()
        {
            if (new[] {KasMode.KdfKc, KasMode.NoKdfKc}.Contains(_schemeParameters.KasMode))
            {
                if (_keyConfirmationFactory == null || _keyConfirmationParameter == null)
                {
                    throw new ArgumentException("KAS configuration specified KeyConfirmation, but KeyConfirmation information was not provided to the builder.");
                }
            }
        }

        private void ValidateConsistentUnderlyingAlgorithm()
        {
            if (_schemeParameters.KasAlgorithm != _thisPartyKeyingMaterial.KasAlgorithm)
                throw new ArgumentException($"{nameof(_schemeParameters)} underlying {nameof(_schemeParameters.KasAlgorithm)} did not match {nameof(_thisPartyKeyingMaterial)}");
        }
    }
}