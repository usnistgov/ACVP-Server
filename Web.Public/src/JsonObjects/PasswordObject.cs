namespace Web.Public.JsonObjects
{
    public class PasswordObject : IJsonObject
    {
        public string AcvVersion { get; set; }
        public string Password { get; set; }
    }
}