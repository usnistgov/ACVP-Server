namespace NIST.CVP.Pools.Interfaces
{
    /// <summary>
    /// Interface for converting to and from Bson
    /// </summary>
    public interface IBsonConverter
    {
        string ToBson<T>(T t);
        T FromBson<T>(string dataEncBase64);
    }
}