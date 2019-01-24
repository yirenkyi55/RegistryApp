using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using RegistryAppUI.GridData;
using RegistryAppUI.Printer;
using RegistryLibrary;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;

namespace RegistryAppUI
{

    public partial class frmAllFiles : Form
    {

        public frmAllFiles()
        {
            InitializeComponent();
            LoadAllFiles();
        }
        int pageNumber = 1;
        IPagedList<IncomingFileModel> list;
        List<IncomingFileModel> printList = new List<IncomingFileModel>();
        List<IncomingFileModel> files = new List<IncomingFileModel>();
       DataGrids gridData = new DataGrids();
        //List<IncomingFileModel> printerFiles = new List<IncomingFileModel>();
        IIncomingFileData fileData = new IncomingFileData();

        private async Task PopulateGrid(List<IncomingFileModel> myResults)
        {
            gridFiles.DataSource = null;
            list = await gridData.GetPagedListAsync(myResults);
            gridFiles.DataSource = gridData.FileDataTable(list);
            FormatGrid();
            btnPrevious.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;
            lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";
            pageNumber = 1;
        }

        private async Task ResetGrid()
        {
            dtpSearch.Visible = false;
            txtSearch.Visible = true;
            cboSearch.SelectedIndex = 0;
            list = await gridData.GetPagedListAsync(pageNumber);
            FormatGrid();
            gridFiles.DataSource = gridData.FileDataTable(list);
            btnPrevious.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;
            lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";
        }

        private void FormatGrid()
        {
            if (gridFiles.Rows.Count > 0)
            {
                gridFiles.Columns[0].Visible = false;
            }
        }

        private void LoadAllFiles()
        {
           files = fileData.SelectAllFiles().ToList();
            lblTotalFiles.Text = $"TOTAL RECORDS WITH PDF FILE: {files.Where(f => f.FileName != null).Count()}";
            lblRegisteredFiles.Text = $"TOTAL REGISTERED RECORDS: {files.Count()}";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {

                list = await gridData.GetPagedListAsync(++pageNumber);
                gridFiles.DataSource = gridData.FileDataTable(list);
                FormatGrid();
                btnPrevious.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";

            }
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                list = await gridData.GetPagedListAsync(--pageNumber);
                gridFiles.DataSource = gridData.FileDataTable(list);
                FormatGrid();
                btnPrevious.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";
            }
        }

        private async void frmAllFiles_Load(object sender, EventArgs e)
        {

            await ResetGrid();
            FormatGrid();
        }

        private void gridFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSearch.SelectedIndex == 2)
            {
                dtpSearch.Visible = true;
                txtSearch.Visible = false;
                dtpSearch.Focus();
            }
            else
            {
                txtSearch.Visible = true;
                dtpSearch.Visible = false;
                txtSearch.Focus();
            }

            if (cboSearch.SelectedIndex == 5 || cboSearch.SelectedIndex == 6)
            {
                txtSearch.Text = "";
                txtSearch.Enabled = false;
                dtpSearch.Enabled = false;
            }
            else
            {
                txtSearch.Enabled = true;
                dtpSearch.Enabled = true;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            LoadAllFiles();

            if (cboSearch.SelectedIndex==6 || cboSearch.SelectedIndex==7)
            {
                //Do some stuffs over here
                var myResults = fileData.SearchForFile(files, null, (SearchCriteria)cboSearch.SelectedIndex);
                if (myResults.Count > 0)
                {
                    await PopulateGrid(myResults);
                }
                
                return;
            }

            if (list.ToList().Count > 0)
            {
                if (txtSearch.Visible )
                {
                    if (txtSearch.Text.Trim() != string.Empty)
                    {

                        try
                        {
                            var myResults = fileData.SearchForFile(files.ToList(), txtSearch.Text.Trim(), (SearchCriteria)cboSearch.SelectedIndex);
                            if (myResults.Count > 0)
                            {
                                await PopulateGrid(myResults);
                            }
                            else
                            {
                                MessageBox.Show("No matching records Found", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (ArgumentException ex)
                        {

                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(txtSearch, $"Enter {cboSearch.Text}");
                    }

                }
                else
                {
                    //Search by date
                    string search = dtpSearch.Value.Date.ToString();
                    //MessageBox.Show(dtpSearch.Value.Date.ToString("dd/MM/yyyy"));
                    var myResults = fileData.SearchForFile(files, search, (SearchCriteria)cboSearch.SelectedIndex);
                    if (myResults.Count > 0)
                    {
                        await PopulateGrid(myResults);
                    }
                    else
                    {
                        MessageBox.Show("No matching date Found", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Records to search for.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == string.Empty)
            {
                await ResetGrid();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ResetGrid();
            if (txtSearch.Visible)
            {
                txtSearch.Clear();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printList = fileData.SelectAllFiles().ToList();
            //Create the printdocument and attach an event handler
            PrintDocument doc = new FilesDocument(printList);
            doc.PrintPage += Doc_PrintPage;
            PrintDialog dlgSettings = new PrintDialog();
            dlgSettings.Document = doc;
            if (dlgSettings.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
                Logger.WriteToFile(Logger.FullName, "printed a file");
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            float lineHeight = e.MarginBounds.Top;
            FilesDocument files = (FilesDocument)sender;
            files.PrintFilesPage(e);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridFiles.Rows.Count > 0)
                {
                    //Find the selected id
                    int selectedId = int.Parse(gridFiles.SelectedRows[0].Cells[0].Value.ToString());
                    //Find the selected row
                    var selectedFile = list.First(x => x.Id == selectedId);
                    //Modify the file
                    frmFile frm = new frmFile();
                    frm.selectedFileToUpdate = selectedFile;
                    frm.PopulateControls(frm.selectedFileToUpdate);
                    this.Hide();
                    frm.BringToFront();
                    frm.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Sorry an error occured.\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridFiles.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete file record?", "Delete File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Find the selected id
                    int selectedId = int.Parse(gridFiles.SelectedRows[0].Cells[0].Value.ToString());
                    //Find the selected row
                    var selectedFile = list.First(x => x.Id == selectedId);
                    //Delete the file
                    await fileData.DeleteFile(selectedFile);
                    LoadAllFiles();
                    await ResetGrid();
                    Logger.WriteToFile(Logger.FullName, "deleted a file");
                    MessageBox.Show("File Record has been successfully deleted");
                }
            }
        }

        private async void btnDeleteAllFiles_Click(object sender, EventArgs e)
        {
            if (gridFiles.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete all file records?", "Delete All Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await fileData.DeleteAllFile();
                    LoadAllFiles();
                    await ResetGrid();
                    Logger.WriteToFile(Logger.FullName, "deleted all files");
                    MessageBox.Show("File Record has been successfully deleted");
                }
            }
        }

        private void btnViewPdf_Click(object sender, EventArgs e)
        {
            if (gridFiles.Rows.Count > 0)
            {
                //Find the selected id
                int selectedId = int.Parse(gridFiles.SelectedRows[0].Cells[0].Value.ToString());
                //Find the selected row
                var selectedFile = list.First(x => x.Id == selectedId);
                if (selectedFile.FileName != null)
                {
                    //View the file
                    FileSettings fileSettings = new FileSettings();
                    fileSettings.FileName = selectedFile.FileName;
                    frmUpdatePdf updatePdf = new frmUpdatePdf(fileSettings, true);
                    updatePdf.ShowDialog();
                }

            }
        }

        private void gridFiles_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridFiles.Rows.Count > 0)
                {
                    //Find the selected id
                    int selectedId = int.Parse(gridFiles.SelectedRows[0].Cells[0].Value.ToString());
                    //Find the selected row
                    var selectedFile = list.First(x => x.Id == selectedId);
                    if (selectedFile.FileName != null)
                    {
                        btnViewPdf.Visible = true;
                    }
                    else
                    {
                        btnViewPdf.Visible = false;
                    }
                }
            }
            catch (Exception)
            {


            }
        }
    }
}
