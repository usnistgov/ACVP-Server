using System.Text.Json.Serialization;

namespace Web.Public.Models
{
    public class VectorSet
    {
        [JsonIgnore]
        public long ID { get; set; }
    }
}