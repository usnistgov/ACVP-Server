using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS.Ecc
{
    internal class KasValFfcTestGeneratorNoKdfNoKc : KasValEccTestGeneratorBase
    {
        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> 
            GetKasInstance(KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasValParametersEcc param, KasValResultEcc result, BitString partyId, IKdfFactory kdfFactory, INoKeyConfirmationFactory noKeyConfirmationFactory, IKeyConfirmationFactory keyConfirmationFactory)
        {
            var shaFactory = new ShaFactory();
            EntropyProvider entropyProvider = new EntropyProvider(new Random800_90());
            var schemeBuilder = new SchemeBuilderEcc(
                new DsaEccFactory(shaFactory), new EccCurveFactory(), kdfFactory,
                keyConfirmationFactory, noKeyConfirmationFactory, new OtherInfoFactory(entropyProvider),
                entropyProvider, new DiffieHellmanEcc(), new MqvEcc());
            schemeBuilder.WithHashFunction(param.HashFunction);

            return new KasBuilderEcc(schemeBuilder)
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(
                    param.EccScheme, param.EccParameterSet, param.Curve
                ))
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}