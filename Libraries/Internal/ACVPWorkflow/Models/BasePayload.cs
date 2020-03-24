namespace ACVPWorkflow.Models
{
	public abstract class BasePayload
	{
		public static long ParseIDFromURL(string url) => long.Parse(url.Split("/")[^1]);
		public static long? ParseNullableIDFromURL(string url) => string.IsNullOrEmpty(url) ? (long?)null : ParseIDFromURL(url);
	}
}
