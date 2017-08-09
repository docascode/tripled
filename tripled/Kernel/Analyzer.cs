using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tripled.Kernel
{
    internal class Analyzer
    {
        internal IEnumerable<string> GetFilesToAnalyze(string path)
        {
            // Set of extensions to search for.
            var extensionSet = new List<string> { "xml" };

            // Set of names that we need to exclude from the search.
            var exclusionNameSet = new List<string> { "index.xml", "ns-", "_" };

            // Get files of an extension and that are not matching against the exclusion set.
            var fileCollection = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                 .Where(s => extensionSet.Contains(Path.GetExtension(s)))
                 .Where(fn => !exclusionNameSet.Any(en => fn.StartsWith(en)));

            return fileCollection;
        }
    }
}
