using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationMacDataCreator : INoKeyConfirmationMacDataCreator
    {
        private const string STANDARD_MESSAGE = "Standard Test Message";

        public BitString GetMacData(INoKeyConfirmationParameters param)
        {
            return new BitString(
                    Encoding.ASCII.GetBytes(STANDARD_MESSAGE)
                )
                .ConcatenateBits(param.Nonce);
        }
    }
}
