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
    public partial class frmLicense : Form
    {
        ILicenseData licenseData = new LicenseData();
        public frmLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
        
        private static void KeyPressValid(object sender, KeyPressEventArgs e)
        {
   
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
          
        }

        private void txtCode2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (txtCode2.Text.Length == 2)
            {
                txtCode3.Focus();
            }

            KeyPressValid(sender, e);

        }

        private void txtCode3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (txtCode3.Text.Length == 2)
            {
                txtCode4.Focus();
            }

            KeyPressValid(sender, e);
        }


        private void txtCode5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            KeyPressValid(sender, e);
            //ControlValidators.ValidateLength((Bunifu.Framework.UI.BunifuMetroTextbox)sender, e, 3);
        }

        

        private void txtCode1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (txtCode1.Text.Length == 2)
            {
                txtCode2.Focus();
            }

            KeyPressValid(sender, e);
        }

    

        private void txtCode4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (txtCode4.Text.Length == 2)
            {
                txtCode5.Focus();
            }
            KeyPressValid(sender, e);
        }

        private bool IsValidControls()
        {
            bool valid = true;
            if (txtCode1.Text.Trim()==string.Empty)
            {
                valid = false;
                errorProvider1.SetError(txtCode1, "Please Insert Key");
            }

            if (txtCode2.Text.Trim() == string.Empty)
            {
                valid = false;
                errorProvider1.SetError(txtCode2, "Please Insert Key");
            }

            if (txtCode3.Text.Trim() == string.Empty)
            {
                valid = false;
                errorProvider1.SetError(txtCode3, "Please Insert Key");
            }

            if (txtCode4.Text.Trim() == string.Empty)
            {
                valid = false;
                errorProvider1.SetError(txtCode4, "Please Insert Key");
            }

            if (txtCode5.Text.Trim() == string.Empty)
            {
                valid = false;
                errorProvider1.SetError(txtCode5, "Please Insert Key");
            }
            return valid;
        }
        private async void btnApply_Click(object sender, EventArgs e)
        {
            if (IsValidControls())
            {
                //Insert the License
                LicenseModel license = new LicenseModel()
                {
                    LicenseKey = txtCode1.Text + txtCode2.Text + txtCode3.Text + txtCode4.Text
                };

                try
                {
                    //Insert the license into the database
                    await licenseData.InsertLicense(license);
                    MessageBox.Show("License has been successfully applied", "Apply License", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
