using System.IO;
using Web.Public.JsonObjects;

namespace Web.Public.Services
{
    public interface IJsonService<out T>
        where T : IJsonObject
    {
        T GetObjectFromBodyJson(string jsonBody);
        string GetJsonFromBody(Stream body);
    }
}