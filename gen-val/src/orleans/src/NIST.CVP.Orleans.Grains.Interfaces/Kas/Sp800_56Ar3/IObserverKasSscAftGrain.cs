using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3
{
	public interface IObserverKasSscAftGrain : IGrainWithGuidKey, IGrainObservable<KasSscAftResult>
	{
		Task<bool> BeginWorkAsync(KasSscAftParameters param);
	}
}