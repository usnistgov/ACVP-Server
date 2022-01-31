using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    public interface IKdfMultiExpansionVisitor
    {
        KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterHkdf param);
        KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterTwoStep param);
    }
}
