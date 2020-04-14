using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IVectorSetService
    {
        VectorSet GetPrompt(long tsID, long vsID);
    }
}