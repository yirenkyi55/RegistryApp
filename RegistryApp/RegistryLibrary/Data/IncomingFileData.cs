using RegistryLibrary.Abstracts;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace RegistryLibrary.Data
{
    public class IncomingFileData : IIncomingFileData
    {



        /// <summary>
        /// Creates a new File into the database
        /// </summary>
        /// <param name="incomingFile">
        /// The file  you want to create
        /// </param>
        /// <returns>
        /// The newly created file.
        /// </returns>
        public async Task<IncomingFileModel> CreateIncomingFile(IncomingFileModel incomingFile)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@RegistryNumber", incomingFile.RegistryNumber);
                param.Add("@DateReceived", incomingFile.DateReceived);
                param.Add("@PersonSent", incomingFile.PersonSent);
                param.Add("@DateOfLetter", incomingFile.DateOfLetter);
                param.Add("@ReferenceNumber", incomingFile.ReferenceNumber);
                param.Add("@Subject", incomingFile.Subject);
                param.Add("@DepartmentSent", incomingFile.DepartmentSent);
                param.Add("@FileName", incomingFile.FileName);
                param.Add("@Remarks", incomingFile.Remarks);
                param.Add("@DepartmentId", incomingFile.Department.Id);
                param.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("spIncomingFile_Create", param, commandType: CommandType.StoredProcedure);
                incomingFile.Id = param.Get<int>("@Id");
                return incomingFile;

            }
        }

        /// <summary>
        /// Deletes all Files from the database
        /// </summary>
        /// <returns>
        /// Returns a boolean value indicating success or failure
        /// </returns>
        public async Task<bool> DeleteAllFile()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                int Id = await connection.ExecuteAsync("spIncomingFile_DeleteAll", commandType: CommandType.StoredProcedure);
                return Id > 0;
            }
        }

        /// <summary>
        /// Delete a file from the database
        /// </summary>
        /// <param name="incomingFile">
        /// The file you wish to delete
        /// </param>
        /// <returns>
        /// A returns a boolean value indicating success or failure.
        /// </returns>
        public async Task<bool> DeleteFile(IncomingFileModel incomingFile)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", incomingFile.Id);
                int result = await connection.ExecuteAsync("spIncomingFile_Delete", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        /// <summary>
        /// Calculates the next registry number
        /// </summary>
        /// <returns>
        /// The next registry number (integer)
        /// </returns>
        public int NextRegistryNumber()
        {
            var files = SelectAllFiles();
            if (files.Count() > 0)
            {
                return files.OrderByDescending(x => x.RegistryNumber).Select(x => x.RegistryNumber).First() + 1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Search for a file in the database
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<IncomingFileModel> SearchForFile(string search)
        {
            var searchResult = new List<IncomingFileModel>().AsEnumerable();
            var files = SelectAllFiles();
            search = search.ToLower();
            //Find the same match.
            if (files.Count() > 0)
            {
                searchResult = files.Where(x => x.FileName.ToLower() == search);
                //Find whether there is a match
                if (searchResult.Count() > 0)
                {
                    return searchResult;
                }
                else
                {
                    //Search for a new pattern
                    searchResult = files.Where(x => x.FileName.ToLower().Contains(search));
                    return searchResult;
                }
            }
            else
            {
                return searchResult;
            }
        }

        /// <summary>
        /// Selects all files from the database
        /// </summary>
        /// <returns>
        /// A list of all files.
        /// </returns>
        public IEnumerable<IncomingFileModel> SelectAllFiles()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                return connection.Query<IncomingFileModel, DepartmentModel, IncomingFileModel>("spIncomingFile_SelectAll",

                    (file, department) => { file.Department = department; return file; },

                    commandType: CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="incomingFile">
        /// The File to update
        /// </param>
        /// <returns>
        /// A boolean value indicating success or failure.
        /// </returns>
        public async Task<bool> UpdateFile(IncomingFileModel incomingFile)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@RegistryNumber", incomingFile.RegistryNumber);
                param.Add("@DateReceived", incomingFile.DateReceived);
                param.Add("@PersonSent", incomingFile.PersonSent);
                param.Add("@DateOfLetter", incomingFile.DateOfLetter);
                param.Add("@ReferenceNumber", incomingFile.ReferenceNumber);
                param.Add("@Subject", incomingFile.Subject);
                param.Add("@DepartmentSent", incomingFile.DepartmentSent);
                param.Add("@FileName", incomingFile.FileName);
                param.Add("@Remarks", incomingFile.Remarks);
                param.Add("@DepartmentId", incomingFile.Department.Id);
                param.Add("@Id", incomingFile.Id);
                int result = await connection.ExecuteAsync("spIncomingFile_Update", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public List<IncomingFileModel> SearchForFile(List<IncomingFileModel> files, string search, SearchCriteria criteria)
        {
            switch (criteria)
            {
                case SearchCriteria.RegistryNumber:
                    int registryNumber = 0;
                    if (int.TryParse(search, out registryNumber))
                    {
                        //Search for the registry number
                        var results = files.Where(f => f.RegistryNumber == registryNumber);
                        return results.ToList();
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Registry Number. Please provide a number");
                    }
                case SearchCriteria.PersonSent:
                    var personResults = files.Where(f => f.PersonSent.ToLower().StartsWith(search.ToLower()) || f.PersonSent.ToLower().Contains(search.ToLower()));
                    return personResults.ToList();
                case SearchCriteria.DateReceived:
                    var dateResults = files.Where(f => f.DateReceived.Date.ToString().Contains(search));
                    return dateResults.ToList();
                case SearchCriteria.DepartmentTo:
                    var departmentSearch = files.Where(f => f.Department.DepartmentName.ToLower().StartsWith(search.ToLower()) || f.Department.DepartmentName.ToLower().Contains(search.ToLower()));
                    return departmentSearch.ToList();
                case SearchCriteria.Remarks:
                    var remarksSearch = files.Where(f => f.Remarks != null && f.Remarks.ToLower().Contains(search));
                    return remarksSearch.ToList();
                case SearchCriteria.FileName:
                    var fileNameResults = files.Where(f => f.FileName != null && (f.FileName.ToLower().StartsWith(search.ToLower()) || f.FileName.ToLower().Contains(search.ToLower())));
                    return fileNameResults.ToList();
                default:
                    return new List<IncomingFileModel>();
            }
        }


    }
}
