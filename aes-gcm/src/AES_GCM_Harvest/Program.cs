using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_GCM;

namespace AES_GCM_Harvest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var finder = new AlgoFileFinder();
            var count = finder.CopyFoundFilesToTargetDirectory(@"C:\ConcludeFileStorage", "resp",
                @"C:\ACAVPTestFiles\AES_GCM");

            Console.WriteLine($"Count: {count}");
            Console.ReadLine();
        }
    }
}
