using System.Collections.Generic;

namespace Web.Public.JsonObjects
{
    public class MultiLoginObject
    {
        public List<string> AccessToken { get; set; }
        public bool LargeEndpointRequired => false;
        public int SizeConstraint => -1;
    }
}