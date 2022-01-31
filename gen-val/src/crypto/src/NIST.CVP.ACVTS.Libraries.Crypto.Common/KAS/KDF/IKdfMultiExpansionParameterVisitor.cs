using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    public interface IKdfMultiExpansionParameterVisitor
    {
        IKdfMultiExpansionParameter CreateParameter(TwoStepMultiExpansionConfiguration kdfConfiguration);
        IKdfMultiExpansionParameter CreateParameter(HkdfMultiExpansionConfiguration kdfConfiguration);
    }
}
