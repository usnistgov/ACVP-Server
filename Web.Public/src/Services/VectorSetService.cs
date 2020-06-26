using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class VectorSetService : IVectorSetService
    {
        private readonly IVectorSetProvider _vectorSetProvider;

        public VectorSetService(IVectorSetProvider vectorSetProvider)
        {
            _vectorSetProvider = vectorSetProvider;
        }

        public VectorSet GetPrompt(long vsID)
        {
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Prompt),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Error),
                
                _ => null
            };
        }

        public VectorSet GetExpectedResults(long vsID)
        {
            // TODO check is sample ?
            
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Error),
                _ => null
            };
        }

        public VectorSet GetValidation(long vsID)
        {
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Error),
                _ => null
            };
        }

        [Obsolete]
        public Task<VectorSet> GetPromptAsync(long vsID)
        {
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.Passed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.Failed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Prompt),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Prompt),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Error),
                
                _ => Task.FromResult<VectorSet>(null)
            };
        }

        [Obsolete]
        public Task<VectorSet> GetExpectedResultsAsync(long vsID)
        {
            // TODO check is sample ?
            
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.Passed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.Failed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.ExpectedAnswers),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Error),
                _ => Task.FromResult<VectorSet>(null)
            };
        }

        [Obsolete]
        public Task<VectorSet> GetValidationAsync(long vsID)
        {
            return GetStatus(vsID) switch
            {
                VectorSetStatus.Passed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Failed => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Error => _vectorSetProvider.GetJsonAsync(vsID, VectorSetJsonFileTypes.Error),
                _ => Task.FromResult<VectorSet>(null)
            };
        }

        public VectorSetStatus GetStatus(long vectorSetID) => _vectorSetProvider.GetStatus(vectorSetID);

        public void SetStatus(long vectorSetID, VectorSetStatus status) => _vectorSetProvider.SetStatus(vectorSetID, status);
    }
}