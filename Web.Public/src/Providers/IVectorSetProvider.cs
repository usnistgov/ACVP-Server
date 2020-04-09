namespace Web.Public.Providers
{
    public interface IVectorSetProvider
    {
        long GetNextVectorSetID(long tsID, string token);
    }
}