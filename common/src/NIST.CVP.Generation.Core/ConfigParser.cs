using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace NIST.CVP.Generation.Core
{
    public class ConfigParser
    {
        public IConfigurationRoot Configuration { get; set; }
        public bool success { get; set; }

        public ConfigParser(string path)
        {
            if(string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                success = false;
                Configuration = null;
            }else
            {
                try
                {
                    // Put the success flag first so when it fails we know it is changed
                    success = true;
                    var configBuilder = new ConfigurationBuilder().AddJsonFile(path);
                    Configuration = configBuilder.Build();
                } catch
                {
                    success = false;
                    Configuration = null;
                }
            }
        }
    }
}
