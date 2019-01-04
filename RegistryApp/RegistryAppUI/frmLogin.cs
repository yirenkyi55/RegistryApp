
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Models;
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
    public partial class frmLogin : Form
    {
        IUserData user = new UserData();
        UserModel loginUser = new UserModel();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                //Log the user in
                UserModel myUser = new UserModel()
                {
                    Name = txtUserName.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };

                lblInfo.Text = "Loging In...";
                if (! await user.LogUser(myUser,LogUserIn))
                {
                    MessageBox.Show("Invalid username and/or password");
                    txtUserName.Focus();
                    lblInfo.Text = "";
                }
                else
                {
                    //Log the user in..
                    //Select the user details
                    var loginUser = await user.SelectUser(myUser.Name);
                    frmMain main = new frmMain(loginUser);
                    main.Show();
                    this.Hide();
                }
            }
        }
    }
}
