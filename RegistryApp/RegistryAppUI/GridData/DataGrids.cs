
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RegistryLibrary.Models;
using System.Globalization;
using PagedList;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;

namespace RegistryAppUI.GridData
{
    public class DataGrids
    {
        //Create an event to populate gridview when user is creating new products
        public event EventHandler<DataTable> departmentGridEvent;
        private IIncomingFileData fileData = new IncomingFileData();
        /// <summary>
        /// Creates a department table to populate gridview
        /// </summary>
        /// <param name="departments">
        /// the list of department you want to populate in gridview
        /// </param>
        /// <returns>
        /// A datatable for the gridview
        /// </returns>
        public DataTable DepartmentTable(IEnumerable<DepartmentModel> departments)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Email", typeof(string));

            foreach (var department in departments)
            {
                department.DepartmentName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(department.DepartmentName);
                table.Rows.Add(department.Id, department.DepartmentName, department?.Address, department?.Email);
            }

            //Invoke the event
            departmentGridEvent?.Invoke(this, table);
            return table;
        }

        public async Task<IPagedList<IncomingFileModel>> GetPagedListAsync(int pageNumber = 1, int pageSize = 15)
        {
            return await Task.Factory.StartNew(() =>
            {
                return fileData.SelectAllFiles().OrderBy(f => f.Id).ToPagedList(pageNumber, pageSize);
            });
        }

        public async Task<IPagedList<IncomingFileModel>> GetOnlyPagedFileListAsync(int pageNumber = 1, int pageSize = 15)
        {
            return await Task.Factory.StartNew(() =>
            {
                return fileData.SelectAllFiles().Where(f=>f.FileName!=null).OrderBy(f => f.Id).ToPagedList(pageNumber, pageSize);
            });
        }


        public async Task<IPagedList<IncomingFileModel>> GetPagedListAsync(List<IncomingFileModel> files, int pageNumber = 1, int pageSize = 15)
        {
            return await Task.Factory.StartNew(() =>
            {
                return files.OrderBy(f => f.Id).ToPagedList(pageNumber, pageSize);
            });
        }

        public DataTable FileDataTable(IPagedList<IncomingFileModel> allFiles)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Registry Number", typeof(int));
            table.Columns.Add("Received Date", typeof(string));
            table.Columns.Add("Person Sent", typeof(string));
            table.Columns.Add("Date of Letter", typeof(string));
            table.Columns.Add("Reference No", typeof(string));
            table.Columns.Add("Subject", typeof(string));
            table.Columns.Add("From", typeof(string));
            table.Columns.Add("File", typeof(string));
            table.Columns.Add("Remarks", typeof(string));
            table.Columns.Add("Department To", typeof(string));

            foreach (var file in allFiles.ToList())
            {
                table.Rows.Add(file.Id, file.RegistryNumber,file.DateReceived.ToString("dd/MM/yyy"),
                    file.PersonSent, file.DateOfLetter.ToString("dd/MM/yyyy"), file?.ReferenceNumber ?? "",
                    file.Subject, file?.DepartmentSent ?? "", file?.FileName ?? "",
                    file?.Remarks ?? "", file.Department.DepartmentName);
            }

            return table;
        }
    }
}
