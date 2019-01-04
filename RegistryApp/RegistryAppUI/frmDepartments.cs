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
    public partial class frmDepartments : Form
    {
        public frmDepartments()
        {
            InitializeComponent();
        }


        private void frmDepartments_Load(object sender, EventArgs e)
        {
            string[] colors = btnCreateNew.Tag.ToString().Split(',');
            btnCreateNew.IdleFillColor = Color.FromArgb(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]));
            newDepartment1.BringToFront();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            //Set the forecolor
            string[] colors = ((Control)sender).Tag.ToString().Split(',');
            btnCreateNew.IdleFillColor = Color.FromArgb(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]));
            btnDepartments.IdleFillColor = Color.FromArgb(19, 50, 71);
            indicator.Top = ((Control)sender).Top;
            newDepartment1.BringToFront();
        }


        private void btnDepartments_Click_1(object sender, EventArgs e)
        {
            string[] colors = btnDepartments.Tag.ToString().Split(',');
            btnDepartments.IdleFillColor = Color.FromArgb(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]));
            btnCreateNew.IdleFillColor = Color.FromArgb(19, 50, 71);
            indicator.Top = ((Control)sender).Top;
            departmentList1.BringToFront();
        }
    }
}
