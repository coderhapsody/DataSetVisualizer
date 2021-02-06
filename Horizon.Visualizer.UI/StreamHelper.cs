using Allegro.Visualizer;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Allegro
{
    public static class StreamHelper
    {
        public static object DeserializeFromStream(MemoryStream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new PreMergeToMergedDeserializationBinder();
            stream.Seek(0, SeekOrigin.Begin);
            object objectType = formatter.Deserialize(stream);
            return objectType;
        }
    }

    public sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!assemblyName.StartsWith("Allegro"))
                return null;

            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String exeAssembly = Assembly.GetExecutingAssembly().FullName;


            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, exeAssembly));

            string assembly = assemblyName.Substring(0, assemblyName.IndexOf(','));
            string assemblyLocation = String.Empty;
            if (AssemblyLocator.Instance.Assemblies.TryGetValue(assembly, out assemblyLocation))
            {
                return Assembly.LoadFrom(assemblyLocation).GetType(typeName);
            }
            else
            {
                MessageBox.Show("Cannot find location for " + assemblyName);
                return null;
            }
        }
    }
}
