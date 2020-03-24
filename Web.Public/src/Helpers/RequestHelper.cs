using System.IO;

namespace Web.Public.Helpers
{
    public static class RequestHelper
    {
        public static string GetJsonFromBody(Stream body)
        {
            var request = body;
            request.Seek(0, SeekOrigin.Begin);
            return new StreamReader(request).ReadToEnd();
        }
    }
}