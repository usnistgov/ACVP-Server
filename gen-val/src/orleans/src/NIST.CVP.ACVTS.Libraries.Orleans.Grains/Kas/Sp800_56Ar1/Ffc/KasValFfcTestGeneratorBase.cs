﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
{
    internal abstract class KasValFfcTestGeneratorBase : KasValTestGeneratorBase<
    KasValParametersFfc, KasValResultFfc,
    KasDsaAlgoAttributesFfc, FfcDomainParameters, FfcKeyPair>
    {
        protected KasValFfcTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfOneStepFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IShaFactory shaFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder, kdfFactory, noKeyConfirmationFactory, keyConfirmationFactory, shaFactory)
        {
        }

        protected override FfcDomainParameters GetDomainParameters(KasValParametersFfc param)
        {
            return new FfcDomainParameters(param.P, param.Q, param.G);
        }

        protected override void MangleKeys(KasValResultFfc result, KasValTestDisposition intendedDisposition, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas)
        {
            TestDispositionHelperFfc.MangleKeys(
                result,
                intendedDisposition,
                serverKas,
                iutKas
            );
        }

        protected override void SetResultInformationFromKasProcessing(KasValParametersFfc param, KasValResultFfc result, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, KasResult iutResult)
        {
            TestDispositionHelperFfc.SetResultInformationFromKasProcessing(
                param,
                result,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}
