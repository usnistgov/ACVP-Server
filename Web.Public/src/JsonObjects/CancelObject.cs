namespace Web.Public.JsonObjects
{
	public class CancelObject
	{
		public string Url { get; set; }
		public string Status => "expired";
	}
}