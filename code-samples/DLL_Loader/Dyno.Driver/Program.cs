using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using Dyno.Interfaces;

namespace Dyno.Driver
{
    class Program
    {
        private const string PLUGINS_DIR =
            @"C:\Projects\Dyno3\Dyno.Driver\bin\Debug\netcoreapp1.1\win7-x64\plugins\";
        static void Main(string[] args)
        {

            var parser = new CommandLineParser.CommandLineParser();
            //switch argument is meant for true/false logic
            var help = new SwitchArgument('h', "help", "Show usage (this message)", false);
            var capabilities = new SwitchArgument('c', "capabilities", "Show supported algorithms", false);
            var input = new ValueArgument<string>('i', "input", "Set the input");
            var algorithm = new ValueArgument<string>('a', "algorithm", "Set desired algorithm");
            //var algorithm = new EnumeratedValueArgument<string>('a', "algorithm", "Set desired algorithm", new[] { "red", "green", "blue" });

            parser.Arguments.Add(help);
            parser.Arguments.Add(capabilities);
            parser.Arguments.Add(input);
            parser.Arguments.Add(algorithm);


            try
            {
                parser.ParseCommandLine(args);

                if (help.Parsed && help.Value)
                {
                    parser.ShowUsage();
                    return;
                }
                if (capabilities.Parsed && capabilities.Value)
                {
                    ShowCapabilites();
                    return;
                }
                if (algorithm.Parsed && input.Parsed)
                {
                    ModifyString(algorithm.Value, input.Value);
                }
                else
                {
                    parser.ShowUsage();
                    return;
                }

            }
            catch (CommandLineException e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.ReadLine();
        }

        private static void ModifyString(string algorithmValue, string inputValue)
        {
            var iStringModifierType = typeof(IStringModifier);
            var dllPath = PLUGINS_DIR + $"Dyno.{algorithmValue}.dll";
            if (File.Exists(dllPath))
            {
                var ass = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                var stringModifierType = ass.GetTypes().Single(x => iStringModifierType.IsAssignableFrom(x));
                var stringModifier = (IStringModifier) Activator.CreateInstance(stringModifierType);
                Console.WriteLine(stringModifier.Modify(inputValue));
            }
            else
            {
                Console.WriteLine("Invalid algorithm specified. Use -c to show capabilites");
            }
        }

        private static void ShowCapabilites()
        {
            var iStringModifierType = typeof(IStringModifier);
            foreach (var file in Directory.GetFiles(@"C:\Projects\Dyno3\Dyno.Driver\bin\Debug\netcoreapp1.1\win7-x64\plugins", "*.dll"))
            {
                if (file.Contains("Dyno.Interfaces.dll"))
                {
                    continue;
                }
                var ass = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                var stringModifierTypes = ass.GetTypes().Where(x => iStringModifierType.IsAssignableFrom(x));
                foreach (var stringModifierType in stringModifierTypes)
                {
                    Console.WriteLine($"{stringModifierType.Namespace.Replace("Dyno.", "")}");
                }
            }
        }
    }
}