using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1
{
    public class KdaValTwoStepResult
    {
        public KdfParameterTwoStep KdfInputs { get; set; }
        public PartyFixedInfo FixedInfoPartyU { get; set; }
        public PartyFixedInfo FixedInfoPartyV { get; set; }
        public BitString DerivedKeyingMaterial { get; set; }
        public bool TestPassed { get; set; }

        public KdfMultiExpansionParameterTwoStep MultiExpansionInputs { get; set; }
        public KdfMultiExpansionResult MultiExpansionResult { get; set; }
    }
}
