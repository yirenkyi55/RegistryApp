using RegistryLibrary;
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
    public partial class frmPerformBackup : Form
    {
        FileSettings settings = new FileSettings();
        public frmPerformBackup()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBackup_OnChange(object sender, EventArgs e)
        {
            bool checkStatus = chkBackup.Checked==true;
            
            settings.WriteSettingsToFile(checkStatus.ToString(), WriteToText.Backup);
            if (checkStatus)
            {
                Logger.WriteToFile(Logger.FullName, "successfully set a backup/restore time");
                MessageBox.Show("Backup/Restore will be performed on next restart", "Backup/Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Logger.WriteToFile(Logger.FullName, "successfully cancelled a backup/restore time");
                MessageBox.Show("Backup/Restore has been cancelled", "Backup/Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void frmPerformBackup_Load(object sender, EventArgs e)
        {
            bool backupStatus = settings.ReadSettingsFromFile(WriteToText.Backup);
            chkBackup.Checked = backupStatus;
        }
    }
}
