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
using FluentValidation;
using RegistryLibrary.Validation;
using FluentValidation.Results;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;

namespace RegistryAppUI
{


    public partial class frmRegUsers : Form
    {
        IUserData user = new UserData();

        public frmRegUsers()
        {
            InitializeComponent();
            CountAllUsers();
        }

        private async Task ResetControls()
        {
            errorProvider1.Clear();
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtFullName.Text = "";
            txtAnswer.Text = "";
            cboAccessType.SelectedIndex = -1;
            cboQuestion.SelectedIndex = -1;
            txtUsername.Focus();
            await CountAllUsers();
        }

        private async Task CountAllUsers()
        {
            var result = await user.GetUsers();
            lblUsers.Text = $"Total Users Created: {result.Count().ToString()}";
        }

        private bool IsValidControls()
        {
            errorProvider1.Clear();
            bool valid = true;
            UserModel oUser = GetUser();
            UserValidator validator = new UserValidator();
            ValidationResult result = validator.Validate(oUser);
            if (!result.IsValid)
            {
                valid = false;
                //Display the error messages
                foreach (var error in result.Errors)
                {
                    if (error.PropertyName == "Name")
                    {
                        errorProvider1.SetError(txtUsername, error.ErrorMessage);
                    }

                    if (error.PropertyName == "FullName")
                    {
                        errorProvider1.SetError(txtFullName, error.ErrorMessage);
                    }

                    if (error.PropertyName == "Password")
                    {
                        errorProvider1.SetError(txtPassword, error.ErrorMessage);
                    }

                    if (error.PropertyName == "Question")
                    {
                        errorProvider1.SetError(cboQuestion, error.ErrorMessage);
                    }

                    if (error.PropertyName == "Answer")
                    {
                        errorProvider1.SetError(txtAnswer, error.ErrorMessage);
                    }


                    if (error.PropertyName == "AccessType")
                    {
                        errorProvider1.SetError(cboAccessType, error.ErrorMessage);
                    }
                }
            }

            if (txtConfirmPassword.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConfirmPassword, "Confrim password is required");
                valid = false;
            }

            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPassword, "Password mismatch");
                valid = false;
            }
            return valid;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private UserModel GetUser()
        {
            var oUser = new UserModel()
            {
                Name = txtUsername.Text.Trim().ToLower(),
                Password = txtPassword.Text,
                AccessType = cboAccessType.Text,
                Answer = txtAnswer.Text.Trim().ToLower(),
                FullName = txtFullName.Text.Trim().ToLower(),
                Question = cboQuestion.SelectedIndex + 1
            };

            return oUser;
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (IsValidControls())
            {
                try
                {

                    //Save user to the database
                    UserModel oUser = GetUser();
                    //check if the user name already exist.
                    if (await user.SelectUser(oUser.Name) == null)
                    {
                        await user.CreateUser(oUser);
                    }

                    else
                    {
                        MessageBox.Show("User name already exist. Choose a different user name.", "User name exist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (Logger.FullName != null)
                    {
                        Logger.WriteToFile(Logger.FullName, $"Created a new user: {oUser.Name}");
                    }
                    MessageBox.Show("A new user has been successfully created", "Create User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await ResetControls();
                    this.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            await ResetControls();
        }

        private void btnAllUsers_Click(object sender, EventArgs e)
        {
            frmAllUsers frmAll = new frmAllUsers();
            frmAll.ShowDialog();
        }

        private void txtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlValidators.ValidateLength(txtFullName, e, 150);
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlValidators.ValidateLength(txtUsername, e, 100);
        }
    }
}
