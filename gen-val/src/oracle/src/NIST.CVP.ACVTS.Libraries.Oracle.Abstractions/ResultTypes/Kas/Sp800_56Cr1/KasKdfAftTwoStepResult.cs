using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1
{
    public class KdaAftTwoStepResult
    {
        public KdfParameterTwoStep KdfInputs { get; set; }
        public PartyFixedInfo FixedInfoPartyU { get; set; }
        public PartyFixedInfo FixedInfoPartyV { get; set; }
        public BitString DerivedKeyingMaterial { get; set; }

        public KdfMultiExpansionParameterTwoStep MultiExpansionInputs { get; set; }
        public KdfMultiExpansionResult MultiExpansionResult { get; set; }
    }
}
