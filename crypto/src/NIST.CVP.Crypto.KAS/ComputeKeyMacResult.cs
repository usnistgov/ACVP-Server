using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public class ComputeKeyMacResult
    {
        public ComputeKeyMacResult(BitString macData, BitString mac)
        {
            MacData = macData;
            Mac = mac;
        }

        public ComputeKeyMacResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// The data in which to generate a MAC
        /// </summary>
        public BitString MacData { get; }
        /// <summary>
        /// The computed MAC
        /// </summary>
        public BitString Mac { get; }
        /// <summary>
        /// Was the generation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string ErrorMessage { get; }
    }
}