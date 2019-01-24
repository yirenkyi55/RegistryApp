using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistryLibrary.Models;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using System.Globalization;
using RegistryAppUI.UserControls;
using RegistryLibrary.Infrastructure;

namespace RegistryAppUI
{
    public partial class frmModifyDepartment : Form
    {
        private DepartmentModel _department;
        private IDepartmentData department = new DepartmentData();
        public frmModifyDepartment(DepartmentModel department)
        {
            InitializeComponent();
            _department = department;
            WireUp();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WireUp()
        {
            txtName.Text = _department.DepartmentName;
            txtEmail.Text = _department?.Email;
            txtAddress.Text = _department?.Address;
            lblModify.Text = $"Modify - {_department.DepartmentName}";
        }

        private void UpdateGrid()
        {
            var departments = new DepartmentData().SelectAllDepartments();
            DepartmentList._gridData.DepartmentTable(departments);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                try
                {
                    _department.DepartmentName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text.Trim());

                    if (txtAddress.Text.Trim() == string.Empty)
                    {
                        _department.Address = null;
                    }
                    else
                    {
                        _department.Address = txtAddress.Text.Trim().ToLower();
                    }

                    if (txtEmail.Text.Trim() == null)
                    {
                        _department.Email = null;
                    }
                    else
                    {
                        _department.Email = txtEmail.Text.Trim();
                    }

                    //Update department records.
                    department.UpdateDepartment(_department);
                    Logger.WriteToFile(Logger.FullName, "successfully modified a department");
                    MessageBox.Show($"Record has been successfully updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trigger gridview event for DepartmentList
                    UpdateGrid();
                    this.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
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
            return isValid;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            WireUp();
        }
    }
}
