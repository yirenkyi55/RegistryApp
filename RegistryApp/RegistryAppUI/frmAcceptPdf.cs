using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RegistryLibrary.Infrastructure;

namespace RegistryAppUI
{
    public partial class frmAcceptPdf : Form
    {
        private FileSettings _fileSettings;
        private string fileName;
        public frmAcceptPdf(FileSettings fileSettings)
        {
            InitializeComponent();
            _fileSettings = fileSettings;
            LoadPdf();
        }

        private  void btnClose_Click(object sender, EventArgs e)
        {
         
            this.Close();
        }

        private void LoadPdf()
        {
            if (!string.IsNullOrEmpty(_fileSettings.FileName))
            {
                try
                {
                    axAcroPDF1.src = _fileSettings.FileName;
                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog opfd = new OpenFileDialog() {Multiselect=false,ValidateNames=true,Filter="PDF|*.pdf" })
            {
                try
                {

                    if (opfd.ShowDialog() == DialogResult.OK)
                    {
                        axAcroPDF1.src = opfd.FileName;
                        lblFileName.Text = opfd.FileName;
                        fileName = opfd.FileName;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured.\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            try
            {
                _fileSettings.GetSourceFile(fileName);
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Sorry an error occured.\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
