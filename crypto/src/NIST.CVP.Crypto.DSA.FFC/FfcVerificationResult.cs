using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcVerificationResult : IDsaVerificationResult
    {
        /// <summary>
        /// Was the generation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public FfcVerificationResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public FfcVerificationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
