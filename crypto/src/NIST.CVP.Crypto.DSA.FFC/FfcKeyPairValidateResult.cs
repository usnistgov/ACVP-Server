using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcKeyPairValidateResult : IKeyPairValidateResult
    {
        /// <summary>
        /// Was the validation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to validation attempt
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public FfcKeyPairValidateResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public FfcKeyPairValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
