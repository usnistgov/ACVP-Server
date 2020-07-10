using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
	public interface IObserverTlsKdfv13Grain : IGrainWithGuidKey, IGrainObservable<TlsKdfv13Result>
	{
		Task<bool> BeginWorkAsync(TlsKdfv13Parameters param);
	}
}