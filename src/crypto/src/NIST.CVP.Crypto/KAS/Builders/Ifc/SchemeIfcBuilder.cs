using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Crypto.KAS.Scheme.Ifc;
using NIST.CVP.Math.Entropy;
using System;
using System.Linq;

namespace NIST.CVP.Crypto.KAS.Builders.Ifc
{
    public class SchemeIfcBuilder : ISchemeIfcBuilder
    {
        private SchemeParametersIfc _schemeParameters;
        private IIfcSecretKeyingMaterial _thisPartyKeyingMaterial;
        private IIfcSecretKeyingMaterialBuilder _thisPartyKeyingMaterialBuilder;
        private IFixedInfoFactory _fixedInfoFactory;
        private FixedInfoParameter _fixedInfoParameter;
        private IKdfFactory _kdfFactory;
        private IKdfParameter _kdfParameters;
        private IKtsFactory _ktsFactory;
        private KtsParameter _ktsParameters;
        private IRsaSve _rsaSve;
        private IKeyConfirmationFactory _keyConfirmationFactory;
        private MacParameters _macParameters;
        private IEntropyProvider _entropyProvider;

        private readonly IKdfVisitor _kdfVisitor;

        public SchemeIfcBuilder(IKdfVisitor kdfVisitor)
        {
            _kdfVisitor = kdfVisitor;
        }

        public ISchemeIfcBuilder WithSchemeParameters(SchemeParametersIfc value)
        {
            _schemeParameters = value;
            return this;
        }

        public ISchemeIfcBuilder WithThisPartyKeyingMaterial(IIfcSecretKeyingMaterial value)
        {
            _thisPartyKeyingMaterial = value;
            return this;
        }

        public ISchemeIfcBuilder WithThisPartyKeyingMaterialBuilder(IIfcSecretKeyingMaterialBuilder value)
        {
            _thisPartyKeyingMaterialBuilder = value;
            return this;
        }

        public ISchemeIfcBuilder WithFixedInfo(IFixedInfoFactory factory, FixedInfoParameter parameter)
        {
            _fixedInfoFactory = factory;
            _fixedInfoParameter = parameter;
            return this;
        }

        public ISchemeIfcBuilder WithKdf(IKdfFactory factory, IKdfParameter parameter)
        {
            _kdfFactory = factory;
            _kdfParameters = parameter;
            return this;
        }

        public ISchemeIfcBuilder WithKts(IKtsFactory factory, KtsParameter parameter)
        {
            _ktsFactory = factory;
            _ktsParameters = parameter;
            return this;
        }

        public ISchemeIfcBuilder WithRsaSve(IRsaSve value)
        {
            _rsaSve = value;
            return this;
        }

        public ISchemeIfcBuilder WithKeyConfirmation(IKeyConfirmationFactory factory, MacParameters parameter)
        {
            _keyConfirmationFactory = factory;
            _macParameters = parameter;
            return this;
        }

        public ISchemeIfcBuilder WithEntropyProvider(IEntropyProvider value)
        {
            _entropyProvider = value;
            return this;
        }

        public ISchemeIfc Build()
        {
            ValidateSchemeParameters();

            ISchemeIfc scheme = null;
            switch (_schemeParameters.KasAlgoAttributes.Scheme)
            {
                case IfcScheme.Kas1_basic:
                case IfcScheme.Kas1_partyV_keyConfirmation:
                    scheme = new SchemeBaseKasOneKeyPair(
                        _entropyProvider,
                        _schemeParameters,
                        _fixedInfoFactory,
                        _fixedInfoParameter,
                        _thisPartyKeyingMaterialBuilder,
                        _keyConfirmationFactory,
                        _macParameters,
                        _kdfVisitor,
                        _kdfParameters,
                        _rsaSve);
                    break;
                case IfcScheme.Kas2_basic:
                case IfcScheme.Kas2_bilateral_keyConfirmation:
                case IfcScheme.Kas2_partyU_keyConfirmation:
                case IfcScheme.Kas2_partyV_keyConfirmation:
                    scheme = new SchemeBaseKasTwoKeyPair(
                        _entropyProvider,
                        _schemeParameters,
                        _fixedInfoFactory,
                        _fixedInfoParameter,
                        _thisPartyKeyingMaterialBuilder,
                        _keyConfirmationFactory,
                        _macParameters,
                        _kdfVisitor,
                        _kdfParameters,
                        _rsaSve);
                    break;
                case IfcScheme.Kts_oaep_basic:
                case IfcScheme.Kts_oaep_partyV_keyConfirmation:
                    scheme = new SchemeKts(
                        _entropyProvider,
                        _schemeParameters,
                        _fixedInfoFactory,
                        _fixedInfoParameter,
                        _thisPartyKeyingMaterialBuilder,
                        _keyConfirmationFactory,
                        _macParameters,
                        _ktsFactory,
                        _ktsParameters);
                    break;
                default:
                    throw new ArgumentException(nameof(_schemeParameters.KasAlgoAttributes.Scheme));
            }

            if (_thisPartyKeyingMaterial != null)
            {
                scheme.ThisPartyKeyingMaterial = _thisPartyKeyingMaterial;
            }

            return scheme;
        }

        private void ValidateSchemeParameters()
        {
            if (_schemeParameters == null)
            {
                throw new ArgumentNullException(nameof(_schemeParameters));
            }
        }

        private void ValidateSecretKeyingContribution()
        {
            if (_thisPartyKeyingMaterialBuilder == null)
            {
                throw new ArgumentNullException(nameof(_thisPartyKeyingMaterialBuilder));
            }
        }

        private void ValidateFixedInfo()
        {
            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _fixedInfoFactory == null)
            {
                throw new ArgumentNullException(nameof(_fixedInfoFactory));
            }

            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _fixedInfoParameter == null)
            {
                throw new ArgumentNullException(nameof(_fixedInfoParameter));
            }
        }

        private void ValidateKdfFactory()
        {
            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _kdfFactory == null)
            {
                throw new ArgumentNullException(nameof(_kdfFactory));
            }

            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _kdfParameters == null)
            {
                throw new ArgumentNullException(nameof(_kdfParameters));
            }
        }

        private void ValidateKtsFactory()
        {
            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _ktsFactory == null)
            {
                throw new ArgumentNullException(nameof(_ktsFactory));
            }

            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _ktsParameters == null)
            {
                throw new ArgumentNullException(nameof(_ktsParameters));
            }
        }

        private void ValidateKcFactory()
        {
            if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _keyConfirmationFactory == null)
            {
                throw new ArgumentNullException(nameof(_keyConfirmationFactory));
            }

            if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(_schemeParameters.KasAlgoAttributes.Scheme) && _macParameters == null)
            {
                throw new ArgumentNullException(nameof(_keyConfirmationFactory));
            }
        }
    }
}