using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public class KasFactory : IKasFactory
    {
        private readonly ISchemeFactory _schemeFactory;

        public KasFactory(ISchemeFactory schemeFactory)
        {
            _schemeFactory = schemeFactory;
        }
        
        public IKas GetInstance(KasParametersComponentOnly kasParameters)
        {
            var scheme = _schemeFactory.GetInstance(
                new SchemeParameters(
                    kasParameters.KeyAgreementRole,
                    KasMode.ComponentOnly,
                    kasParameters.Scheme,
                    KeyConfirmationRole.None, // Key confirmation not performed
                    KeyConfirmationDirection.None, // Key confirmation not performed
                    kasParameters.ParameterSet,
                    kasParameters.Assurances,
                    kasParameters.PartyId
                ),
                null, // No KDF for Component only
                null // Component only does not use a MAC
            );

            return new Kas(scheme);
        }

        public IKas GetInstance(KasParametersNoKeyConfirmation kasParameters)
        {
            var scheme = _schemeFactory.GetInstance(
                new SchemeParameters(
                    kasParameters.KeyAgreementRole,
                    KasMode.NoKeyConfirmation,
                    kasParameters.Scheme,
                    KeyConfirmationRole.None, // Key confirmation not performed
                    KeyConfirmationDirection.None, // Key confirmation not performed
                    kasParameters.ParameterSet,
                    kasParameters.Assurances,
                    kasParameters.PartyId
                ),
                kasParameters.KdfParameters,
                kasParameters.MacParameters 
            );

            return new Kas(scheme);
        }

        public IKas GetInstance(KasParametersKeyConfirmation kasParameters)
        {
            var scheme = _schemeFactory.GetInstance(
                new SchemeParameters(
                    kasParameters.KeyAgreementRole,
                    KasMode.KeyConfirmation,
                    kasParameters.Scheme,
                    kasParameters.KeyConfirmationRole,
                    kasParameters.KeyConfirmationDirection,
                    kasParameters.ParameterSet,
                    kasParameters.Assurances,
                    kasParameters.PartyId
                ),
                kasParameters.KdfParameters,
                kasParameters.MacParameters
            );

            return new Kas(scheme);
        }
    }
}
