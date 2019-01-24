using RegistryAppUI.GridData;
using RegistryLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmLogger : Form
    {
        private List<LoggerModel> loggers;
        DataGrids dataGrid = new DataGrids();
        public frmLogger()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogger_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        private void LoadGridData()
        {
            loggers = Logger.GetLoggers(Logger.ReadAllLogLines());          
            dgvLog.DataSource = dataGrid.LoggerDataTable(loggers);
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim()!=string.Empty && txtSearch.Text.Trim().ToLower()!="at")
            {
                if (loggers.Count()>0)
                {
                    string searchText = txtSearch.Text.Trim().ToLower();
                    var SearchedResults = loggers.Where(l => l.LoggerName.ToLower().Contains(searchText) || l.Event.ToLower().Contains(searchText) || l.Date.Contains(searchText)).ToList();
                    dgvLog.DataSource = dataGrid.LoggerDataTable(SearchedResults);
                }
            }
            else
            {
                LoadGridData();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all log events?","Confirm Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                try
                {
                    //Delete all log files
                    Logger.DeleteLogFiles();

                    MessageBox.Show("All Logs Events has been successfully deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Logger.WriteToFile(Logger.FullName, "successfully deleted all log events");
                    LoadGridData();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
