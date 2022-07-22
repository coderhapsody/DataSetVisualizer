using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Horizon.Visualizer.UI
{
    public partial class VisualizerForm : Form
    {
        #region Member Variables and construction

        private DataSet dataSet;
        private Dictionary<string, string> _filterText = new Dictionary<string, string>();
        
        public VisualizerForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            // override auto apply if set
            LoadSelectedDataTable(true);
        }
        private void cboTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            // use auto apply if set
            LoadSelectedDataTable();
        }
        
        #endregion

        #region Private Support Functions

        public void SetDataSource(DataSet dataSet, string title)
        {
            if (dataSet == null)
                throw new Exception("DataSet object is null");
 
            this.dataSet = dataSet;
            Text = $"DataSet: {title}";
            cboTables.Items.Clear();
            _filterText.Clear();
            foreach (DataTable dataTable in dataSet.Tables)
            {
                var tableName = dataTable.TableName;
                cboTables.Items.Add(tableName);
                _filterText.Add(tableName, "");
            }

            if(cboTables.Items.Count > 0)
            {
                cboTables.SelectedItem = cboTables.Items[0];

                string tableName = Convert.ToString(cboTables.SelectedItem);
                BindToGrid(dataSet.Tables[tableName]);
            }
        }

        public void SetDataSource(DataTable dataTable, string title)
        {
            if (dataTable == null)
                throw new Exception("DataTable object is null");
            Text = $"DataTable: {title}";
            cboTables.Items.Clear();
            cboTables.Items.Add(dataTable.TableName);            

            cboTables.SelectedItem = cboTables.Items[0];
            
            BindToGrid(dataTable);
        }

        public void SetDataSource(object obj, string title)
        {
            Text = title;
            dgvGrid.DataSource = obj;
            dgvGrid.ReadOnly = true;
            propGrid.SelectedObject = obj;
        }

        private void BindToGrid(DataTable dataTable)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgvGrid.DataSource = dataTable;
            dgvGrid.ReadOnly = true;
            foreach (DataColumn column in dataTable.Columns)
            {
                DataGridViewColumn dgColumn = dgvGrid.Columns[column.ColumnName];
                dgColumn.ToolTipText = String.Format("{0}:{1},{2}", column.ColumnName, column.DataType.Name, column.AllowDBNull ? "Null" : "Not Null");
                dgColumn.DefaultCellStyle.NullValue = "(null)";               
            }
            propGrid.SelectedObject = dataTable;
            Cursor.Current = Cursors.Default;
        }

        private void LoadSelectedDataTable(bool forceLoad = false)
        {
            if (cboTables.SelectedItem != null)
            {
                string tableName = Convert.ToString(cboTables.SelectedItem);
                if (dataSet != null)
                {
                    var filterText = _filterText[tableName];
                    tbFilter.Text = filterText;

                    if (!cbAutoApply.Checked || forceLoad)
                    {
                        BindToGrid(dataSet.Tables[tableName]);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(filterText))
                        {
                            ApplyFilter();
                        }
                        else
                        {
                            BindToGrid(dataSet.Tables[tableName]);
                        }
                    }
                }
            }
        }

        private void ApplyFilter()
        {
            Cursor.Current = Cursors.WaitCursor;

            var tableName = cboTables.SelectedItem.ToString();
            var dataTable = dataSet.Tables[tableName];

            var filterText = tbFilter.Text;
            var dataRows = dataTable.Select(filterText);

            var filteredTable = dataTable.Clone();
            filteredTable.BeginLoadData();
            Array.ForEach(dataRows, filteredTable.ImportRow);
            filteredTable.EndLoadData();

            dgvGrid.ReadOnly = false;
            dgvGrid.DataSource = filteredTable;

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                _filterText[tableName] = filterText;
            }

            dgvGrid.ReadOnly = true;
            Cursor.Current = Cursors.Default;

        }

        #endregion

        private void VisualizerForm_Load(object sender, EventArgs e)
        {
            dgvGrid.DataError += (senderObject, errorEventArgs) => { };
            //splitContainer1.Panel1Collapsed = true;
            //splitContainer1.Panel1.Hide();
        }

        private void VisualizerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
