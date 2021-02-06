using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;

namespace Allegro.Visualizer
{
    public class DataTableDebuggerSide : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            object obj = objectProvider.GetObject();
            DataTable dataTable = obj as DataTable;
            if (dataTable != null)
            {
                using (VisualizerForm visualizerForm = new VisualizerForm())
                {
                    visualizerForm.SetDataSource(dataTable, dataTable.TableName);
                    windowService.ShowDialog(visualizerForm);
                }
            }
        }
    }
}
