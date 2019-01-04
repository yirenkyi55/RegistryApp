using System;
using System.IO;
using System.Configuration;
namespace RegistryLibrary.Infrastructure
{

    public class FileSettings
    {
        public string FileName { get; set; }
        private string settingsFile = "file.txt";
        private string fullFilePath;
        public string SourceFilePath { get; private set; }
        public event EventHandler<string> GetSourceFilePathEvent;

        /// <summary>
        /// Gets the source of the file you want to copy
        /// </summary>
        /// <param name="sourceFilePath">
        /// The file path of the file you want to read
        /// </param>
        public void GetSourceFile(string sourceFilePath)
        {
            SourceFilePath = sourceFilePath;
            GetSourceFilePathEvent?.Invoke(this, SourceFilePath);
        }

        string[] folderNames =
        {
           @"Workspace\",
           @"Workspace\Settings\",
           @"Workspace\Files\"
        };

        /// <summary>
        /// File Settings for the file you want to create/ access
        /// </summary>
        public FileSettings()
        {

        }

        /// <summary>
        /// File Settings for the file you want to create/ access
        /// </summary>
        /// <param name="fileName">
        /// The name of the file you want to create/access
        /// </param>
        /// <param name="sourceFilePath">
        /// The source of the file you want to copy file from
        /// </param>
        public FileSettings(string sourceFilePath, string fileName)
        {
            FileName = fileName;
            SourceFilePath = sourceFilePath;
            fullFilePath = GetFolderByName(Folders.Files) + fileName;
        }

        /// <summary>
        /// Gets the name of the folder you want to work with
        /// </summary>
        /// <param name="folders">
        /// The folder name you want to work with(Enum)
        /// </param>
        /// <returns>
        /// The folder as specified
        /// </returns>
        private string GetFolderByName(Folders folders)
        {
            var folderLocation = ConfigurationManager.AppSettings["folders"];
            return folderLocation + folderNames[(int)folders];
        }

        /// <summary>
        /// Creates all working folders/Directories in the bin folder if folders do not exist
        /// </summary>
        private void CreateFolders()
        {
            foreach (var folder in Enum.GetNames(typeof(Folders)))
            {
                var folderEnum = (Folders)Enum.Parse(typeof(Folders), folder);
                if (!Directory.Exists(GetFolderByName(folderEnum)))
                {
                    Directory.CreateDirectory(GetFolderByName(folderEnum));
                }
            }
        }

        /// <summary>
        /// Copy the file into the working folder
        /// </summary>
        /// <param name="sourceFilePath">
        /// The source of the file you want to copy from
        /// </param>
        /// <param name="fileName">
        /// The file name you want to copy
        /// </param>
        public void CreateFile(string fileName, string sourceFilePath)
        {
            FileName = fileName;
            SourceFilePath = sourceFilePath;
            //Create folders in not exist
            CreateFolders();
            fullFilePath = GetFolderByName(Folders.Files) + FileName;

            if (!File.Exists(fullFilePath))
            {
                //Creates the file
                File.Copy(SourceFilePath, fullFilePath);
            }
        }

        /// <summary>
        /// Delete the file from the Files folder
        /// </summary>
        /// <param name="fileName">
        /// The name of the file you want to delete 
        /// </param>
        /// <returns>
        /// A boolean value indicating the success/failure
        /// </returns>
        public bool DeleteFile(string fileName)
        {
            FileName = fileName;
            fullFilePath = GetFolderByName(Folders.Files) + fileName;
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the fileLocation of the file you want to access
        /// </summary>
        /// <param name="fileName">
        /// The name of the file you want to access
        /// </param>
        /// <returns>
        /// A string of the file location you want to access.
        /// </returns>
        public string GetFileLocation()
        {

            fullFilePath = GetFolderByName(Folders.Files) + FileName;
            if (File.Exists(fullFilePath))
            {
                return fullFilePath;
            }
            return null;
        }


        /// <summary>
        /// Write Settings to file
        /// </summary>
        /// <param name="text">
        /// The message you want to write to the file
        /// </param>
        public void WriteSettingsToFile(string text)
        {
            CreateFolders();
            File.WriteAllText(GetFolderByName(Folders.Settings) + settingsFile, text);
        }

        /// <summary>
        /// Reads all value from the settings file
        /// </summary>
        /// <returns>
        /// A boolean value indicating file status
        /// </returns>
        public bool ReadSettingsFromFile()
        {
            CreateFolders();
            string path = GetFolderByName(Folders.Settings) + settingsFile;
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                return bool.Parse(text);
            }
            else
            {
                return false;
            }
        }
    }
}
