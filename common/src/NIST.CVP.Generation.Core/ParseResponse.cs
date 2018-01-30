namespace NIST.CVP.Generation.Core
{
    public class ParseResponse<T> 
    {
        public T ParsedObject { get; }
        public string ErrorMessage { get; }

        public ParseResponse(T parsedObject)
        {
            ParsedObject = parsedObject;
        }
        public ParseResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

    }
}
