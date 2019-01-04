using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            // start the waiting animation
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;

            // simply start and await the loading task
            btnImport.Enabled = false;
            await Task.Run(() => LoadExcel());

            // re-enable things
            btnImport.Enabled = true;
            progressBar1.Visible = false;
        }

        private void LoadExcel()
        {
            // some work takes 5 sec
            Thread.Sleep(5000);
        }
    }
}
