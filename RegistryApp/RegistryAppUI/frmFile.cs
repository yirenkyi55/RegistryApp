using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmFile : Form
    {
        private FileSettings _pdfSettings = new FileSettings();
        private IIncomingFileData _incomingFile = new IncomingFileData();
        private IEnumerable<DepartmentModel> departments;
        private IDepartmentData _department = new DepartmentData();
        private IEnumerable<IncomingFileModel> allFiles;
        //private IncomingFileModel selectedFileToUpdate;
        public IncomingFileModel selectedFileToUpdate { get; set; }
        int itemsToTake = 0;

        private void RegistryNumber()
        {
            if (_pdfSettings.ReadSettingsFromFile())
            {
                int registryNumber = _incomingFile.NextRegistryNumber();
                txtRegistryNumber.Text = registryNumber.ToString("");
                txtRegistryNumber.Enabled = false;
                switchGenerateReg.Value = true;
                dtpDateReceived.Focus();

            }
            else
            {

                txtRegistryNumber.Enabled = true;
                txtRegistryNumber.Text = "";
                switchGenerateReg.Value = false;
                txtRegistryNumber.Focus();
            }

        }
        /// <summary>
        /// Loads all registered files into list
        /// </summary>
        private void LoadAllFiles()
        {
            allFiles = new IncomingFileData().SelectAllFiles();

        }

        /// <summary>
        /// Enables/Disable insert, update and delete button 
        /// </summary>
        public void ToggleButtonStates()
        {
            if (selectedFileToUpdate != null)
            {
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnSave.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void ResetControls()
        {

            dtpDateReceived.Value = DateTime.Now;
            txtFromDepartment.Text = "";
            txtPdfName.Text = "";
            txtPersonSent.Text = "";
            txtReferenceNumber.Text = "";
            txtRemarks.Text = "";
            txtSearch.Text = "";
            txtSubject.Text = "";
            dtpDateofLetter.Value = DateTime.Now;
            cboDepartment.SelectedIndex = -1;
            RegistryNumber();
            itemsToTake = 0;
            ToggleNavigationButtons();
            selectedFileToUpdate = null;
            ToggleButtonStates();
        }

        private void LoadAllDepartments()
        {
            IDepartmentData department = new DepartmentData();
            departments = department.SelectAllDepartments();
            if (departments.Count() > 0)
            {
                cboDepartment.DataSource = departments;
                cboDepartment.DisplayMember = "DepartmentName";
                cboDepartment.ValueMember = "Id";
                cboDepartment.SelectedIndex = -1;
                lblDepartments.Text = $"{departments.Count()} Registered Departments";
            }
            else
            {
                lblDepartments.Text = "No Departments Found.";
            }

        }

        public frmFile()
        {
            InitializeComponent();
            LoadAllDepartments();
            RegistryNumber();
            LoadAllFiles();
            ToggleNavigationButtons();
            ToggleButtonStates();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();


        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (selectedFileToUpdate != null)
            {
                _pdfSettings.FileName = selectedFileToUpdate.FileName;
                _pdfSettings.GetSourceFilePathEvent += _pdfSettings_GetSourceFilePathEvent;
                frmUpdatePdf frmUpdate = new frmUpdatePdf(_pdfSettings);
                frmUpdate.ShowDialog();
            }
            else
            {
                _pdfSettings.FileName = txtPdfName?.Text;
                _pdfSettings.GetSourceFilePathEvent += PdfSettings_GetSourceFilePathEvent;
                frmAcceptPdf acceptPdf = new frmAcceptPdf(_pdfSettings);
                acceptPdf.ShowDialog();
            }

        }

        private void _pdfSettings_GetSourceFilePathEvent(object sender, string e)
        {
            txtPdfName.Text = e;
        }

        private void PdfSettings_GetSourceFilePathEvent(object sender, string e)
        {
            txtPdfName.Text = e;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddDepartments_Click(object sender, EventArgs e)
        {
            frmDepartments ofrmDepart = new frmDepartments();
            ofrmDepart.ShowDialog();
            LoadAllDepartments();
        }

        private void switchGenerateReg_Click(object sender, EventArgs e)
        {
            if (switchGenerateReg.Value)
            {
                _pdfSettings.WriteSettingsToFile(true.ToString());
            }
            else
            {
                _pdfSettings.WriteSettingsToFile(false.ToString());
            }

            RegistryNumber();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
            errorProvider1.Clear();
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == string.Empty)
            {
                LoadAllDepartments();
            }
            else
            {
                var searchedResult = _department.SearchForDepartment(txtSearch.Text.Trim());
                if (searchedResult.Count() > 0)
                {
                    cboDepartment.DataSource = null;
                    cboDepartment.DataSource = searchedResult.ToList();
                    cboDepartment.DisplayMember = "DepartmentName";
                    cboDepartment.ValueMember = "Id";
                    lblDepartments.Text = $"{searchedResult.Count()} Items Matched";
                }
                else
                {
                    lblDepartments.Text = "No records matched";
                    cboDepartment.DataSource = null;
                }
            }
        }

        /// <summary>
        /// Validates the form controls
        /// </summary>
        /// <returns><
        /// A value indicating whether form is valid or not.
        /// /returns>
        private bool IsValidForm()
        {
            bool valid = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtRegistryNumber.Text.Trim()))
            {
                errorProvider1.SetError(txtRegistryNumber, "Registry number is required");
                valid = false;
            }

            if (string.IsNullOrEmpty(txtPersonSent.Text.Trim()))
            {
                errorProvider1.SetError(txtPersonSent, "Person sent is required");
                valid = false;
            }

            if (string.IsNullOrEmpty(txtSubject.Text.Trim()))
            {
                errorProvider1.SetError(txtSubject, "Subject is required");
                valid = false;
            }

            if (string.IsNullOrEmpty(txtFromDepartment.Text.Trim()))
            {
                errorProvider1.SetError(txtFromDepartment, "Department/Person from is required");
                valid = false;
            }

            if (cboDepartment.SelectedIndex == -1)
            {
                errorProvider1.SetError(btnAddDepartments, "Department is required");
                valid = false;
            }

            if (txtFileName.Text.Trim() != string.Empty)
            {

                FileSettings settings = new FileSettings()
                {
                    FileName = txtFileName.Text.Trim()
                };

                string fileLocation = settings.GetFileLocation();
                if (File.Exists(fileLocation))
                {
                    errorProvider1.SetError(txtFileName, "A file withe the same already exist in working folder.\n Choose a different file name");
                    valid = false;
                }
            }
            return valid;
        }

        private string PdfName()
        {
            if (txtFileName.Text.Trim() == string.Empty)
            {
                return $"File_{txtRegistryNumber.Text}_{DateTime.Now.ToString("hh_mm_ss")}.pdf";
            }
            else
            {
                return txtFileName.Text.Trim().ToLower()+".pdf";
            }

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                string fileName = null;
                if (txtPdfName.Text.Trim() != string.Empty)
                {
                    fileName = PdfName();
                }

                //Try to save pdf to folder
                if (fileName != null)
                {
                    if (File.Exists(txtPdfName.Text.Trim()))
                    {
                        _pdfSettings.CreateFile(fileName, txtPdfName.Text.Trim());

                    }
                    else
                    {
                        MessageBox.Show($"The pdf you want to save has been \n moved from the location {txtPdfName.Text.Trim()}\n Please check the file again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }

                //Save the file to the database
                IncomingFileModel fileModel = new IncomingFileModel()
                {
                    RegistryNumber = int.Parse(txtRegistryNumber.Text.Trim()),
                    DateReceived = dtpDateReceived.Value,
                    PersonSent = txtPersonSent.Text.Trim().ToLower(),
                    DateOfLetter = dtpDateofLetter.Value,
                    Subject = txtSubject.Text.Trim().ToLower(),
                    DepartmentSent = txtFromDepartment.Text.Trim().ToLower(),
                    Department = (DepartmentModel)cboDepartment.SelectedItem,
                    FileName = fileName
                };

                if (txtReferenceNumber.Text.Trim() != string.Empty)
                    fileModel.ReferenceNumber = txtReferenceNumber.Text.Trim();
                if (txtRemarks.Text.Trim() != string.Empty)
                    fileModel.Remarks = txtRemarks.Text.Trim();
                //Save the file to the database
                await _incomingFile.CreateIncomingFile(fileModel);
                MessageBox.Show("File information has been successfully saved", "Save File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAllFiles();
                ResetControls();
            }
        }

        private void btnRegistrySearch_Click(object sender, EventArgs e)
        {
            if (txtRegistrySearch.Text.Trim() != string.Empty)
            {
                int registryNumber = 0;
                if (int.TryParse(txtRegistrySearch.Text.Trim(), out registryNumber))
                {
                    if (allFiles.Count() > 0)
                    {
                        selectedFileToUpdate = allFiles.FirstOrDefault(x => x.RegistryNumber == registryNumber);
                        if (selectedFileToUpdate != null)
                        {
                            btnNext.Enabled = true;
                            btnPrevious.Enabled = true;
                            itemsToTake = registryNumber;
                            ToggleNavigationButtons();
                            //Populate records into controls
                            PopulateControls(selectedFileToUpdate);
                            ToggleButtonStates();
                        }
                        else
                        {
                            MessageBox.Show("File not Found. Invalid Registry Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Files Found. Please Register Files", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Registry Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void txtRegistrySearch_OnValueChanged(object sender, EventArgs e)
        {
            if (txtRegistrySearch.Text.Trim() == string.Empty)
            {
                if (selectedFileToUpdate != null)
                {
                    ResetControls();
                    selectedFileToUpdate = null;
                }
            }
        }

        private void ToggleNavigationButtons()
        {
            if (allFiles.Count() == 0)
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                if (itemsToTake == 1)
                {
                    btnNext.Enabled = true;
                    btnPrevious.Enabled = false;
                }

                if (itemsToTake == allFiles.Count())
                {
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = true;
                }

                if (itemsToTake == 0)
                {
                    btnNext.Enabled = true;
                    btnPrevious.Enabled = true;
                }
            }

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
            if (allFiles.Count() > 0)
            {
                //Select the last element and decrease selection by 1
                if (itemsToTake == 0)
                {
                    selectedFileToUpdate = allFiles.Last();
                    PopulateControls(selectedFileToUpdate);
                    itemsToTake = allFiles.Count();
                }
                else if (itemsToTake <= allFiles.Count())
                {
                    selectedFileToUpdate = allFiles.Take(itemsToTake - 1).Last();
                    PopulateControls(selectedFileToUpdate);
                    itemsToTake--;
                }
            }
            ToggleNavigationButtons();
            ToggleButtonStates();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnPrevious.Enabled = true;
            if (allFiles.Count() > 0)
            {
                //Select the first element and increase selection by 1.
                if (itemsToTake == 0)
                {
                    selectedFileToUpdate = allFiles.First();
                    PopulateControls(selectedFileToUpdate);
                    itemsToTake++;
                }
                else if (itemsToTake <= allFiles.Count())
                {
                    selectedFileToUpdate = allFiles.Skip(itemsToTake).First();
                    PopulateControls(selectedFileToUpdate);
                    itemsToTake++;
                }
            }

            ToggleNavigationButtons();
            ToggleButtonStates();
        }

        public void PopulateControls(IncomingFileModel result)
        {
            txtRegistryNumber.Text = result.RegistryNumber.ToString();
            dtpDateReceived.Value = result.DateReceived;
            txtPersonSent.Text = result.PersonSent;
            dtpDateofLetter.Value = result.DateOfLetter;
            txtReferenceNumber.Text = result?.ReferenceNumber;
            txtSubject.Text = result.Subject;
            txtFromDepartment.Text = result.DepartmentSent;
            txtRemarks.Text = result?.Remarks;
            txtPdfName.Text = result?.FileName;
            cboDepartment.SelectedValue = result.Department.Id;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (selectedFileToUpdate != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this file?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _incomingFile.DeleteFile(selectedFileToUpdate);
                        //Delete the file from the folder as well
                        if (selectedFileToUpdate.FileName != null)
                            _pdfSettings.DeleteFile(selectedFileToUpdate.FileName);
                        MessageBox.Show("File has been successfully deleted", "Delete File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetControls();
                        LoadAllFiles();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"Sorry, there was an error while deleting file. \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
        }

        private void chkRemoveFile_OnChange(object sender, EventArgs e)
        {
            if (chkRemoveFile.Checked)
            {
                if (selectedFileToUpdate != null && selectedFileToUpdate.FileName != null)
                {
                    //Confirm from the user before deleting the file

                    if (MessageBox.Show($"Are you sure you want to delete the pdf {selectedFileToUpdate.FileName}", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            FileSettings settings = new FileSettings();
                            settings.DeleteFile(selectedFileToUpdate.FileName);
                            //Update the database
                            selectedFileToUpdate.FileName = null;
                            _incomingFile.UpdateFile(selectedFileToUpdate);
                            txtPdfName.Text = "";
                            MessageBox.Show("File has been successfully deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Sorry an erorr occured while deleting file \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                else if (txtPdfName.Text != string.Empty)
                {
                    //Don't delete any file. Just clear the text box
                    txtPdfName.Text = "";
                }
            }

        }

        private void txtPdfName_OnValueChanged(object sender, EventArgs e)
        {
            chkRemoveFile.Checked = false;
        }

        private async void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (selectedFileToUpdate != null)
            {
                if (IsValidForm())
                {

                    if (MessageBox.Show("Are you sure you want to update this file", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //***********************
                        string fileName = null;
                        //First check whether selectedfileToUpdate has a filename
                        if (selectedFileToUpdate.FileName != null)
                        {
                            //Lets check whether the filename has changed
                            if (selectedFileToUpdate.FileName == txtPdfName.Text)
                            {
                                //FileName has not been changed by the user
                                fileName = selectedFileToUpdate.FileName;
                            }
                            else
                            {
                                //The File has been changed by the user. So try to delete the previous file and create the new file
                                //Delete file from folder first
                                _pdfSettings.FileName = selectedFileToUpdate.FileName;
                                if (File.Exists(_pdfSettings.GetFileLocation()))
                                    _pdfSettings.DeleteFile(selectedFileToUpdate.FileName);

                                if (txtPdfName.Text.Trim() != string.Empty)
                                {
                                    //Generates a new filename for the file
                                    fileName = PdfName();

                                    //Creates the file in the working folder
                                    if (File.Exists(txtPdfName.Text.Trim()))
                                    {
                                        _pdfSettings.CreateFile(fileName, txtPdfName.Text.Trim());

                                    }
                                    else
                                    {
                                        MessageBox.Show($"The pdf you want to update has been \n moved from the location {txtPdfName.Text.Trim()}\n Please check the file again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }



                        //update the file to the database
                        selectedFileToUpdate.RegistryNumber = int.Parse(txtRegistryNumber.Text.Trim());
                        selectedFileToUpdate.DateReceived = dtpDateReceived.Value;
                        selectedFileToUpdate.PersonSent = txtPersonSent.Text.Trim().ToLower();
                        selectedFileToUpdate.DateOfLetter = dtpDateofLetter.Value;
                        selectedFileToUpdate.Subject = txtSubject.Text.Trim().ToLower();
                        selectedFileToUpdate.DepartmentSent = txtFromDepartment.Text.Trim().ToLower();
                        selectedFileToUpdate.Department = (DepartmentModel)cboDepartment.SelectedItem;
                        selectedFileToUpdate.FileName = fileName;

                        if (txtReferenceNumber.Text.Trim() != string.Empty)
                            selectedFileToUpdate.ReferenceNumber = txtReferenceNumber.Text.Trim();
                        if (txtRemarks.Text.Trim() != string.Empty)
                            selectedFileToUpdate.Remarks = txtRemarks.Text.Trim();
                        //Update the file to the database
                        await _incomingFile.UpdateFile(selectedFileToUpdate);
                        MessageBox.Show("File information has been successfully updated", "Update File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllFiles();
                        ResetControls();
                        //********************** 
                    }
                }


            }
        }

        private void frmFile_Load(object sender, EventArgs e)
        {
            ToggleButtonStates();
        }

        private void btnDepartments_Click(object sender, EventArgs e)
        {
            frmAllFiles frm = new frmAllFiles();
            frm.Show();
            this.Close();
        }

        private void frmFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.Count == 0)
            {
                Application.Exit();
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void header_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtFileName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsLetter(e.KeyChar))
                e.Handled = true;
        }

        private void txtPersonSent_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlValidators.ValidateLength(txtPersonSent, e, 150);
        }

        private void txtReferenceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlValidators.ValidateLength(txtReferenceNumber, e, 100);
        }

        private void txtSubject_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlValidators.ValidateLength(txtSubject, e, 150);
        }

        private void txtRegistryNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
