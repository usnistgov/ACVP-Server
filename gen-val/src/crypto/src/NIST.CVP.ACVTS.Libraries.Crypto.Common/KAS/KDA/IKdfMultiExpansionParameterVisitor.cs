using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    public interface IKdfMultiExpansionParameterVisitor
    {
        IKdfMultiExpansionParameter CreateParameter(TwoStepMultiExpansionConfiguration kdfConfiguration);
        IKdfMultiExpansionParameter CreateParameter(HkdfMultiExpansionConfiguration kdfConfiguration);
    }
}
