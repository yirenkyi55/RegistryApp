using System.Threading.Tasks;
using RegistryLibrary.Abstracts;
using RegistryLibrary.Models;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace RegistryLibrary.Data
{
    public class RegistryInfoData : IRegistryInfo
    {
        /// <summary>
        /// Get the registry information from the database
        /// </summary>       
        public async Task<RegistryInfoModel> GetRegistryInfo()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var registrys = await connection.QueryAsync<RegistryInfoModel>("spRegistry_Select", CommandType.StoredProcedure);
                return registrys.Count() > 0 ? registrys.First() : new RegistryInfoModel();
            }
        }


        public RegistryInfoModel GetRegistryInfoPrint()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var registrys =  connection.Query<RegistryInfoModel>("spRegistry_Select", CommandType.StoredProcedure);
                return registrys.Count() > 0 ? registrys.First() : new RegistryInfoModel();
            }
        }


        /// <summary>
        /// Save Registry Information into the database
        /// </summary>
        /// <param name="registry">
        /// The registry data, you want to save
        /// </param>  
        public async Task<bool> SaveRegistryInfo(RegistryInfoModel registry)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                //Check if registryId is 0
                if (registry.Id == 0)
                {
                    //This is a new record so save
                    var param = new DynamicParameters();
                    param.Add("@MunicipalName", registry.MunicipalName);
                    param.Add("@RegistryName", registry.RegistryName);
                    param.Add("@Address", registry.Address);
                    param.Add("@Email", registry.Email);
                    param.Add("@Contact", registry.Contact);
                    param.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                    if (registry.PicData!=null)
                    {
                        param.Add("@PicName", registry.PicName);
                        param.Add("@PicData", registry.PicData);
                        await connection.ExecuteAsync("spRegistry_Create", param, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        await connection.ExecuteAsync("spRegistry_CreateNoImage", param, commandType: CommandType.StoredProcedure);
                    }              
                                        
                    registry.Id = param.Get<int>("@Id");
                    return registry.Id > 0;
                }
                else
                {
                    //Update the record;
                    var param = new DynamicParameters();
                    param.Add("@MunicipalName", registry.MunicipalName);
                    param.Add("@RegistryName", registry.RegistryName);
                    param.Add("@Address", registry.Address);
                    param.Add("@Email", registry.Email);
                    param.Add("@Contact", registry.Contact);               
                    param.Add("@Id", registry.Id);
                    int result = 0;
                    if (registry.PicData != null)
                    {
                        param.Add("@PicName", registry.PicName);
                        param.Add("@PicData", registry.PicData);
                        result = await connection.ExecuteAsync("spRegistry_Update", param, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        result = await connection.ExecuteAsync("spRegistry_UpdateNoImage", param, commandType: CommandType.StoredProcedure);
                    }                    
                    return result > 0;
                }
            }
        }
    }
}
