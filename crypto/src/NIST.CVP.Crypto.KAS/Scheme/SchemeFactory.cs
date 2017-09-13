using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeFactory : ISchemeFactory
    {
        private readonly IDsaFfc _dsa;
        private readonly IKdfFactory _kdfFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly IOtherInfoFactory _otherInfoFactory;
        private readonly IEntropyProvider _entropyProvider;

        public SchemeFactory(
            IDsaFfc dsa,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider
        )
        {
            _dsa = dsa;
            _kdfFactory = kdfFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _otherInfoFactory = otherInfoFactory;
            _entropyProvider = entropyProvider;
        }

        public IScheme GetInstance(SchemeParameters schemeParameters, KdfParameters kdfParameters, MacParameters macParameters)
        {
            switch (schemeParameters.Scheme)
            {
                case FfcScheme.DhEphem:
                    return new SchemeDiffieHellmanEphemeral(_dsa, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, _otherInfoFactory, _entropyProvider, schemeParameters, kdfParameters, macParameters, new DiffieHellman());
                case FfcScheme.DhHybrid1:
                case FfcScheme.DhHybridOneFlow:
                case FfcScheme.DhOneFlow:
                case FfcScheme.DhStatic:
                case FfcScheme.Mqv1:
                case FfcScheme.Mqv2:
                    // TODO coming soon to a KAS near you!
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException(nameof(schemeParameters.Scheme));
            }
        }
    }
}
