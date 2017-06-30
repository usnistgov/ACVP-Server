using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.Core.Parsers
{
    public class ParameterParser<TParameters> : IParameterParser<TParameters> 
        where TParameters : IParameters
    {

        protected readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        public ParseResponse<TParameters> Parse(string path)
        {
            if (!File.Exists(path))
            {
                return new ParseResponse<TParameters>($"Could not find file: {path}");
            }

            try
            {
                var parameters = JsonConvert.DeserializeObject<TParameters>(
                    File.ReadAllText(path),
                    new JsonSerializerSettings
                    {
                        Converters = _jsonConverters
                    }
                );
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
