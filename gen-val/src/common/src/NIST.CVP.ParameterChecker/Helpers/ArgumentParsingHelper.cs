using System;
using System.Collections.Generic;
using System.Text;
using CommandLineParser.Exceptions;
using NIST.CVP.ParameterChecker.Models;

namespace NIST.CVP.ParameterChecker.Helpers
{
    public class ArgumentParsingHelper
    {
        private readonly CommandLineParser.CommandLineParser _parser;

        public ArgumentParsingHelper()
        {
            _parser = new CommandLineParser.CommandLineParser();
        }

        public ArgumentParsingTarget Parse(string[] args)
        {
            var parsedParameters = new ArgumentParsingTarget();
            _parser.ExtractArgumentAttributes(parsedParameters);
            _parser.ParseCommandLine(args);

            return parsedParameters;
        }

        public void ShowUsage()
        {
            _parser.ShowUsage();
        }
    }
}
