using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums
{
	public enum TlsModes1_3
	{
		None,
		[EnumMember(Value = "DHE")]
		DHE,
		[EnumMember(Value = "PSK")]
		PSK,
		[EnumMember(Value = "PSK-DHE")]
		PSK_DHE
	}
}