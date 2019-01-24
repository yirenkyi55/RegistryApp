using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmMain : Form
    {
        UserModel loginUser = new UserModel();
        private bool appOnExit = false;
        ILicenseData licenseData = new LicenseData();

        public frmMain(UserModel user)
        {
            //StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(ColorScheme.GetColor("007ACC"),, ColorScheme.GetColor("007ACC"));
            InitializeComponent();
            loginUser = user;
            LoadUserInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Logger.WriteToFile(Logger.FullName, "successfully logged out");
            this.Close();

        }

        private void btnRegInfo_Click(object sender, EventArgs e)
        {

            frmRegistryInfo frmRegistry = new frmRegistryInfo();
            frmRegistry.ShowDialog();
        }

        private void ProductStatus(string message)
        {
            lblExpiry.Text = $"Product Status: ({message})";
        }

        private void HasLicensedKey(bool keyApplied)
        {
            if (keyApplied)
            {
                //Enable all controls
                btnAllUsers.Enabled = true;
                btnAllFiles.Enabled = true;
                btnBackup.Enabled = true;
                btnLog.Enabled = true;
                btnReceiveFile.Enabled = true;
                btnRegDepartment.Enabled = true;
                btnRegUsers.Enabled = true;
                btnRegInfo.Enabled = true;
                btnSendMail.Enabled = true;
                lblChangePassword.Enabled = true;
                lblExpiry.ForeColor = Color.Green;
            }
        }

        private async Task LoadUserInfo()
        {
            lblStatus.Text = $"Login Status: {loginUser.AccessType}";
            lblFullName.Text = $" {loginUser?.FullName.ToUpper()}";
            if (await licenseData.HasExpired(ProductStatus, HasLicensedKey))
            {
                //Disable all controls
                btnAllUsers.Enabled = false;
                btnAllFiles.Enabled = false;
                btnBackup.Enabled = false;
                btnLog.Enabled = false;
                btnReceiveFile.Enabled = false;
                btnRegDepartment.Enabled = false;
                btnRegUsers.Enabled = false;
                btnRegInfo.Enabled = false;
                btnSendMail.Enabled = false;
                lblChangePassword.Enabled = false;
                lblExpiry.ForeColor = Color.Red;
            }

            if (loginUser.AccessType.ToLower()!="administrator")
            {
                //Disable access to certain controls
                btnRegInfo.Enabled = false;
                btnRegUsers.Enabled = false;
                btnAllUsers.Enabled = false;
                btnLog.Enabled = false;
            }
        }

        private void btnRegUsers_Click(object sender, EventArgs e)
        {
            frmRegUsers frmUsers = new frmRegUsers();
            frmUsers.ShowDialog();
        }

        private void btnRegDepartment_Click(object sender, EventArgs e)
        {
            frmDepartments departments = new frmDepartments();
            departments.ShowDialog();
        }

        private void btnReceiveFile_Click(object sender, EventArgs e)
        {
            frmFile file = new frmFile();
            file.ShowDialog();
        }

        private void btnAllFiles_Click(object sender, EventArgs e)
        {
            frmAllFiles allFiles = new frmAllFiles();
            allFiles.ShowDialog();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            frmMailUsers frmMail = new frmMailUsers();
            frmMail.ShowDialog();
        }

        private void btnAllDepartments_Click(object sender, EventArgs e)
        {
            frmAllUsers allUsers = new frmAllUsers();
            allUsers.ShowDialog();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Logger.WriteToFile(Logger.FullName, "successfully logged out");
            this.Close();

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!appOnExit)
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Confirm logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                frmLogin login = new frmLogin();
                login.Show();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(loginUser);
            changePassword.ShowDialog();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {

            frmPerformBackup backup = new frmPerformBackup();
            backup.ShowDialog();

        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            frmLogger frmLogger = new frmLogger();
            frmLogger.ShowDialog();
        }

        private async void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
            await LoadUserInfo();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
