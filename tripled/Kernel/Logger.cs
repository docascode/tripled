using System;
using System.Diagnostics;
using System.IO;

namespace tripled.Kernel
{
    internal class Logger
    {
        internal string CurrentLogFile { get; private set; }
        internal Logger()
        {
            CurrentLogFile = string.Format("{0}.txt", DateTime.Now.Ticks);
        }

        internal static void InternalLog(string logEntry)
        {
            StackTrace stackTrace = new StackTrace();
            Debug.WriteLine(string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, logEntry));
        }

        internal void Log(string logEntry)
        {
           File.AppendAllText(CurrentLogFile, logEntry + "\n");
        }
    }
}
