using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IVectorSetProvider
    {
        long GetNextVectorSetID(long tsID, string token);
        VectorSet GetJson(long vsID, JsonFileType fileType);
        VectorSetStatus CheckStatus(long vsID);
    }
}