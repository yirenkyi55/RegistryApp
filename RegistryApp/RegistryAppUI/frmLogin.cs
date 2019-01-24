
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
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
    public partial class frmLogin : Form
    {
        IUserData user = new UserData();
        UserModel loginUser = new UserModel();
        ILicenseData licenseData = new LicenseData();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?", "File Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
           
            }

         
        }

     

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void LogUserIn(UserModel user)
        {
            loginUser = user;
        }

        private bool IsValidControl()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                errorProvider1.SetError(txtUserName, "User name is required");
                valid = false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errorProvider1.SetError(txtPassword, "Password is required");
                valid = false;
            }
            return valid;

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsValidControl())
            {
                try
                {
                    FileSettings settings = new FileSettings();
                    settings.CreateFolders();
                    //Log the user in
                    UserModel myUser = new UserModel()
                    {
                        Name = txtUserName.Text.Trim(),
                        Password = txtPassword.Text.Trim()
                    };

                    lblInfo.Text = "Loging In...";
                    if (!await user.LogUser(myUser, LogUserIn))
                    {
                        MessageBox.Show("Invalid username and/or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUserName.Focus();
                        lblInfo.Text = "";
                    }
                    else
                    {

                        //Log the user in..
                        //Select the user details
                        //Inspect product to verify validity
                        await licenseData.InsertInstalledDate(DateTime.Now);
                        await licenseData.InsertCurrentDate(DateTime.Now);

                        var loginUser = await user.SelectUser(myUser.Name);
                        Logger.FullName = loginUser.FullName;
                        //Write to Logger File
                        Logger.WriteToFile(Logger.FullName, $"successfully logged in");

                        frmMain main = new frmMain(loginUser);
                        main.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occurred while loggin in. \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            frmForgotPassword frmForgot = new frmForgotPassword();
            frmForgot.ShowDialog();
            txtUserName.Focus();
        }

        private async void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                await DetermineCreatingAdmin();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DetermineCreatingAdmin()
        {
            IEnumerable<UserModel> allUsers = await user.GetUsers();
            if (allUsers.Count() == 0)
            {
                btnNewAdmin.Visible = true;
            }
            else
            {
                var admin = allUsers.Where(u => u.AccessType.ToLower() == "administrator");
                if (admin.Count() < 1)
                {
                    btnNewAdmin.Visible = true;
                }
                else
                {
                    btnNewAdmin.Visible = false;
                }

            }
        }

        private async void btnNewAdmin_Click(object sender, EventArgs e)
        {
            frmRegUsers frmReg = new frmRegUsers();
            frmReg.ShowDialog();
           await DetermineCreatingAdmin();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
       
          
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
       
        }
    }
}
