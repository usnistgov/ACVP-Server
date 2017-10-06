using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IMacParametersBuilder
    {
        MacParameters Build();
        IMacParametersBuilder WithKeyAgreementMacType(KeyAgreementMacType value);
        IMacParametersBuilder WithMacLength(int value);
        IMacParametersBuilder WithNonce(BitString value);
    }
}