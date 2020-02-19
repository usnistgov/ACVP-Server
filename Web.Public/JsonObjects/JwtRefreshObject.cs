namespace Web.Public.JsonObjects
{
    public class JwtRefreshObject : IJsonObject
    {
        public string AcvVersion { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
    }
}