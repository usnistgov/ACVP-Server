namespace Web.Public.JsonObjects
{
    public class LoginObject
    {
        public string AccessToken { get; set; }
        public bool LargeEndpointRequired => false;
        public int SizeConstraint => -1;
    }
}