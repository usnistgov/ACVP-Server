using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Web.Public.Models;
using Web.Public.Providers;
using VectorSetStatus = Web.Public.Models.VectorSetStatus;

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
            return _vectorSetProvider.CheckStatus(vsID) switch
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
            
            return _vectorSetProvider.CheckStatus(vsID) switch
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
            return _vectorSetProvider.CheckStatus(vsID) switch
            {
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Validation),
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, VectorSetJsonFileTypes.Error),
                _ => null
            };
        }
    }
}