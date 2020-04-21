namespace NIST.CVP.Libraries.Internal.ACVPCore
{
	public enum ImplementationType
	{
		Software = 0,
		Hardware = 1,
		Firmware = 2,
		Unknown = 3
	}

	public static class ImplementationTypeExtensions
	{
		public static ImplementationType FromString(string value)
		{
			return value?.ToLower() switch
			{
				"software" => ImplementationType.Software,
				"hardware" => ImplementationType.Hardware,
				"firmware" => ImplementationType.Firmware,
				_ => ImplementationType.Unknown
			};
		}
	}
}
