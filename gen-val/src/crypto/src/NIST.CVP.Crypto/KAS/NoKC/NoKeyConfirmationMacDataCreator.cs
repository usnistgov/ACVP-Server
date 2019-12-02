using System.Text;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
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