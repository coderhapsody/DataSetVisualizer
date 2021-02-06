using Horizon.Visualizer.UI;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;


[assembly: DebuggerVisualizer(typeof(ListDebuggerSide), typeof(VisualizerObjectSource), Target = typeof(List<>),
           Description = "Horizon List Visualizer")]

namespace Horizon.Visualizer.UI
{
    public class ListDebuggerSide : DialogDebuggerVisualizer
    {
        override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            {
                ShowVisualizer(objectProvider);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception getting object data: " + ex.Message);
            }
        }

        private static void ShowVisualizer(IVisualizerObjectProvider objectProvider)
        {
            IList data = objectProvider.GetObject() as IList;
            ListDebuggerSide.ShowVisualizerForm(data, data.GetType().FullName);            
        }

        public static void ShowVisualizerForm(IList data, string typeName)
        {
            var form = new VisualizerForm();
            form.SetDataSource(data, typeName);
            form.ShowDialog();
        }
    }
}