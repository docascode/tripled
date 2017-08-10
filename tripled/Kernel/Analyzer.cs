using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tripled.Kernel
{
    internal class Analyzer
    {
        internal IEnumerable<string> GetFilesToAnalyze(string path)
        {
            // Set of extensions to search for.
            var extensionSet = new List<string> { ".xml" };

            // Set of names that we need to exclude from the search.
            var exclusionFileNameSet = new List<string> { "index.xml", "ns-", "_" };

            // Set of names that we need to exclude from the search.
            var exclusionFolderNameSet = new List<string> { @"\FrameworksIndex" };

            // Get files of an extension and that are not matching against the exclusion set.
            var fileCollection = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                 .Where(s => extensionSet.Contains(Path.GetExtension(s)))
                 .Where(fn => !exclusionFileNameSet.Any(Path.GetFileName(fn).StartsWith))
                 .Where(fp => !exclusionFolderNameSet.Any(Path.GetDirectoryName(fp).Contains));

            Logger.InternalLog(string.Format("Number of entries in return collection - {0}", fileCollection.Count()));

            return fileCollection;
        }
    }
}
