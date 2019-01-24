
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
    public partial class frmForgotPassword : Form
    {
        private bool isConfirmUser = false;
        private bool isQuestion = false;
        private bool isPassword = false;
        IUserData user = new UserData();
        private UserModel oUser = null;

        private string GetQuestion(int questionNumber)
        {


            switch (questionNumber)
            {
                case 1:
                    return "What is the name of your favorite childhood friend?";
                case 2:
                    return "What is your favorite movie?";
                case 3:
                    return "What is the name of the hospital where you were born?";
                case 4:
                    return "What is your uncle's name?";
                case 5:
                    return "What was your favorite place to visit as a child ?";
                default:
                    return "";
            }
        }

        public frmForgotPassword()
        {
            InitializeComponent();
        }

        private void confirmUser1_Load(object sender, EventArgs e)
        {

        }

        private bool isValidConfirmUser()
        {
            errorProvider1.Clear();
            bool valid = true;
            if (confirmUser1.txtUserName.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(confirmUser1.txtUserName, "User name is required");
                confirmUser1.txtUserName.Focus();
                valid = false;
            }
            return valid;
        }

        private bool isValidQuestion()
        {
            errorProvider1.Clear();
            bool valid = true;
            if (securityQuestion1.txtAnswer.Text.Trim() == string.Empty)
            {
                valid = false;
                errorProvider1.SetError(securityQuestion1.txtAnswer, "Please Confirm your answer");
                securityQuestion1.txtAnswer.Focus();
            }
            return valid;
        }

        private bool isValidPassword()
        {
            errorProvider1.Clear();
            bool valid = true;
            if (userPassword1.txtPassword.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(userPassword1.txtPassword, "Password is required");
                valid = false;
            }

            if (userPassword1.txtConfirmPassword.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(userPassword1.txtConfirmPassword, "Please confirm password");
                return false;
            }

            if (userPassword1.txtConfirmPassword.Text.Trim() != userPassword1.txtPassword.Text.Trim())
            {
                MessageBox.Show("Password mismatch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                errorProvider1.SetError(userPassword1.txtConfirmPassword, "Password mismatch");
                valid = false;
            }
            return valid;
        }

        private void frmForgotPassword_Load(object sender, EventArgs e)
        {
            confirmUser1.BringToFront();
            isConfirmUser = true;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (isConfirmUser)
                {
                    //Do for confirm user
                    if (isValidConfirmUser())
                    {
                        lblWait.Visible = true;


                        string username = confirmUser1.txtUserName.Text.Trim();
                        oUser = await user.SelectUser(username);
                        if (oUser != null)
                        {
                            //Show the security question
                            lblWait.Visible = false;
                            //Set the question
                            securityQuestion1.lblQuestion.Text = GetQuestion(oUser.Question);
                            securityQuestion1.BringToFront();
                            isConfirmUser = false;
                            isQuestion = true;
                            return;
                        }
                        else
                        {
                            lblWait.Visible = false;
                            MessageBox.Show("Invalid Username. You are not a registered user", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }


                if (isQuestion)
                {
                    //Do for question.
                    if (isValidQuestion())
                    {
                        lblWait.Visible = true;
                        string userAnswer = securityQuestion1.txtAnswer.Text.Trim().ToLower();
                        if (EncryptionData.ValidateEncryptedData(userAnswer, oUser.Answer))
                        {
                            //Move to the next step
                            lblWait.Visible = false;
                            isQuestion = false;
                            isPassword = true;
                            userPassword1.BringToFront();
                            btnNext.Text = "Change";
                            return;
                        }
                        else
                        {
                            lblWait.Visible = false;
                            MessageBox.Show("Invalid Answer. Please provide a valid answer.", "Invalid Answer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }

                }


                if (isPassword)
                {

                    //Change the user's password
                    if (isValidPassword())
                    {

                        string newPassword = userPassword1.txtPassword.Text.Trim();
                        oUser.Password = newPassword;
                        if (await user.ChangePassword(oUser))
                        {
                            Logger.WriteToFile(oUser.FullName, "successfully changed password");
                            MessageBox.Show("Password has been successfully changed", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Unable to change password", "Please try again", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
