using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace tripled.Kernel
{
    internal class Analyzer
    {
        /// <summary>
        /// Gets the list of files that need to be analyzed for duplicate signatures.
        /// </summary>
        /// <param name="path">Path to the root directory where the ECMAXML files are located.</param>
        /// <returns>List of file paths.</returns>
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

        /// <summary>
        /// Out of a large group of elements with the same DocID, picks those that need to be
        /// removed from the source ECMAXML.
        /// </summary>
        /// <param name="dupedElements">List of duplicate elements.</param>
        /// <returns>List of XElement instances that need to be removed from the source list.</returns>
        internal IEnumerable<XElement> PickLosingElements(IEnumerable<XElement> dupedElements)
        {
            List<KeyValuePair<XElement, int>> weightedElements = new List<KeyValuePair<XElement, int>>();
            
            foreach(var element in dupedElements)
            {
                var rawElementString = element.ToString();
                Logger.InternalLog(rawElementString);

                int score = Regex.Matches(rawElementString, "To be added").Count;

                weightedElements.Add(new KeyValuePair<XElement, int>(element, score));
            }

            weightedElements.Remove(weightedElements.OrderBy(c => c.Value).First());

            return weightedElements.Select(r => r.Key).ToList();
        }
    }
}
