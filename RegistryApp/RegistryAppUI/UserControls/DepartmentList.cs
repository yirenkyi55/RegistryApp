using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistryLibrary.Models;
using RegistryAppUI.GridData;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;

namespace RegistryAppUI.UserControls
{
    public partial class DepartmentList : UserControl
    {
        private  IEnumerable<DepartmentModel> _departments;
        private IDepartmentData department = new DepartmentData();
        public static DataGrids _gridData = new DataGrids();

        public DepartmentList()
        {
            InitializeComponent();
            WireUp();
        }

        private void WireUp()
        {
            _departments = department.SelectAllDepartments();
            gridDepartments.DataSource = _gridData.DepartmentTable(_departments);
            _gridData.departmentGridEvent += _gridData_departmentGridEvent;
            FormatGridColumns();
            CountDepartments();
        }

        private void CountDepartments()
        {
            lblStatus.Text = $"Number of Registered Departments: {gridDepartments.Rows.Count}";
        }

        private void FormatGridColumns()
        {
            if (gridDepartments.Columns.Count>0)
            {
                gridDepartments.Columns[0].Visible = false;
            }

        
        }

        private void _gridData_departmentGridEvent(object sender, DataTable e)
        {
            gridDepartments.DataSource = null;
            gridDepartments.DataSource = e;
            FormatGridColumns();
            _departments = department.SelectAllDepartments();
            CountDepartments();
        }

        private void DepartmentList_Load(object sender, EventArgs e)
        {

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gridDepartments.SelectedRows.Count>0)
            {
                int selectedId = int.Parse(gridDepartments.SelectedRows[0].Cells[0].Value.ToString());
                var selectedDepartment = _departments.First(d => d.Id == selectedId);
                frmModifyDepartment modifyDepartment = new frmModifyDepartment(selectedDepartment);
                modifyDepartment.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridDepartments.SelectedRows.Count>0)
            {
                //Confirm message before deleting
                int SelectedId =int.Parse( gridDepartments.SelectedRows[0].Cells[0].Value.ToString());
                var selectedDepartment = _departments.First(d => d.Id == SelectedId);
                if (MessageBox.Show($"Are you sure you want to delete {selectedDepartment.DepartmentName} ?","Confirm Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
                {
                    return;
                }

                //Delete the selected department
                department.DeleteDepartment(selectedDepartment);
                Logger.WriteToFile(Logger.FullName, "successfully deleted a department");
                MessageBox.Show($"{selectedDepartment.DepartmentName} has been successfully deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WireUp();
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (gridDepartments.SelectedRows.Count > 0)
            {                
                if (MessageBox.Show($"Are you sure you want to delete all departments?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //Delete the selected department
                department.DeleteAllDepartment();
                Logger.WriteToFile(Logger.FullName, "successfully deleted all files");
                MessageBox.Show($"All Departments has been successfully deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WireUp();
            }
        }

        private void SearchedDepartments(string search)
        {
            search = search.Trim().ToLower();
            var searchDepartments = _departments.Where(d => d.DepartmentName.ToLower().StartsWith(search));
            if (searchDepartments.Count()>0)
            {
                gridDepartments.DataSource = _gridData.DepartmentTable(searchDepartments);
                //_gridData.departmentGridEvent += _gridData_departmentGridEvent;
                FormatGridColumns();
                lblStatus.Text = $"{searchDepartments.Count()} record(s) matched";
            }
            else
            {
                //Find a new search
                searchDepartments = _departments.Where(d => d.DepartmentName.ToLower().Contains(search));
                if (searchDepartments.Count()>0)
                {
                    gridDepartments.DataSource = _gridData.DepartmentTable(searchDepartments);
                    //_gridData.departmentGridEvent += _gridData_departmentGridEvent;
                    FormatGridColumns();
                    lblStatus.Text = $"{searchDepartments.Count()} record(s) matched";
                }
                else
                {
                    //No search found
                    gridDepartments.DataSource = null;
                    lblStatus.Text = "No record matched";
                }
            }
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                WireUp();
                
            }
            else
            {
                SearchedDepartments(txtSearch.Text.Trim());
            }
           
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtSearch.ValidateMaterial(100, e);
        }
    }
}
