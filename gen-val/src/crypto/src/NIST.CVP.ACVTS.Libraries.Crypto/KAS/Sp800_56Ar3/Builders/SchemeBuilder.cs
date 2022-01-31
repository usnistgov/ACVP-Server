using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Builders
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

        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _eccDh;
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _ffcDh;
        private readonly IMqv<EccDomainParameters, EccKeyPair> _eccMqv;
        private readonly IMqv<FfcDomainParameters, FfcKeyPair> _ffcMqv;

        public SchemeBuilder(
            IDiffieHellman<EccDomainParameters, EccKeyPair> eccDh,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> ffcDh,
            IMqv<EccDomainParameters, EccKeyPair> eccMqv,
            IMqv<FfcDomainParameters, FfcKeyPair> ffcMqv)
        {
            _eccDh = eccDh;
            _ffcDh = ffcDh;
            _eccMqv = eccMqv;
            _ffcMqv = ffcMqv;
        }

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

            switch (_schemeParameters.KasAlgoAttributes.KasScheme)
            {
                case KasScheme.FfcDhEphem:
                    return new SchemeFfcDiffieHellmanEphemeral(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcDh);
                case KasScheme.FfcDhHybrid1:
                    return new SchemeFfcDhHybrid1(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcDh);
                case KasScheme.FfcDhHybridOneFlow:
                    return new SchemeFfcDhHybridOneFlow(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcDh);
                case KasScheme.FfcDhOneFlow:
                    return new SchemeFfcDhOneFlow(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcDh);
                case KasScheme.FfcMqv1:
                    return new SchemeFfcMqv1(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcMqv);
                case KasScheme.FfcMqv2:
                    return new SchemeFfcMqv2(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcMqv);
                case KasScheme.FfcDhStatic:
                    return new SchemeFfcDhStatic(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _ffcDh);


                case KasScheme.EccEphemeralUnified:
                    return new SchemeEccEphemeralUnified(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccDh);
                case KasScheme.EccFullMqv:
                    return new SchemeEccFullMqv(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccMqv);
                case KasScheme.EccFullUnified:
                    return new SchemeEccFullUnified(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccDh);
                case KasScheme.EccOnePassDh:
                    return new SchemeEccOnePassDh(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccDh);
                case KasScheme.EccOnePassMqv:
                    return new SchemeEccOnePassMqv(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccMqv);
                case KasScheme.EccOnePassUnified:
                    return new SchemeEccOnePassUnified(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccDh);
                case KasScheme.EccStaticUnified:
                    return new SchemeEccStaticUnified(_schemeParameters, _thisPartyKeyingMaterial, _fixedInfoFactory,
                        _fixedInfoParameter, _kdfFactory, _kdfParameter, _keyConfirmationFactory,
                        _keyConfirmationParameter, _eccDh);

                default:
                    throw new ArgumentException("Invalid scheme provided to builder.");
            }
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
            if (new[] { KasMode.KdfKc, KasMode.KdfNoKc }.Contains(_schemeParameters.KasMode))
            {
                if (_fixedInfoFactory == null || _fixedInfoParameter == null)
                {
                    throw new ArgumentException("KAS configuration specified FixedInfo, but FixedInfo information was not provided to the builder.");
                }
            }
        }

        private void ValidateKdf()
        {
            if (new[] { KasMode.KdfKc, KasMode.KdfNoKc }.Contains(_schemeParameters.KasMode))
            {
                if (_kdfParameter == null)
                {
                    throw new ArgumentException("KAS configuration specified KDF, but KDF information was not provided to the builder.");
                }
            }
        }

        private void ValidateKeyConfirmation()
        {
            if (new[] { KasMode.KdfKc, KasMode.NoKdfKc }.Contains(_schemeParameters.KasMode))
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
