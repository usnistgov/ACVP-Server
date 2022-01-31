using NIST.CVP.ACVTS.Libraries.Common.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class ValidateResponse
    {
        /// <summary>
        /// Status code representing the outcome of the validation
        /// </summary>
        public StatusCode StatusCode { get; }

        /// <summary>
        /// A delimited Error response due to an error in generation.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// The json representing the validation result.
        /// </summary>
        public string ValidationResult { get; }

        // public ValidateResponse()
        // {
        //     StatusCode = StatusCode.Success;
        // }

        public ValidateResponse(string message, StatusCode statusCode)
        {
            if (statusCode == StatusCode.Success || statusCode == StatusCode.ValidatorFail)
            {
                ValidationResult = message;
            }
            else
            {
                ErrorMessage = message;
            }

            StatusCode = statusCode;
        }

        // public ValidateResponse(string validationResult)
        // {
        //     ValidationResult = validationResult;
        //     StatusCode = StatusCode.Success;
        // }
        //
        // public ValidateResponse(string errorMessage, StatusCode statusCode = StatusCode.ValidatorError)
        // {
        //     ErrorMessage = errorMessage;
        //     StatusCode = statusCode;
        // }

        public bool Success => StatusCode == StatusCode.Success || StatusCode == StatusCode.ValidatorFail;
    }
}
