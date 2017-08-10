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
            Logger logger = null;

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
                    if (filesToAnalyze.Count > 0)
                    {
                        string logEntry = string.Format("Detected {0} files.", filesToAnalyze.Count);

                        if (options.EnableLogging)
                        {
                            logger = new Logger();
                            logger.Log(logEntry);
                        }

                        Console.WriteLine(logEntry);
                        
                        foreach(var file in filesToAnalyze)
                        {
                            logEntry = string.Format("Analyzing file: {0}", file);

                            if(options.EnableLogging)
                            {
                                logger.Log(logEntry);
                            }

                            Console.WriteLine(logEntry);
                        }

                    }
                }
            });
            
            Console.ReadKey();
        }
    }
}
