using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Cr1;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1
{
	public interface IObserverKasKdfValHkdfCaseGrain : IGrainWithGuidKey, IGrainObservable<KasKdfValHkdfResult>
	{
		Task<bool> BeginWorkAsync(KasKdfValHkdfParameters param);
	}
}