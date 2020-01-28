using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public interface IMCTResult<TAlgoArrayResponse> 
        where TAlgoArrayResponse : ICryptoResult
    {
        string ErrorMessage { get; }
        List<TAlgoArrayResponse> Response { get; }
        bool Success { get; }
    }
}