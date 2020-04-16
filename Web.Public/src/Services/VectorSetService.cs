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
            return _vectorSetProvider.CheckStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJson(vsID, JsonFileType.Prompt),
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, JsonFileType.Prompt),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, JsonFileType.Prompt),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJson(vsID, JsonFileType.Prompt),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, JsonFileType.Error),
                
                _ => null
            };
        }

        public VectorSet GetExpectedResults(long vsID)
        {
            // TODO check is sample ?
            
            return _vectorSetProvider.CheckStatus(vsID) switch
            {
                VectorSetStatus.Processed => _vectorSetProvider.GetJson(vsID, JsonFileType.ExpectedResults),
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, JsonFileType.ExpectedResults),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, JsonFileType.ExpectedResults),
                VectorSetStatus.KATReceived => _vectorSetProvider.GetJson(vsID, JsonFileType.ExpectedResults),
                
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, JsonFileType.Error),
                _ => null
            };
        }

        public VectorSet GetValidation(long vsID)
        {
            return _vectorSetProvider.CheckStatus(vsID) switch
            {
                VectorSetStatus.Passed => _vectorSetProvider.GetJson(vsID, JsonFileType.Validation),
                VectorSetStatus.Failed => _vectorSetProvider.GetJson(vsID, JsonFileType.Validation),
                VectorSetStatus.Error => _vectorSetProvider.GetJson(vsID, JsonFileType.Error),
                _ => null
            };
        }
    }
}