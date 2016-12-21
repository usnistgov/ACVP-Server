using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace NIST.CVP.Generation.Core.Parsers
{
    public class ParameterParser<TParameters> : IParameterParser<TParameters> 
        where TParameters : IParameters
    {
        public ParseResponse<TParameters> Parse(string path)
        {
            if (!File.Exists(path))
            {
                return new ParseResponse<TParameters>($"Could not find file: {path}");
            }

            try
            {
                var parameters = JsonConvert.DeserializeObject<TParameters>(File.ReadAllText(path));
                return new ParseResponse<TParameters>(parameters);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new ParseResponse<TParameters>($"Could not parse file: {path}");
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

    }
}
