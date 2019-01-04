using PagedList;
using RegistryAppUI.GridData;
using RegistryLibrary;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmSearchFile : Form
    {
        int pageNumber = 1;
        IPagedList<IncomingFileModel> list;
        DataGrids gridData = new DataGrids();
        List<IncomingFileModel> printerFiles = new List<IncomingFileModel>();
        IIncomingFileData fileData = new IncomingFileData();

        private async Task PopulateGrid(List<IncomingFileModel> myResults)
        {
            gridFiles.DataSource = null;
            list = await gridData.GetPagedListAsync(myResults);
            gridFiles.DataSource = gridData.FileDataTable(list);
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
            gridFiles.DataSource = gridData.FileDataTable(list);
            btnPrevious.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;
            lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";

        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {

                list = await gridData.GetPagedListAsync(++pageNumber);
                gridFiles.DataSource = gridData.FileDataTable(list);
                btnPrevious.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";

            }
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                list = await gridData.GetOnlyPagedFileListAsync(--pageNumber);
                gridFiles.DataSource = gridData.FileDataTable(list);
                btnPrevious.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                lblPageNumber.Text = $"page {pageNumber} of {list.PageCount}";
            }
        }


        public frmSearchFile()
        {
            InitializeComponent();
        }

        private async void frmSearchFile_Load(object sender, EventArgs e)
        {
            await ResetGrid();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (list.ToList().Count > 0)
            {
                if (txtSearch.Visible)
                {
                    if (txtSearch.Text.Trim() != string.Empty)
                    {

                        try
                        {
                            var myResults = fileData.SearchForFile(list.ToList(), txtSearch.Text.Trim(), (SearchCriteria)cboSearch.SelectedIndex);
                            if (myResults.Count > 0)
                            {
                                await PopulateGrid(myResults);
                            }
                            else
                            {
                                MessageBox.Show("No matching files Found", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (ArgumentException ex)
                        {

                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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
                    var myResults = fileData.SearchForFile(list.ToList(), search, (SearchCriteria)cboSearch.SelectedIndex);
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
                MessageBox.Show("No files to search for.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ResetGrid();
            if (txtSearch.Visible)
            {
                txtSearch.Clear();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            if (gridFiles.Rows.Count > 0)
            {
                //Find the selected id
                int selectedId = int.Parse(gridFiles.SelectedRows[0].Cells[0].Value.ToString());
                //Find the selected row
                var selectedFile = list.First(x => x.Id == selectedId);
                if (selectedFile.FileName!=null)
                {
                    //Modify the file
                    frmMailUsers.FileName = selectedFile.FileName;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There is no pdf file available for the selected file.\nPlease modify file to include pdf", "Add File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
        }
    }
}
