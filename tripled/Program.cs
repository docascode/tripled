using CommandLine;
using System;
using System.Collections.Generic;
using tripled.Kernel;
using tripled.Models;

namespace tripled
{
    class Program
    {
        static void Main(string[] args)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine(string.Format("Document De-Duper | Build Version: {0}", version));

            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(options =>
            {
                var analyzer = new Analyzer();
                if (!string.IsNullOrWhiteSpace(options.XmlPath))
                {
                    // We have the XML path, so we can proceed.
                    // NOTE: There is no need to error out if there is no XML path specified because
                    // CommandLineParser will automatically throw an error.

                    List<string> filesToAnalyze = new List<string>(analyzer.GetFilesToAnalyze(options.XmlPath));

                }
            });
            
            Console.ReadKey();
        }
    }
}
