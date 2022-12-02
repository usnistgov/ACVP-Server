using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    public interface IKdfMultiExpansionVisitor
    {
        KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterHkdf param);
        KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterTwoStep param);
    }
}
