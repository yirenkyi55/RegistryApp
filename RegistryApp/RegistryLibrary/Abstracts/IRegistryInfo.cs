using RegistryLibrary.Models;
using System.Threading.Tasks;

namespace RegistryLibrary.Abstracts
{
    public interface IRegistryInfo
    {
        Task<bool> SaveRegistryInfo(RegistryInfoModel registry);
        Task<RegistryInfoModel> GetRegistryInfo();
    }
}
