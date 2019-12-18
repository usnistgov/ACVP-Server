using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Crypto.KAS.Fakes;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;
using NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3
{
    public class ObserverKasValGrain : ObservableOracleGrainBase<KasValResult>, 
        IObserverKasValGrain
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly ISecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly ISecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IKdfFactory _kdfFactory;
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _random;
        private readonly IDsaEccFactory _dsaEccFactory;
        private readonly IDsaFfcFactory _dsaFfcFactory;

        private KasValParameters _param;
        
        public ObserverKasValGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasBuilder kasBuilder,
            ISchemeBuilder schemeBuilder,
            ISecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            ISecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IKdfFactory kdfFactory,
            IKdfVisitor kdfVisitor,
            IKdfParameterVisitor kdfParameterVisitor,
            IKeyConfirmationFactory keyConfirmationFactory,
            IFixedInfoFactory fixedInfoFactory,
            IEntropyProvider entropyProvider,
            IRandom800_90 random,
            IDsaEccFactory dsaEccFactory,
            IDsaFfcFactory dsaFfcFactory
            ) : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _kdfFactory = kdfFactory;
            _kdfVisitor = kdfVisitor;
            _kdfParameterVisitor = kdfParameterVisitor;
            _keyConfirmationFactory = keyConfirmationFactory;
            _fixedInfoFactory = fixedInfoFactory;
            _entropyProvider = entropyProvider;
            _random = random;
            _dsaEccFactory = dsaEccFactory;
            _dsaFfcFactory = dsaFfcFactory;
        }

        public async Task<bool> BeginWorkAsync(KasValParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                // Certain failure conditions require multiple runs, thus the loop (rather than recursion)
                while (true)
                {
                    var testPassed = true;

                    // TODO this is yucky... may need to bring in some generics around DomainParameters and KeyPair.
                    switch (_param.KasAlgorithm)
                    {
                        case KasAlgorithm.Ecc:
                            KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                                _serverSecretKeyingMaterialBuilder, _param.ServerGenerationRequirements,
                                (EccDomainParameters) _param.DomainParameters, _dsaEccFactory, _entropyProvider,
                                _param.PartyIdServer);

                            KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                                _iutSecretKeyingMaterialBuilder, _param.IutGenerationRequirements, 
                                (EccDomainParameters) _param.DomainParameters,
                                _dsaEccFactory, _entropyProvider, _param.PartyIdIut);
                            break;
                        case KasAlgorithm.Ffc:
                            KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                                _serverSecretKeyingMaterialBuilder, _param.ServerGenerationRequirements,
                                (FfcDomainParameters) _param.DomainParameters, _dsaFfcFactory, _entropyProvider,
                                _param.PartyIdServer);
                            KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                                _iutSecretKeyingMaterialBuilder, _param.IutGenerationRequirements, 
                                (FfcDomainParameters) _param.DomainParameters,
                                _dsaFfcFactory, _entropyProvider, _param.PartyIdIut);
                            break;
                    }

                    var serverSecretKeyingMaterial = _serverSecretKeyingMaterialBuilder.Build(_param.KasScheme,
                        _param.ServerGenerationRequirements.KasMode, _param.ServerGenerationRequirements.ThisPartyKasRole,
                        _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                        _param.ServerGenerationRequirements.KeyConfirmationDirection);
                    var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder.Build(_param.KasScheme,
                        _param.IutGenerationRequirements.KasMode, _param.IutGenerationRequirements.ThisPartyKasRole,
                        _param.IutGenerationRequirements.ThisPartyKeyConfirmationRole,
                        _param.IutGenerationRequirements.KeyConfirmationDirection);

                    var kdfParameter = _param.KdfConfiguration.GetKdfParameter(_kdfParameterVisitor);

                    var fixedInfoParameter = new FixedInfoParameter
                    {
                        L = _param.L,
                        Encoding = kdfParameter.FixedInputEncoding,
                        FixedInfoPattern = kdfParameter.FixedInfoPattern,
                        Salt = kdfParameter.Salt,
                        Iv = kdfParameter.Iv,
                        Label = kdfParameter.Label,
                        Context = kdfParameter.Context,
                        AlgorithmId = kdfParameter.AlgorithmId,
                    };

                    // KDF
                    IKdfFactory kdfFactory = _kdfFactory;
                    switch (_param.Disposition)
                    {
                        case KasValTestDisposition.FailChangedZ:
                            kdfFactory = new KdfFactory(new FakeKdfVisitor_BadZ(_kdfVisitor, _random));
                            testPassed = false;
                            break;
                        case KasValTestDisposition.FailChangedDkm:
                            kdfFactory = new KdfFactory(new FakeKdfVisitor_BadDkm(_kdfVisitor, _random));
                            testPassed = false;
                            break;
                    }

                    // MAC
                    MacParameters macParam = null;
                    IKeyConfirmationFactory kcFactory = null;
                    if (_param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole != KeyConfirmationRole.None)
                    {
                        macParam = new MacParameters(_param.MacConfiguration.MacType, _param.MacConfiguration.KeyLen, _param.MacConfiguration.MacLen);

                        kcFactory = _keyConfirmationFactory;

                        if (_param.Disposition == KasValTestDisposition.FailChangedMacData)
                        {
                            kcFactory = new FakeKeyConfirmationFactory_BadMacData();
                            testPassed = false;
                        }
                    }

                    // Set up scheme for IUT
                    _schemeBuilder.WithThisPartyKeyingMaterial(iutSecretKeyingMaterial)
                        .WithKdf(kdfFactory, kdfParameter)
                        .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                        .WithKeyConfirmation(kcFactory, macParam)
                        .WithSchemeParameters(new SchemeParameters(
                            new KasAlgoAttributes(_param.KasScheme),
                            _param.IutGenerationRequirements.ThisPartyKasRole, 
                            _param.IutGenerationRequirements.KasMode,
                            _param.IutGenerationRequirements.ThisPartyKeyConfirmationRole,
                            _param.IutGenerationRequirements.KeyConfirmationDirection, 
                            KasAssurance.None,
                            _param.PartyIdIut));

                    var kasIut = _kasBuilder.WithSchemeBuilder(_schemeBuilder)
                        .Build();

                    var result = kasIut.ComputeResult(serverSecretKeyingMaterial);
                    var response = new KasValResult()
                    {
                        Disposition = _param.Disposition,
                        KasResult = result,
                        KdfParameter = kdfParameter,
                        MacParameters = macParam,
                        TestPassed = testPassed,
                        IutSecretKeyingMaterial = iutSecretKeyingMaterial,
                        ServerSecretKeyingMaterial = serverSecretKeyingMaterial
                    };

                    // Post run failure checks/setup
                    switch (_param.Disposition)
                    {
                        case KasValTestDisposition.FailKeyConfirmationBits:
                        {
                            var newDkm = result.MacKey.ConcatenateBits(result.Dkm);

                            response.KasResult = new KeyAgreementResult(
                                result.SecretKeyingMaterialPartyU, 
                                result.SecretKeyingMaterialPartyV, 
                                result.Z, 
                                result.FixedInfo, 
                                newDkm, 
                                result.MacKey, 
                                result.MacData, 
                                result.Tag);
                            response.TestPassed = false;
                            break;
                        }
                        // check for successful conditions w/ constraints.
                        case KasValTestDisposition.SuccessLeadingZeroNibbleZ when result.Z[0] >= 0x10:
                        case KasValTestDisposition.SuccessLeadingZeroNibbleDkm when result.Dkm[0] >= 0x10:
                            // generate again, until getting to a zero nibble
                            continue;
                    }

                    await Notify(response);
                    break;
                }
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}