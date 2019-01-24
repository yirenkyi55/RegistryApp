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
    public partial class frmUpdatePdf : Form
    {
        private string fileName = null;
        private FileSettings _fileSettings;
        public  bool PrintFile { get; set; } 

        public frmUpdatePdf(FileSettings fileSettings, bool print=false)
        {
            InitializeComponent();
            _fileSettings = fileSettings;
            PrintFile = print;
            LoadFile();
       
        }

        private void LoadFile()
        {
            if (_fileSettings!=null)
            {
                axAcroPDF1.src = _fileSettings.GetFileLocation();
            }

            if (PrintFile)
            {
                btnAddFile.Visible = false;
                btnBrowse.Visible = false;
                btnDone.Visible = true;
                lblModify.Text = "View Pdf";
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog opfd = new OpenFileDialog() { Multiselect = false, ValidateNames = true, Filter = "PDF|*.pdf" })
            {
                if (opfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        axAcroPDF1.src = opfd.FileName;
                        lblFileName.Text = opfd.FileName;
                        fileName = opfd.FileName;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"Sorry an error occured. \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                _fileSettings.GetSourceFile(fileName);
                this.Close();
            }
            else
            {
                this.Close();
            }
           
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
