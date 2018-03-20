using System;
using System.Collections.Generic;
using System.Text;
using CommandLineParser.Exceptions;
using NIST.CVP.Generation.GenValApp.Models;

namespace NIST.CVP.Generation.GenValApp.Helpers
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
            args = GetArgsWhenNotProvided(args);
            _parser.ParseCommandLine(args);

            return parsedParameters;
        }

        public void ShowUsage()
        {
            _parser.ShowUsage();
        }

        /// <summary>
        /// Allows input of arguments when not provided at invocation.
        /// Note this is not "exactly" the same as the entry, as the parsing it done by
        /// splitting on " ". Will not work as expected on(as example)
        /// files with spaces in the name
        /// 
        /// Using this shouldn't be the "normal flow" of application use.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string[] GetArgsWhenNotProvided(string[] args)
        {
            if (args.Length != 0)
            {
                return args;
            }

            _parser.ShowUsage();
            Console.WriteLine("cmd arguments were not provided, please provide them below:\n");
            var argsInput = Console.ReadLine();
            args = string.IsNullOrEmpty(argsInput) ? new string[0] : argsInput.Split(' ');

            return args;
        }
    }
}
