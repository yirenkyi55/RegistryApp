using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmAbout : Form
    {
        ILicenseData licenseData = new LicenseData();

        public frmAbout()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void lblInsertLicense_Click(object sender, EventArgs e)
        {
            frmLicense frmLicense = new frmLicense();
            frmLicense.ShowDialog(this);
           await InspectLicense();
        }

        private void ProductStatus(string message)
        {
            lblActivationStatus.Text = $"{message}";
        }

        private void HasLicensedKey(bool keyApplied)
        {
            if (keyApplied)
            {
             
                lblActivationStatus.ForeColor = Color.Green;
                lblInsertLicense.Visible = false;
            }
        }


        private async void frmAbout_Load(object sender, EventArgs e)
        {
            try
            {
                await InspectLicense();
            }
            catch (Exception)
            {

              
            }
        }

        private async Task InspectLicense()
        {
            if (await licenseData.HasExpired(ProductStatus, HasLicensedKey))
            {
                lblInsertLicense.Visible = true;
                lblActivationStatus.ForeColor = Color.Red;
            }
        }
    }
}
