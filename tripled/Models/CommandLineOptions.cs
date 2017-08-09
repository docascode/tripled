﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tripled.Models
{
    internal class CommandLineOptions
    {
        [Option('x', "xml", Required = true, HelpText = "Source of the XML files.")]
        public string XmlPath { get; set; }

        [Option('l', "log", Required = false, HelpText = "Determines whether to produce a log in the app location.")]
        public bool EnableLogging { get; set; }
    }
}
