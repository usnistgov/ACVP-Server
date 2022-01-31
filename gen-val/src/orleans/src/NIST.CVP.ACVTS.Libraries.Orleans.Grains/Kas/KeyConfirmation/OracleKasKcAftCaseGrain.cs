using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.KeyConfirmation;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.KeyConfirmation
{
    public class OracleKasKcAftCaseGrain : ObservableOracleGrainBase<KasKcAftResult>, IOracleKasKcAftCaseGrain
    {
        private const int IdLength = 128;
        private readonly ILogger<OracleKasKcAftCaseGrain> _logger;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _random;

        private KasKcAftParameters _param;

        public OracleKasKcAftCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, ILogger<OracleKasKcAftCaseGrain> logger, IKeyConfirmationFactory keyConfirmationFactory, IEntropyProvider entropyProvider, IRandom800_90 random) : base(nonOrleansScheduler)
        {
            _logger = logger;
            _keyConfirmationFactory = keyConfirmationFactory;
            _entropyProvider = entropyProvider;
            _random = random;
        }

        public async Task<bool> BeginWorkAsync(KasKcAftParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var iutId = _entropyProvider.GetEntropy(IdLength);
                var serverId = _entropyProvider.GetEntropy(IdLength);

                var iutEphemeralData = IncludeEphemeralData() ? _entropyProvider.GetEntropy(_param.KeyLen) : null;
                var serverEphemeralData = IncludeEphemeralData() ? _entropyProvider.GetEntropy(_param.KeyLen) : null;

                IEntropyProvider entropyProvider;
                switch (_param.Disposition)
                {
                    case KasKcDisposition.Success:
                        entropyProvider = _entropyProvider;
                        break;
                    case KasKcDisposition.LeadingZeroNibbleKey:
                        entropyProvider = new EntropyProviderLeadingZeroes(_random, 4);
                        break;
                    case KasKcDisposition.LeadingZeroByteKey:
                        entropyProvider = new EntropyProviderLeadingZeroes(_random, 8);
                        break;
                    case KasKcDisposition.LeadingOneBitKey:
                        entropyProvider = new EntropyProviderLeadingOnes(_random, 1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var macKey = entropyProvider.GetEntropy(_param.KeyLen);

                var keyConf = _keyConfirmationFactory.GetInstance(new KeyConfirmationParameters(
                    _param.KasRole, _param.KeyConfirmationRole, _param.KeyConfirmationDirection, _param.KeyAgreementMacType,
                    _param.KeyLen, _param.MacLen,
                    iutId, serverId,
                    iutEphemeralData, serverEphemeralData,
                    macKey
                ));

                var result = keyConf.ComputeMac();

                await Notify(new KasKcAftResult()
                {
                    IutMacDataContribution = new PartyFixedInfo(iutId, iutEphemeralData),
                    ServerMacDataContribution = new PartyFixedInfo(serverId, serverEphemeralData),

                    MacKey = macKey,
                    MacData = result.MacData,
                    Tag = result.Mac
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                await Throw(e);
            }
        }

        private bool IncludeEphemeralData()
        {
            return _entropyProvider.GetEntropy(1).Bits[0];
        }
    }
}
