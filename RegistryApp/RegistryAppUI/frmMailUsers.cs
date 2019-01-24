using RegistryAppUI.ViewModels;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmMailUsers : Form
    {

        List<DepartmentModel> departments = new List<DepartmentModel>();
        Dictionary<DepartmentModel, string> toEmails = new Dictionary<DepartmentModel, string>();
        public static string FileName { get; set; }
        Attachment fileAttachment = null;

        public frmMailUsers()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            cboDepartment.DataSource = null;
            IDepartmentData department = new DepartmentData();
            departments = department.SelectAllDepartments().OrderBy(d=>d.DepartmentName).ToList();

            if (departments.Count > 0)
            {
                cboDepartment.DataSource = departments;
                cboDepartment.DisplayMember = "DepartmentName";
                cboDepartment.ValueMember = "Id";
                cboDepartment.SelectedIndex = -1;
            }
            lblMessage.Text = "";
        }

        private bool ValidateEmail(string email)
        {
            bool valid = true;
            errorProvider1.Clear();
            UserEmail userEmail = new UserEmail() { Email = email };
            UserMailsValidator validator = new UserMailsValidator();
            var result = validator.Validate(userEmail);
            if (!result.IsValid)
            {
                valid = false;
                foreach (var error in result.Errors)
                {
                    errorProvider1.SetError(txtEmail, error.ErrorMessage);
                }
            }

            return valid;
        }



        private void AddEmails(DepartmentModel department)
        {
            if (!toEmails.Values.Contains(department.Email))
            {
                toEmails.Add(department, department.Email);
                lsbEmails.DataSource = null;
                lsbEmails.DataSource = new BindingSource(toEmails, null);
                lsbEmails.DisplayMember = "Value";
                lsbEmails.ValueMember = "Key";
            }
            else
            {
                MessageBox.Show($"The address: {department.Email} has already been added", "Add", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepartment.Items.Count > 0)
            {
                DepartmentModel department = (DepartmentModel)cboDepartment.SelectedItem;
                if (department != null)
                {
                    //Check if email exist.
                    txtEmail.Text = "";
                    if (department.Email == null)
                    {
                        lblMessage.Text = $"No Email Found for {department.DepartmentName}. Click to Modify Department";
                    }
                    else
                    {
                        lblMessage.Text = "";
                    }
                }
            }
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {
            DepartmentModel department = (DepartmentModel)cboDepartment.SelectedItem;
            frmModifyDepartment frmModify = new frmModifyDepartment(department);
            frmModify.ShowDialog();
            LoadDepartments();
        }

        private void txtEmail_OnValueChanged(object sender, EventArgs e)
        {
            cboDepartment.SelectedIndex = -1;
        }

        private void RemoveDepartment(DepartmentModel department)
        {
            departments.Remove(department);
            cboDepartment.DataSource = null;
            cboDepartment.DataSource = departments.OrderBy(d=>d.DepartmentName).ToList();
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "Id";
            cboDepartment.SelectedIndex = -1;
            lblMessage.Text = "";
        }

        private void ReAddDepartment(DepartmentModel department)
        {
            departments.Add(department);
            cboDepartment.DataSource = null;
            cboDepartment.DataSource = departments.OrderBy(d=>d.DepartmentName).ToList();
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "Id";
            cboDepartment.SelectedIndex = -1;
            lblMessage.Text = "";
        }
        private void btnAddMail_Click(object sender, EventArgs e)
        {
            if (cboDepartment.SelectedIndex != -1)
            {
                DepartmentModel department = (DepartmentModel)cboDepartment.SelectedItem;
                if (department.Email != null)
                {
                    AddEmails(department);
                    RemoveDepartment(department);
                }
                else
                {
                    MessageBox.Show($"No Email address found for {department.DepartmentName}.\n Please Update Department Information", "No Email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


            }
            else
            {
                if (txtEmail.Text.Trim() != string.Empty)
                {
                    if (ValidateEmail(txtEmail.Text.Trim()))
                    {
                        DepartmentModel department = new DepartmentModel();
                        department.Email = txtEmail.Text;
                        AddEmails(department);
                        txtEmail.Text = "";
                        txtEmail.Focus();
                    }

                }

            }


        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                FileName = "",
                Filter = "(pdf)|*.pdf|(jpg)|*.jpg|(png)|*.png",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = ofd.FileName;
                btnRefresh.Visible = true;
            }
        }

        private void btnSearchPdf_Click(object sender, EventArgs e)
        {
            frmSearchFile frmSearch = new frmSearchFile();
            frmSearch.ShowDialog();
            FileSettings settings = new FileSettings();
            if (FileName != null)
            {
                settings.FileName = FileName;
                try
                {
                    txtFileName.Text = settings.GetFileLocation();
                    btnRefresh.Visible = true;
                }
                catch (Exception)
                {


                }
                FileName = null;
            }


        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboDepartment.SelectedIndex = -1;
            txtBody.Clear();
            txtEmail.Text = "";
            txtFileName.Text = "";
            txtSubject.Text = "";
            txtEmail.Focus();
            lsbEmails.DataSource = null;
            toEmails.Clear();
            btnRefresh.Visible = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lsbEmails.SelectedIndex != -1)
            {
                DepartmentModel selectedDepartment = ((KeyValuePair<DepartmentModel, string>)lsbEmails.SelectedItem).Key;
                //Remove the selected department from the emails
                toEmails.Remove(selectedDepartment);
                lsbEmails.DataSource = null;
                if (toEmails.Values.Count > 0)
                {
                    lsbEmails.DataSource = new BindingSource(toEmails, null);
                    lsbEmails.DisplayMember = "Value";
                    lsbEmails.ValueMember = "Key";

                }

                if (selectedDepartment.Id != 0)
                {
                    ReAddDepartment(selectedDepartment);
                }


            }
        }
        private bool IsValidControls()
        {
            bool valid = true;
            if (lsbEmails.Items.Count == 0)
            {
                errorProvider1.SetError(lsbEmails, "Please Specify Recipient Mailing Address");
                valid = false;
            }

            if (txtSubject.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtSubject, "Please enter email subject");
                valid = false;
            }

            if (txtBody.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtBody, "Please Enter email body");
                valid = false;
            }
            return valid;
        }

        private async void btnSendMail_Click(object sender, EventArgs e)
        {
            if (IsValidControls())
            {
                if (txtFileName.Text.Trim() != string.Empty)
                {
                    fileAttachment = new Attachment(txtFileName.Text.Trim());
                }

                string subject = $"{txtSubject.Text.Trim()}";
                string body = txtBody.Text.Trim();

                // start the waiting animation
                pnlProgress.Visible = true;
                progressBar1.Style = ProgressBarStyle.Marquee;
                List<string> useremails = toEmails.Values.ToList();
                var result = await Task.Run(() => Mailer.SendMail(useremails, subject, body, ShowMailError, fileAttachment));


                if (result)
                {
                    pnlProgress.Visible = false;
                    Logger.WriteToFile(Logger.FullName, "successfully sent a mail");
                    MessageBox.Show("Your mail has been successfully sent", "Send Mail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnReset_Click(this, null);

                }



            }
        }

        private void ShowMailError(string errorMessage)
        {
            pnlProgress.Visible = false;
            MessageBox.Show($"Sorry. An Error occured while sending mail.\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFileName.Text = "";
            btnRefresh.Visible = false;
        }

        private void frmMailUsers_Load(object sender, EventArgs e)
        {

        }
    }
}
