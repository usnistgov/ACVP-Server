using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IVectorSetProvider
    {
        long GetNextVectorSetID(long tsID, string token);
        Task<VectorSet> GetJsonAsync(long vsID, VectorSetJsonFileTypes fileType);
        VectorSetStatus GetStatus(long vsID);
        void SetStatus(long vsID, VectorSetStatus status);
    }
}