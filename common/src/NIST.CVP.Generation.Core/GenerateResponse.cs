namespace NIST.CVP.Generation.Core
{
    public class GenerateResponse
    {
        
        public string ErrorMessage { get; }

        public GenerateResponse()
        {
            
        }
        public GenerateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
