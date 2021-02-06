using System.Data;
using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace Allegro.Visualizer.NetCore.DebuggeeSide
{
    public class DataRowVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target != null)
            {
                if (target is DataRow row)
                {
                    DataTable table;

                    if (row.Table == null)
                    {
                        table = new DataTable();

                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            table.Columns.Add(string.Format("Col{0}", i.ToString()), typeof(string));
                        }
                    }
                    else
                    {
                        table = row.Table.Clone();
                    }

                    table.LoadDataRow(row.ItemArray, true);

                    StreamSerializer.ObjectToStream(outgoingData, table);
                }               
                else if (target is DataView view)
                {
                    DataTable table = view.Table.Clone();

                    foreach (DataRow dataRow in view.Table.Rows)
                    {
                        table.LoadDataRow(dataRow.ItemArray, true);
                    }

                    StreamSerializer.ObjectToStream(outgoingData, table);
                }
                else if (target is DataRowView dataRowView)
                {
                    DataTable table = dataRowView.Row.Table.Clone();

                    table.LoadDataRow(dataRowView.Row.ItemArray, true);

                    StreamSerializer.ObjectToStream(outgoingData, table);
                }
                else if (target is DataRowCollection dataRowCollection)
                {
                    DataTable table = null;

                    if (dataRowCollection != null && dataRowCollection.Count > 0)
                    {
                        if (dataRowCollection[0].Table == null)
                        {
                            table = new DataTable();

                            for (int i = 0; i < dataRowCollection[0].ItemArray.Length; i++)
                            {
                                table.Columns.Add(string.Format("Col{0}", i.ToString()), typeof(string));
                            }
                        }
                        else
                        {
                            table = dataRowCollection[0].Table.Clone();
                        }

                        foreach (DataRow dataRow in dataRowCollection)
                        {
                            table.LoadDataRow(dataRow.ItemArray, true);
                        }
                    }

                    StreamSerializer.ObjectToStream(outgoingData, table);
                }
            }
        }
    }
}
