using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Models;
using System.Globalization;

using RegistryAppUI.ViewModels;
using RegistryLibrary.Infrastructure;

namespace RegistryAppUI.UserControls
{
    public partial class NewDepartment : UserControl
    {
        #region Private Variables
        IDepartmentData _department = new DepartmentData();
        #endregion
        public NewDepartment()
        {
            InitializeComponent();
        }

        private void PopulateGrid()
        {
            var departments = new DepartmentData().SelectAllDepartments();
            DepartmentList._gridData.DepartmentTable(departments);
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                //Create new department
                DepartmentModel department = new DepartmentModel()
                {
                    DepartmentName = txtName.Text.Trim().ToLower()
                };

                if (txtEmail.Text.Trim() != string.Empty)
                {
                    department.Email = txtEmail.Text.Trim();
                }

                if (txtAddress.Text.Trim() != string.Empty)
                {
                    department.Address = txtAddress.Text.Trim().ToLower();
                }
                department.DepartmentName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(department.DepartmentName);
                //Save department into the database
                department = await _department.CreateDepartment(department);
                Logger.WriteToFile(Logger.FullName, "successfully created new department");
                MessageBox.Show($"{department.DepartmentName} has been successfully created", "Create", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateGrid();
                btnReset_Click(this, null);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtName.Focus();
        }

        private bool IsValidForm()
        {
            errorProvider1.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                errorProvider1.SetError(txtName, "Department Name is required");
                isValid = false;
            }

            if (txtEmail.Text.Trim() != string.Empty)
            {
                UserEmail email = new UserEmail()
                {
                    Email = txtEmail.Text.Trim()
                };
                UserMailsValidator validator = new UserMailsValidator();
                var validate = validator.Validate(email);
                if (!validate.IsValid)
                {
                    isValid = false;
                    foreach (var error in validate.Errors)
                    {
                        errorProvider1.SetError(txtEmail, error.ErrorMessage);
                    }
                }
            }

            if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                errorProvider1.SetError(txtAddress, "Address is required");
                isValid = false;
            }

            return isValid;
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
           ControlValidators.ValidateLength(txtName,e, 100);
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
          ControlValidators.ValidateLength(txtEmail, e,20);
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
