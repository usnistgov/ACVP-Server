using System.IO;
using Web.Public.JsonObjects;

namespace Web.Public.Services
{
    public interface IJsonReaderService
    {
        T GetObjectFromBodyJson<T>(string jsonBody) where T : IJsonObject;
        string GetJsonFromBody(Stream body);
    }
}