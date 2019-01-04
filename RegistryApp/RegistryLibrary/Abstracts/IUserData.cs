using RegistryLibrary.Data;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Abstracts
{
    public interface IUserData
    {
        Task<UserModel> CreateUser(UserModel user);
        Task<bool> UpdateUser(UserModel user);
        Task<bool> DeleteUser(int userId);
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel> SelectUser(string userName);
        Task<bool> LogUser(UserModel user, LoginDetails loginUser);
        Task<bool> ChangeAccess(UserModel user);
    }
}
