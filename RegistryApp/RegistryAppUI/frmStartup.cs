using RegistryLibrary;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
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
    public partial class frmStartup : DevComponents.DotNetBar.Office2007Form
    {
      
        public frmStartup()
        {
            InitializeComponent();
        }

        private bool startBackup = false;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void frmStartup_Load(object sender, EventArgs e)
        {
            //Read settings from backup file
            FileSettings settings = new FileSettings();

            startBackup = settings.ReadSettingsFromFile(WriteToText.Backup);          
               
            progressBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;

            
        }
       

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (progressBar1.Value==100)
            {
               
                timer1.Enabled = false;

                if (!startBackup)
                {
                    frmLogin frmLogin = new frmLogin();
                    frmLogin.Show();
                }
                else
                {
                    frmBackupRestore frmBackupRestore = new frmBackupRestore();
                    frmBackupRestore.Show();
                }

                this.Hide();
            }
            else
            {
              
                progressBar1.Value += 1;
            }
        }
    }
}
