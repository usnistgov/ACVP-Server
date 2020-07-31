using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.TLS
{
	public class TlsKdfV13FullResult
	{
		public TlsKdfV13EarlySecretResult EarlySecretResult { get; set; }
		public TlsKdfV13HandshakeSecretResult HandshakeSecretResult { get; set; }
		public TlsKdfV13MasterSecretResult MasterSecretResult { get; set; }
	}
	public class TlsKdfV13EarlySecretResult
	{
		public BitString EarlySecret { get; set; }
		public BitString BinderKey { get; set; }
		public BitString ClientEarlyTrafficSecret { get; set; }
		public BitString EarlyExporterMasterSecret { get; set; }
		public BitString DerivedEarlySecret { get; set; }
	}
	public class TlsKdfV13HandshakeSecretResult
	{
		public BitString HandshakeSecret { get; set; }
		public BitString ClientHandshakeTrafficSecret { get; set; }
		public BitString ServerHandshakeTrafficSecret { get; set; }
		public BitString DerivedHandshakeSecret { get; set; }
	}
	public class TlsKdfV13MasterSecretResult
	{
		public BitString MasterSecret { get; set; }
		public BitString ClientApplicationTrafficSecret { get; set; }
		public BitString ServerApplicationTrafficSecret { get; set; }
		public BitString ExporterMasterSecret { get; set; }
		public BitString ResumptionMasterSecret { get; set; }
	}
}