using RegistryLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistryLibrary.Abstracts
{
    public interface IIncomingFileData
    {
        Task<IncomingFileModel> CreateIncomingFile(IncomingFileModel incomingFile);
        Task<bool> UpdateFile(IncomingFileModel incomingFile);
        Task<bool> DeleteFile(IncomingFileModel incomingFile);
        Task<bool> DeleteAllFile();
        IEnumerable<IncomingFileModel> SelectAllFiles();
        int NextRegistryNumber();
        IEnumerable<IncomingFileModel> SearchForFile(string search);
        List<IncomingFileModel> SearchForFile(List<IncomingFileModel> files, string search, SearchCriteria criteria);
    }
}
