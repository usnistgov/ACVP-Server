namespace Web.Public.JsonObjects
{
    public class JwtObject : IJsonObject
    {
        public string AcvVersion { get; set; }
        public string AccessToken { get; set; }
    }
}