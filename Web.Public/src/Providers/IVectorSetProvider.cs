using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IVectorSetProvider
    {
        long GetNextVectorSetID(long tsID, string token);
        VectorSet GetJson(long vsID, VectorSetJsonFileTypes fileType);
        VectorSetStatus GetStatus(long vsID);
        void PrepareVectorSetForAnswerResubmit(long vsID);
    }
}