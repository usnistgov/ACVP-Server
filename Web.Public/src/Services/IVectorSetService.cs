using System;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IVectorSetService
    {
        VectorSet GetPrompt(long vsID);
        VectorSet GetExpectedResults(long vsID);
        VectorSet GetValidation(long vsID);
        [Obsolete]
        Task<VectorSet> GetPromptAsync(long vsID);
        [Obsolete]
        Task<VectorSet> GetExpectedResultsAsync(long vsID);
        [Obsolete]
        Task<VectorSet> GetValidationAsync(long vsID);
        VectorSetStatus GetStatus(long vectorSetID);
        bool GetArchived(long vectorSetID);
        void SetStatus(long vectorSetID, VectorSetStatus status);
    }
}