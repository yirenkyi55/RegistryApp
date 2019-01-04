using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Abstracts
{
    public interface IDepartmentData
    {
        Task<DepartmentModel> CreateDepartment(DepartmentModel department);
        Task<bool> UpdateDepartment(DepartmentModel department);
        Task<bool> DeleteDepartment(DepartmentModel department);
        Task<bool> DeleteAllDepartment();
        IEnumerable<DepartmentModel> SelectAllDepartments();
        IEnumerable<DepartmentModel> SearchForDepartment(string departmentName);
    }
}
