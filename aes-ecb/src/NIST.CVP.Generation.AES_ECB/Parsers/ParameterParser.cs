using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_ECB.Parsers
{
    public class ParameterParser : IParameterParser
    {
        public ParseResponse<Parameters> Parse(string path)
        {
            if (!File.Exists(path))
            {
                return new ParseResponse<Parameters>($"Could not find file: {path}");
            }

            try
            {
                var parameters = JsonConvert.DeserializeObject<Parameters>(File.ReadAllText(path));
                return new ParseResponse<Parameters>(parameters);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new ParseResponse<Parameters>($"Could not parse file: {path}");
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

    }
}
