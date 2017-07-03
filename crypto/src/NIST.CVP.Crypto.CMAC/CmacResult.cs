using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacResult
    {
        public BitString ResultingMac { get; private set; }
        public string ErrorMessage { get; private set; }
        public CmacResult(BitString resultingMac)
        {
            ResultingMac = resultingMac;
        }

        public CmacResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }
            return $"Resulting MAC: {ResultingMac.ToHex()}";
        }
    }
}