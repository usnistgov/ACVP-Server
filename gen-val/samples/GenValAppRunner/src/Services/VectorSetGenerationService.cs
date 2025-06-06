using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
public interface IVectorSetGenerationService
{
    Task<GenerateResponseDTO> GenerateAsync(string registrationJson);
}

public class VectorSetGenerationService<TParameters, TTestVectorSet, TTestGroup, TTestCase> : IVectorSetGenerationService
    where TParameters : IParameters
    where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
    where TTestGroup : ITestGroup<TTestGroup, TTestCase>
    where TTestCase : ITestCase<TTestGroup, TTestCase>
{
    private readonly Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> _generator;

    public VectorSetGenerationService(Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> generator)
    {
        _generator = generator;
    }

    public async Task<GenerateResponseDTO> GenerateAsync(string registrationJson)
    {
        var request = new GenerateRequest(registrationJson);
        
        var result = await _generator.GenerateAsync(request);

        return new GenerateResponseDTO
        {
            ErrorMessage = result.ErrorMessage,
            StatusCode = result.StatusCode,
           // ExpectedResultsJson = result.ExpectedResultsJson
        };
    }
}


    public class GenerateResponseDTO
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
