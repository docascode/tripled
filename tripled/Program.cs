using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
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

                if (options.EnableLogging)
                {
                    logger = new Logger();
                }

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

                        OutputLog(logger, options.EnableLogging, logEntry);

                        foreach (var file in filesToAnalyze)
                        {
                            logEntry = string.Format("Analyzing file: {0}", file);

                            OutputLog(logger, options.EnableLogging, logEntry);

                            var xml = XDocument.Load(file);

                            // We need to parse out in the following member hierarchy:
                            // Type -> Members -> Member -> MemberSignature (Language=DocId) -> Value

                            var elementSet = xml.Root.XPathSelectElements("/Type/Members/Member");
                            foreach(var element in elementSet)
                            {
                                var targetSignature = element.Descendants("MemberSignature").FirstOrDefault(el => el.Attribute("Language").Value == "DocId").Attribute("Value").Value;

                                logEntry = string.Format("De-duping signature: {0}", targetSignature);
                                OutputLog(logger, options.EnableLogging, logEntry);

                                var dupedElements = from xe
                                                    in elementSet
                                                    where xe.Descendants("MemberSignature").FirstOrDefault(el => el.Attribute("Language").Value == "DocId" &&
                                                                                                           el.Attribute("Value").Value == targetSignature) != null
                                                    select xe;

                                logEntry = string.Format("Elements with matching signature: {0}", dupedElements.Count());
                                OutputLog(logger, options.EnableLogging, logEntry);

                                if (dupedElements.Count() == 1)
                                {
                                    logEntry = string.Format("{0} is CLEAN", targetSignature);
                                    OutputLog(logger, options.EnableLogging, logEntry);
                                }
                                else
                                {
                                    logEntry = string.Format("{0} is DIRTY", targetSignature);
                                    OutputLog(logger, options.EnableLogging, logEntry);

                                    var elementsToRemove = analyzer.PickLosingElements(dupedElements);
                                }
                            }

                            Logger.InternalLog(string.Format("Individual members in the type: {0}", elementSet.Count()));
                        }

                    }
                }
            });
            
            Console.ReadKey();
        }

        static void OutputLog(Logger logger, bool shouldLog, string logEntry)
        {
            if (shouldLog)
            {
                logger.Log(logEntry);
            }

            Console.WriteLine(logEntry);
        }
    }
}
