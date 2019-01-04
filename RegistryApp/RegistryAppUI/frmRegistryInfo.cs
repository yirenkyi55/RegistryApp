using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;

using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp;
using RegistryLibrary.Models;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Validation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace RegistryAppUI
{
    public partial class frmRegistryInfo : Form
    {

        private IRegistryInfo registry = new RegistryInfoData();
        private RegistryInfoModel registryRecord = new RegistryInfoModel();
        string picFileName = null;
        public frmRegistryInfo()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                FileName = "",
                Filter = "png|*.png|jpeg|*.jpeg|jpg|*.jpg|gif|*.gif",
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures)
               
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picFileName = ofd.FileName;
                picLogo.Image = Image.FromFile(picFileName);
            }
        }

        private async Task LoadRegistryInfo()
        {
            registryRecord = await registry.GetRegistryInfo();
        }

        private RegistryInfoModel GetRegistry()
        {

            registryRecord.MunicipalName = txtMunicipalName.Text.Trim().ToLower();
            registryRecord.RegistryName = txtRegName.Text.Trim().ToLower();
            registryRecord.Address = txtAddress.Text.Trim().ToLower();


            if (txtEmail.Text.Trim() != string.Empty)
            {
                registryRecord.Email = txtEmail.Text.Trim();
            }

            if (txtTelephone.Text.Trim() != string.Empty)
            {
                registryRecord.Contact = txtTelephone.Text.Trim();
            }

            if (picFileName != null)
            {
                try
                {
                    registryRecord.PicData = picLogo.Image.ImageToBinary();
                    registryRecord.PicName = "Logo";
                }
                catch (Exception)
                {
                  
                }
               
            }
       

            return registryRecord;
        }

        private bool ValidControls()
        {
            bool valid = true;
            var oRegistry = GetRegistry();
            var validator = new RegistryInfoValidator();
            ValidationResult results = validator.Validate(oRegistry);
            if (!results.IsValid)
            {
                //Show error messages
                valid = false;
                foreach (var error in results.Errors)
                {
                    if (error.PropertyName == "MunicipalName")
                    {
                        errorProvider1.SetError(txtMunicipalName, error.ErrorMessage);
                    }

                    if (error.PropertyName == "RegistryName")
                    {
                        errorProvider1.SetError(txtRegName, error.ErrorMessage);
                    }

                    if (error.PropertyName == "Address")
                    {
                        errorProvider1.SetError(txtAddress, error.ErrorMessage);
                    }

                    if (error.PropertyName == "Email")
                    {
                        errorProvider1.SetError(txtEmail, error.ErrorMessage);
                    }
                }
            }
            return valid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            //Save Record into database
            if (ValidControls())
            {
                bool Update = false;
                string save = "Saved";
                if (registryRecord.Id > 0)
                {
                    Update = true;
                    save = "Updated";
                }

                if (Update)
                {
                    if (MessageBox.Show("Are you sure you want to update record?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                //Try and save record
                try
                {
                    var oRegistry = GetRegistry();
                    //Save the registryInfo
                    await registry.SaveRegistryInfo(oRegistry);
                    MessageBox.Show($"Registry Information has been successfully {save}", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sorry an Error occured while saving Data \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void PopulateControls()
        {
            if (registryRecord.Id != 0)
            {
                txtMunicipalName.Text = registryRecord.MunicipalName.ToUpper();
                txtRegName.Text = registryRecord.RegistryName.ToUpper();
                txtAddress.Text = registryRecord.Address.ToUpper();
                txtEmail.Text = registryRecord?.Email;
                txtTelephone.Text = registryRecord?.Contact;
                if (registryRecord.PicData != null)
                {
                    picLogo.Image = registryRecord.PicData.BinaryToImage();
                }

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private async void frmRegistryInfo_Load(object sender, EventArgs e)
        {
            await LoadRegistryInfo();
            PopulateControls();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (picLogo.Image != null)
            {
                picLogo.Image = null;
                registryRecord.PicData = null;
                registryRecord.PicName = null;
            }
        }
    }
}
