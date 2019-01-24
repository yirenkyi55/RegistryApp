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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmChangePassword : Form
    {
        private UserModel oUser = null;
        IUserData userdata = new UserData();

        public frmChangePassword(UserModel user)
        {
            InitializeComponent();
            oUser = user;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtConfirm.Text = "";
            txtCurrent.Text = "";
            txtNew.Text = "";
            txtCurrent.Focus();
        }

        private bool IsValidControls()
        {
            bool valid = true;
            if (txtCurrent.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtCurrent, "Current password is required");
                valid = false;
            }

            if (txtNew.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtNew, "Please Enter new password");
                valid = false;
            }

            if (txtConfirm.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConfirm, "Please confirm password");
                return false;
            }

            if (txtConfirm.Text.Trim() != txtNew.Text.Trim())
            {
                MessageBox.Show("Password mismatch", "Password mismatch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                valid = false;
            }
            return valid;
        }
        private async void btnChange_Click(object sender, EventArgs e)
        {
            if (IsValidControls())
            {
                try
                {
                    string oldPassword = txtCurrent.Text.Trim();
                    if (EncryptionData.ValidateEncryptedData(oldPassword, oUser.Password))
                    {
                        //Change the current password
                        oUser.Password = txtNew.Text.Trim();
                        if (await userdata.ChangePassword(oUser))
                        {
                            Logger.WriteToFile(Logger.FullName, "Changed password");
                            MessageBox.Show("Password has been successfully changed", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Unable to change password", "Change password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password. Please Enter the right password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
