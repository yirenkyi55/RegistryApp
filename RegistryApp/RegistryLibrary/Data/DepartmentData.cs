using RegistryLibrary.Abstracts;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace RegistryLibrary.Data
{
    public class DepartmentData : IDepartmentData
    {
        public async Task<DepartmentModel> CreateDepartment(DepartmentModel department)
        {
           
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@DepartmentName", department.DepartmentName);
                param.Add("@Address",department.Address);
                param.Add("@Email",department.Email);
                param.Add("@Id",0,dbType:DbType.Int32,direction:ParameterDirection.Output);

                 await connection.ExecuteAsync("spDepartments_Create", param, commandType: CommandType.StoredProcedure);
                department.Id = param.Get<int>("@Id");
                return department;
            }
        }

        public async Task<bool> DeleteAllDepartment()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                int Id = await connection.ExecuteAsync("spDepartments_DeleteAll", commandType: CommandType.StoredProcedure);
                return Id > 0;
            }
        }

        public async Task<bool> DeleteDepartment(DepartmentModel department)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", department.Id);
                int Id = await connection.ExecuteAsync("spDepartments_Delete", param, commandType: CommandType.StoredProcedure);
                return Id >0;
            }
        }

        public async Task<IEnumerable<DepartmentModel>> SelectAllDepartmentsAsync()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
              return  await connection.QueryAsync<DepartmentModel>("spDepartments_SelectAll",commandType:CommandType.StoredProcedure);
            }
        }

        public  IEnumerable<DepartmentModel> SelectAllDepartments()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                return connection.Query<DepartmentModel>("spDepartments_SelectAll", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdateDepartment(DepartmentModel department)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@DepartmentName", department.DepartmentName);
                param.Add("@Address", department.Address);
                param.Add("@Email", department.Email);
                param.Add("@Id", department.Id);
                int Id = await connection.ExecuteAsync("spDepartments_Update", param, commandType: CommandType.StoredProcedure);
                return Id > 0;
            }
        }

        /// <summary>
        /// Selects all matching departments based on search result
        /// </summary>
        /// <param name="departmentName">
        /// The name of the department you want to search for.
        /// </param>
        /// <returns>
        /// An enumerable of user search results.
        /// </returns>
        public IEnumerable<DepartmentModel> SearchForDepartment(string departmentName)
        {
            var allDepartments = SelectAllDepartments();
            if (allDepartments.Count()>0)
            {
                //Find a search pattern.
                var results = allDepartments.Where(d => d.DepartmentName.ToLower().StartsWith(departmentName.ToLower()));
                if (results.Count()>0)
                {
                    return results;
                }
                else
                {
                    //Search by a new pattern
                    results = allDepartments.Where(d => d.DepartmentName.ToLower().Contains(departmentName.ToLower()));
                    return results;
                }
            }
            else
            {
                return new List<DepartmentModel>().AsEnumerable();
            }
        }
    }
}
