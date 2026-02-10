using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using GenValAppRunner.DTO;
using Autofac;


  public interface IVectorSetService
    {
        Task<VectorSetResponse> GenerateAsync(
         Registration registration);

        Task<ValidationResponse> ValidateAsync(
         ValidationRequest request);
    }

public class VectorSetService : IVectorSetService
{
   private readonly IAlgoModeContainerRegistry _containerRegistry;

    public VectorSetService(IAlgoModeContainerRegistry containerRegistry)
    {
        _containerRegistry = containerRegistry;
    }

    public async Task<VectorSetResponse> GenerateAsync(
        Registration registration)
    {
        var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(
            registration.Algorithm,
            "",
            registration.Revision);

        using var scope = _containerRegistry.BeginScope(algoMode);
        {
        var generator = scope.Resolve<IGenerator>();

        var internalResponse = await generator.GenerateAsync(
            new GenerateRequest(JsonConvert.SerializeObject(registration)));

        return new VectorSetResponse
        {
            StatusCode = internalResponse.StatusCode,
            ErrorMessage = internalResponse.ErrorMessage,
            Result = JsonConvert.DeserializeObject<TestVectorSet>(
                internalResponse.InternalProjection)
        };
        }
    }

    public async Task<ValidationResponse> ValidateAsync(
        ValidationRequest request)
    {
        var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(
            request.Answer.Algorithm,
            "",
            request.Answer.Revision);

        using var scope = _containerRegistry.BeginScope(algoMode);
        {
        var validator = scope.Resolve<IValidator>();

        var internalResponse = await validator.ValidateAsync(
            new ValidateRequest(
                JsonConvert.SerializeObject(request.Answer),
                JsonConvert.SerializeObject(request.Expected),
                showExpected: true));

        return new ValidationResponse
        {
            StatusCode = internalResponse.StatusCode,
            ErrorMessage = internalResponse.ErrorMessage,
            Result = JsonConvert.DeserializeObject<VectorSetValidationResults>(
                internalResponse.ValidationResult)
        };
        }
    }
    
}
