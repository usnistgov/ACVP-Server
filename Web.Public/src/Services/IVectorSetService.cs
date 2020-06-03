using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IVectorSetService
    {
        Task<VectorSet> GetPromptAsync(long vsID);
        Task<VectorSet> GetExpectedResultsAsync(long vsID);
        Task<VectorSet> GetValidationAsync(long vsID);
        VectorSetStatus GetStatus(long vectorSetID);
        void SetStatus(long vectorSetID, VectorSetStatus status);
    }
}