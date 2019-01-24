using RegistryLibrary.Abstracts;
using RegistryLibrary.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using RegistryLibrary.Infrastructure;
using System.Configuration;
using System.IO;

namespace RegistryLibrary.Data
{


    public class LicenseData : ILicenseData
    {
        private string keySettings = @"C:\ProgramData\PassReg\";
    

        private void CreateDirectory()
        {
            if (!Directory.Exists(keySettings))
            {
                Directory.CreateDirectory(keySettings);
            }
        }

        /// <summary>
        /// Reads from the file saved on users disk.
        /// </summary>
        /// <returns></returns>
        private bool ReadFromKey()
        {
            string path = keySettings + "reg.csv";
            CreateDirectory();
            if (File.Exists(path))
            {
                return bool.Parse(File.ReadAllText(path));
            }
            return false;
        }
        /// <summary>
        /// Confirms whether the license the user is inserting is valid or not.
        /// </summary>
        /// <param name="license">The license you want to apply</param>

        public async Task<bool> ConfirmLicense(LicenseModel license)
        {
            var data = await ReadLicense();
            if (data != null)
            {
                return EncryptionData.ValidateEncryptedData(license.LicenseKey, data.LicenseKey);
            }
            else
            {
                return false;
            }
        }

        private async Task<int> GetDateDifference()
        {
            var data = await ReadLicense();
            if (data != null)
            {
                //Get the current date and installed date
                if (data.CurrentDate != null && data.InstalledDate != null)
                {
                    TimeSpan dateDiff = (TimeSpan)(data.InstalledDate - data.CurrentDate);
                    if (dateDiff.Days >= 0)
                    {
                        return dateDiff.Days;
                    }
                    else
                    {
                        return -dateDiff.Days;
                    }
                }

                return 0;

            }
            else
            {
                return 0;
            }
        }


        public async Task<bool> HasExpired(TryPeriod period, KeyApplied key)
        {
                 string path = keySettings + "reg.csv";
            if (ReadFromKey())
            {
                //The product has already expired
                period($"Expired.");
                return true;
            }
            else
            {
                var data = await ReadLicense();
                int getUsedDays = await GetDateDifference();
                if (!data.Licensed)
                {
                    if (getUsedDays > 30)
                    {
                        //The product has expired
                        //The product has expired
                        period($"Expired.");
                        CreateDirectory();
                        File.WriteAllText(path, true.ToString());
                        return true;
                    }

                    //The product is not licensed
                    period($"Try Period. {30 - getUsedDays} days Left");
                    return false;

                }
                else
                {
                    //Data may be licensed
                    if (data.AppliedKey != null)
                    {
                        //The user has applied the key
                        period("Activated.");
                        key(true);
                        return false; //The product has been activated
                    }
                    else
                    {
                        //The product is not activated..
                        //Lets check for expiry date
                        if (getUsedDays > 30)
                        {
                            //The product has expired
                            CreateDirectory();
                            File.WriteAllText(path, true.ToString());
                            period($"Expired");
                            return true;
                        }
                        else
                        {
                            period($"Try Period. {30 - getUsedDays} days Left");
                            return false;//The product is in try period
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Inserts the current date into the database
        /// </summary>
        /// <param name="currentDate">The Current date of the user</param>
        /// <returns></returns>
        public async Task InsertCurrentDate(DateTime currentDate)
        {
            var data = await ReadLicense();
       

            if (data != null)
            {
                if (currentDate.Date == ((DateTime)data.InstalledDate).Date)
                {
                    return;
                }

                //Insert installed date for the user.
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
                {
                    var param = new DynamicParameters();
                    param.Add("@CurrentDate", currentDate);
                    await connection.ExecuteAsync("spLicense_CurrentDate", param, commandType: CommandType.StoredProcedure);
                }
            }
        }


        /// <summary>
        /// Inserts the installed date for the user for first time use
        /// </summary>
        /// <param name="InstalledDate">The installed date from the user.</param>

        public async Task InsertInstalledDate(DateTime InstalledDate)
        {
            var data = await ReadLicense();
            if (data != null)
            {
                if (data.InstalledDate == null)
                {
                    //Insert installed date for the user.
                    using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
                    {
                        var param = new DynamicParameters();
                        param.Add("@InstalledDate", InstalledDate);
                        await connection.ExecuteAsync("spLicense_InstalledDate", param, commandType: CommandType.StoredProcedure);
                    }
                }
            }

        }

        /// <summary>
        /// Insert the license into the database
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public async Task<LicenseModel> InsertLicense(LicenseModel license)
        {
            string path = keySettings + "reg.csv";
            var data = await ReadLicense();

            if (data != null)
            {
                //Try to insert the applied license
                if (await ConfirmLicense(license))
                {
                    //Insert the license key
                    //Insert license for the user. at programming level
                    using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
                    {
                        license.Licensed = true;
                        license.AppliedKey = license.LicenseKey;

                        var param = new DynamicParameters();
                        param.Add("@Licensed", license.Licensed);
                        param.Add("@AppliedKey", EncryptionData.EncryptData(license.AppliedKey));                    
                        var result = await connection.ExecuteAsync("spLicense_Update", param, commandType: CommandType.StoredProcedure);                        
                        CreateDirectory();
                        File.WriteAllText(path, false.ToString());
                        return license;
                    }
                }
                else
                {
                    throw new Exception("Invalid Licensed Key. Please Insert the Right Key");
                }
            }
            else
            {
                //Insert license for the user. at programming level
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
                {
                    license.Licensed = false;
                    var param = new DynamicParameters();
                    param.Add("@LicenseKey", EncryptionData.EncryptData(license.LicenseKey));
                    param.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync("spLicense_InsertFirstUse", param, commandType: CommandType.StoredProcedure);
                    license.Id = param.Get<int>("@Id");
                    return license;
                }
            }
        }


        /// <summary>
        /// Reads all license information from the database
        /// </summary>
        /// <returns>A list of all the license</returns>
        private async Task<LicenseModel> ReadLicense()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString()))
            {
                var results = await connection.QueryFirstOrDefaultAsync<LicenseModel>("spLicenseRead", commandType: CommandType.StoredProcedure);
                return results;
            }
        }
    }
}
