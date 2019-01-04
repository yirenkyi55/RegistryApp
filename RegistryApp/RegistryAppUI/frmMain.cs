using RegistryLibrary.Models;
using System;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmMain : Form
    {
        UserModel loginUser = new UserModel();

        public frmMain(UserModel user)
        {
            //StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(ColorScheme.GetColor("007ACC"),, ColorScheme.GetColor("007ACC"));
            InitializeComponent();
            loginUser = user;
            LoadUserInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegInfo_Click(object sender, EventArgs e)
        {
            
            frmRegistryInfo frmRegistry = new frmRegistryInfo();          
            frmRegistry.ShowDialog();
        }

        private void LoadUserInfo()
        {
            lblStatus.Text =$"Login as: {loginUser.AccessType}";
           
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
            if (MessageBox.Show("Are you sure you want to log out?","Confrim log out",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Close();
            }
           
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
        }
    }
}
