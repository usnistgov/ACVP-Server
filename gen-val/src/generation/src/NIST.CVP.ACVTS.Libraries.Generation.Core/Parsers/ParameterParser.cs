using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math.JsonConverters;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers
{
    public class ParameterParser<TParameters> : IParameterParser<TParameters>
        where TParameters : IParameters
    {

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        public ParseResponse<TParameters> Parse(string contents)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<TParameters>(
                    contents,
                    new JsonSerializerSettings
                    {
                        Converters = _jsonConverters
                    }
                );
                return new ParseResponse<TParameters>(parameters);
            }
            catch (Exception ex)
            {
                return new ParseResponse<TParameters>(ex.Message);
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

    }
}
