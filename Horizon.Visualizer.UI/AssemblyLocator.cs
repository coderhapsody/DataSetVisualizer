using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Allegro.Visualizer
{
    public class AssemblyLocator
    {
        private static Lazy<AssemblyLocator> instance = new Lazy<AssemblyLocator>(() => new AssemblyLocator());
        private Dictionary<string, string> assemblies;

        private AssemblyLocator()
        {
            assemblies = new Dictionary<string, string>();
            string[] lines;
            var searchPath = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);           
            lines = File.ReadAllLines(searchPath.DirectoryName + "\\allegro.ini");

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line) || !line.Contains("="))
                {
                    // NOTE:  Loop back to top
                    continue;
                }

                try
                {
                    var items = line.Split('=');
                    assemblies.Add(items[0], items[1]);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Error to parse allegro.ini at line {i} : {lines[i]}");
                }
            }
        }

        public static AssemblyLocator Instance => instance.Value;

        public Dictionary<string, string> Assemblies => assemblies;
    }
}
