using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistryLibrary.Abstracts
{
    public delegate void TryPeriod(string message);
    public delegate void KeyApplied(bool applied);
    public interface ILicenseData
    {
        Task<LicenseModel> InsertLicense(LicenseModel license);       
        Task<bool> ConfirmLicense(LicenseModel license);
        Task InsertInstalledDate(DateTime InstalledDate);
        Task InsertCurrentDate(DateTime currentDate);
        Task<bool> HasExpired(TryPeriod period, KeyApplied key);
    }
}
