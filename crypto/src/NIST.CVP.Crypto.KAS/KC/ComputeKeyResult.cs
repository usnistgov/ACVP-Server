using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class ComputeKeyResult
    {
        public ComputeKeyResult(BitString macData, BitString mac)
        {
            MacData = macData;
            Mac = mac;
        }

        public ComputeKeyResult(string errorMessage)
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