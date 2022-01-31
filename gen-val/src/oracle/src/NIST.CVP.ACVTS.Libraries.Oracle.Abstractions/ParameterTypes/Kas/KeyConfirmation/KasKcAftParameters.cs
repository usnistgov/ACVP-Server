using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation
{
    public record KasKcAftParameters
    {
        public KasKcDisposition Disposition { get; init; }
        public KeyAgreementRole KasRole { get; init; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; init; }
        public KeyConfirmationRole KeyConfirmationRole { get; init; }
        public KeyAgreementMacType KeyAgreementMacType { get; init; }
        public int KeyLen { get; init; }
        public int MacLen { get; init; }
    }
}
