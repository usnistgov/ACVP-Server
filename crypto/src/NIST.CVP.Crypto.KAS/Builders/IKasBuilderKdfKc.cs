using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfKc
    {
        IKas Build();
        IKasBuilderKdfKc WithKeyConfirmationDirection(KeyConfirmationDirection value);
        IKasBuilderKdfKc WithKeyConfirmationRole(KeyConfirmationRole value);
        IKasBuilderKdfKc WithKeyLength(int value);
        IKasBuilderKdfKc WithMacParameters(MacParameters value);
        IKasBuilderKdfKc WithOtherInfoPattern(string value);
    }
}