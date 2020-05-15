using System.Text.Json;

namespace Web.Public.Models
{
    public class VectorSet
    {
        public string JsonContent { get; set; }

        public object Content => JsonSerializer.Deserialize<object>(JsonContent);
    }
}