using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.KeyConfirmation
{
    public record KasKcAftResult
    {
        public PartyFixedInfo IutMacDataContribution { get; set; }
        public PartyFixedInfo ServerMacDataContribution { get; set; }
        public BitString MacData { get; init; }
        public BitString MacKey { get; init; }
        public BitString Tag { get; init; }
    }
}
