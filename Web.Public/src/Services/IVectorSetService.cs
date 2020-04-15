using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IVectorSetService
    {
        VectorSet GetPrompt(long vsID);
        VectorSet GetExpectedResults(long vsID);
        VectorSet GetValidation(long vsID);
    }
}