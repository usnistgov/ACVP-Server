using NIST.CVP.Math;

namespace NIST.CVP.Crypto.HMAC
{
    public class HmacResult
    {
        public BitString Mac { get; private set; }
        public string ErrorMessage { get; private set; }

        public HmacResult(BitString mac)
        {
            Mac = mac;
        }

        public HmacResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }

            return $"MAC: {Mac.ToHex()}";
        }
    }
}