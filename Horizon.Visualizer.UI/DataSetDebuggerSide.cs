using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Allegro.Visualizer.NetCore.DebuggeeSide;
using Horizon.Visualizer.UI;
using Microsoft.VisualStudio.DebuggerVisualizers;


[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(VisualizerObjectSource), Target = typeof(DataSet), Description = "Horizon DataSet Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(VisualizerObjectSource), Target = typeof(DataTable), Description = "Horizon DataTable Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(DataRowVisualizerObjectSource), Target = typeof(DataRow), Description = "Horizon DataRow Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(DataRowVisualizerObjectSource), Target = typeof(DataRowCollection), Description = "Horizon DataRowCollection Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(DataRowVisualizerObjectSource), Target = typeof(DataView), Description = "Horizon DataView Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(DataRowVisualizerObjectSource), Target = typeof(DataRowView), Description = "Horizon DataRowView Visualizer")]
[assembly: DebuggerVisualizer(typeof(DataSetDebuggerSide), typeof(ListVisualizerObjectSource), Target = typeof(List<>), Description = "Horizon List Visualizer")]
namespace Horizon.Visualizer.UI
{
    internal class CustomSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (assemblyName.Contains("Schema") || typeName.EndsWith("DS", StringComparison.OrdinalIgnoreCase) || (typeName.ToLower().Contains("allegro") && typeName.ToLower().Contains("schema")))
                return typeof(DataSet);
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }

    public class DataSetDebuggerSide : DialogDebuggerVisualizer
    {
        private object TryGetVisualizingObject(IVisualizerObjectProvider objectProvider)
        {
            object obj = null;
            try
            {
                obj = objectProvider.GetObject();
            }
            catch(Exception ex1) //ex1 just for debugging purpose
            {
                try
                {
                    var memoryStream = objectProvider.GetData();

                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Binder = new CustomSerializationBinder();
                    bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                    bf.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
                    obj = bf.UnsafeDeserialize(memoryStream, _ => null);
                }
                catch(Exception ex2)
                {
                    MessageBox.Show($"Cannot show Allegro .NET Core Visualizer\n\n{ex2.Message}", "Allegro .NET Core Visualizer");
                }
            }
            return obj;
        }

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            object obj = TryGetVisualizingObject(objectProvider);
         
            using (DataSet dataSource = BuildDataSetToBeVisualized(obj))
            {
                if (dataSource != null)
                {
                    using (VisualizerForm visualizerForm = new VisualizerForm())
                    {
                        visualizerForm.SetDataSource(dataSource, dataSource.DataSetName);
                        windowService.ShowDialog(visualizerForm);
                    }
                }
            }
        }

        private DataSet BuildDataSetToBeVisualized(object obj)
        {
            DataSet dataSource = null;
            DataTable dataTable = null;
            switch (obj)
            {
                case DataSet ds when obj is DataSet:
                    dataSource = ds;
                    break;
                case DataTable dt when obj is DataTable:
                    dataSource = new DataSet();
                    dataSource.Tables.Add(dt);
                    break;
                case DataRow dataRow when obj is DataRow:
                    dataSource = new DataSet();
                    dataTable = new DataTable();
                    dataTable.ImportRow(dataRow);
                    dataSource.Tables.Add(dataTable);
                    break;
                default:
                    break;
            }

            if (dataSource != null)
                dataSource.EnforceConstraints = false;
            
            return dataSource;
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost =
                   new VisualizerDevelopmentHost(objectToVisualize, typeof(DataSetDebuggerSide));
            visualizerHost.ShowVisualizer();
        }
    }
}
