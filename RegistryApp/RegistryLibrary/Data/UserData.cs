using Dapper;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Data
{
    public delegate void LoginDetails(UserModel user);

    public class UserData : IUserData
    {
        /// <summary>
        /// Creates a new user 
        /// </summary>
        /// <param name="user">The user you want to create</param>
        /// <returns>The newly created user.</returns>
        public async Task<UserModel> CreateUser(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Name", user.Name);
                param.Add("@FullName", user.FullName);
                param.Add("@Password", EncryptionData.EncryptData(user.Password));
                param.Add("@Question", user.Question);
                param.Add("@Answer", EncryptionData.EncryptData(user.Answer));
                param.Add("@AccessType", user.AccessType);
                param.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);

                await connection.ExecuteAsync("spUsers_Insert", param, commandType: CommandType.StoredProcedure);

                user.Id = param.Get<int>("@Id");
                return user;
            }
        }


        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="userId">The userid you want to delete.</param>
      
        public async Task<bool> DeleteUser(int userId)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", userId);
                var result = await connection.ExecuteAsync("spUser_Delete", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                return await connection.QueryAsync<UserModel>("spUser_SelectAll", commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Select a user from the database
        /// </summary>
        /// <param name="userName">Then name of the user you want to select</param>
        /// <returns></returns>
        public async Task<UserModel> SelectUser(string userName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Name", userName);
                return await connection.QueryFirstOrDefaultAsync<UserModel>("spUser_SelectUser", param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Update a user in the database
        /// </summary>
        /// <param name="user">The user you want to update</param>
     
        public async Task<bool> UpdateUser(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Name", user.Name);
                param.Add("@Password", EncryptionData.EncryptData(user.Password));
                param.Add("@Question", user.Question);
                param.Add("@Answer", EncryptionData.EncryptData(user.Answer));
                param.Add("@AccessType", user.AccessType);
                param.Add("@Id", user.Id);

                var result = await connection.ExecuteAsync("spUsers_Update", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }


        public async Task<bool> LogUser(UserModel user,LoginDetails loginUser)
        {
            //Select the user from the database
            var userFromDatabase = await SelectUser(user.Name);
            //Check if the user name exist
            if (userFromDatabase==null)
            {
                
                return false;
            }
            //Compare the password
            if (!EncryptionData.ValidateEncryptedData(user.Password, userFromDatabase.Password))
            {
               
                return false;
            }

            loginUser(userFromDatabase);
            return true;
        }

        public async Task<bool> ChangeAccess(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", user.Id);
                param.Add("AccessType", user.AccessType);

                var result = await connection.ExecuteAsync("spUsers_UpdateAccess", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<bool> ChangePassword(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", user.Id);
                param.Add("@Password", EncryptionData.EncryptData(user.Password));
                var result = await connection.ExecuteAsync("spUser_ChangePassword", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        
    }
}
