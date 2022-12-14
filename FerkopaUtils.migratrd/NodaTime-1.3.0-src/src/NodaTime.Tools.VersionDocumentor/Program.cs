// Copyright 2013 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NodaTime.Tools.VersionDocumentor
{
    /// <summary>
    /// Tool to annotate an XML documentation file with the version in which each member was introduced,
    /// and whether or not it's present in the PCL build.
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Usage: NodaTime.Tools.VersionDocumentor <desktop XML file> <desktop reference XML file> <PCL XML file> <previous versions directory> <output history file>");
                Console.WriteLine("The reference XML file is generated by MRefBuilder.exe (part of Sandcastle)");
                return 1;
            }
            var desktopXml = XDocument.Load(args[0]);
            var referenceXml = XDocument.Load(args[1]);
            var currentVersion = DetermineCurrentVersion(referenceXml);
            var publicMembers = FindPublicMembers(referenceXml);
            var pclMembers = LoadMembers(args[2]);
            var versionMap = FindFirstVersions(Path.GetFileName(args[0]), args[3]);
            // Remove any information from a previous run
            desktopXml.Descendants("since").Remove();
            desktopXml.Descendants("pcl").Remove();
            foreach (var member in desktopXml.Descendants("member"))
            {
                string name = member.Attribute("name").Value;
                if (!publicMembers.Contains(name))
                {
                    continue;
                }
                member.Add(new XElement("pcl", new XAttribute("supported", pclMembers.Contains(name))));
                string firstVersion;
                if (!versionMap.TryGetValue(name, out firstVersion))
                {
                    versionMap[name] = currentVersion;
                    firstVersion = currentVersion;
                }
                member.Add(new XElement("since", firstVersion));
            }

            desktopXml.Save(args[0]);

            var memberByVersion = versionMap.GroupBy(pair => pair.Value, pair => pair.Key);
            using (var historyWriter = File.CreateText(args[4]))
            {
                foreach (var group in memberByVersion.OrderByDescending(g => g.Key))
                {
                    historyWriter.WriteLine(group.Key);
                    foreach (var name in group.OrderBy(name => name.Substring(2)))
                    {
                        historyWriter.WriteLine(name);
                    }
                    historyWriter.WriteLine();
                }
            }
            return 0;
        }

        private static string DetermineCurrentVersion(XDocument referenceXml)
        {
            return referenceXml.Descendants("type")
                .Where(t => (string)t.Attribute("api") == "T:System.Reflection.AssemblyInformationalVersionAttribute")
                .Select(t => t.Parent.Descendants("value").Single().Value)
                .Single();
        }

        private static HashSet<string> FindPublicMembers(XDocument referenceXml)
        {
            // TODO: Include protected and protected internal methods of public types.
            var ids = referenceXml.Descendants("api")
                .Where(t => (string) t.Elements("apidata").Attributes("group").Select(a => (string)a).FirstOrDefault() != "namespace")
                .Where(t => (string) t.Elements().Attributes("visibility").Single().Value == "public")
                .Select(t => (string) t.Attribute("id"));
            return new HashSet<string>(ids);
        }

        private static Dictionary<string, string> FindFirstVersions(string baseName, string directory)
        {
            // We really have files like foo.xml-1.0.0, so let's look for the - as well.
            baseName += "-";
            Dictionary<string, string> firstVersions = new Dictionary<string, string>();
            // Load the files in reverse order, so that later versions are replaced by earlier ones.
            foreach (var file in Directory.GetFiles(directory).Where(file => Path.GetFileName(file).StartsWith(baseName)).OrderByDescending(x => x))
            {
                string version = Path.GetFileName(file).Substring(baseName.Length);
                var members = LoadMembers(file);
                foreach (var member in members)
                {
                    firstVersions[member] = version;
                }
            }
            return firstVersions;
        }

        private static HashSet<string> LoadMembers(string file)
        {
            XDocument doc = XDocument.Load(file);
            var members = doc.Descendants("member").Select(m => m.Attribute("name").Value);
            return new HashSet<string>(members);
        }
    }
}
