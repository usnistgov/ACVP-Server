using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3
{
    public interface IObserverSafePrimesGroupDomainParameterGrain : IGrainWithGuidKey, IGrainObservable<FfcDomainParameters>
    {
        Task<bool> BeginWorkAsync(SafePrime param);
    }

}