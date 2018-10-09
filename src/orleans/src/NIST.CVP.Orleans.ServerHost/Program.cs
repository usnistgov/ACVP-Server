using System.ServiceProcess;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ServiceBase.Run(new OrleansServerService());
        }
    }
}
