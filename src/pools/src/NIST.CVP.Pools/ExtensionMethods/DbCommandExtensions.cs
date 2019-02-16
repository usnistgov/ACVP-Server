using System;
using System.Data;

namespace NIST.CVP.Pools.ExtensionMethods
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a parameter by name and value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter to add.</param>
        public static void AddParameter(
            this IDbCommand command, string parameterName, object value
        )
        {
            var param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }
    }
}