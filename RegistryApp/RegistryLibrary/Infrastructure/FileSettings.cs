using System;
using System.IO;
using System.Configuration;


namespace RegistryLibrary.Infrastructure
{

    public class FileSettings
    {
        public string FileName { get; set; }
        private string settingsFile = "file.txt";
        private string backupFile = "backup.txt";

        private string fullFilePath;
        public string SourceFilePath { get; private set; }
        public event EventHandler<string> GetSourceFilePathEvent;
        string folderLocation = ConfigurationManager.AppSettings["folders"];

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

            return folderLocation + folderNames[(int)folders];
        }

        /// <summary>
        /// Creates all working folders/Directories in the specified path folders do not exist
        /// </summary>
        public void CreateFolders()
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
        /// Creates bakup folders and files in the directory specified.
        /// </summary>
        /// <param name="sourcePath">The source path you want to create the backup files.</param>

        public void BackupFolders(string sourcePath)
        {
            //Create the folders in the specified path..
            string backupPath = sourcePath + @"\Reg_BackupFiles\";

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            foreach (var folder in folderNames)
            {
                if (!Directory.Exists(backupPath + folder))
                {
                    Directory.CreateDirectory(backupPath + folder);
                }
            }

            //Check and create folders if not exist
            CreateFolders();

            foreach (var folder in folderNames)
            {
                string backupFolder = backupPath + folder;
                //Fetch all files from the working directory to this current folder
                string currentWorkingPath = folderLocation + folder;
                var files = Directory.GetFiles(currentWorkingPath);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        string destinationFile = backupFolder + Path.GetFileName(file);
                        if (!File.Exists(destinationFile))
                        {
                            File.Copy(file, destinationFile); 
                        }
                    }
                }
            }
          
        }

        /// <summary>
        /// Restore from the backup folder to thee working folders
        /// </summary>
        /// <param name="sourcePath">The path you want to restore from</param>
        public void RestoreFolders(string sourcePath)
        {
            //First check if the backup folder exist
            string backupPath = sourcePath + @"\Reg_BackupFiles\";
            if (!Directory.Exists(backupPath))
            {
                throw new DirectoryNotFoundException($"The folder path '{backupPath}' to restore from does not exist.\n Please specify the right backup folder");
            }
          
                //The directory exists so lets check other directories
                foreach (var folder in folderNames)
                {
                    var fileFolder = backupPath + folder;
                    if (!Directory.Exists(fileFolder))
                    {
                        throw new DirectoryNotFoundException($"The folder path '{fileFolder}' to restore from does not exist.\n Please specify the right backup folder");
                    }
                }

            //All check is complete so lets backup from the backup directories

            //First lets check if the working directories exist, if not lets create the folders..
            CreateFolders();

            foreach (var folder in folderNames)
            {
                var backupFolderPath = backupPath + folder;
                var workingFolder = folderLocation + folder;
                var files = Directory.GetFiles(backupFolderPath);

                if (files.Length>0)
                {
                    foreach (var file in files)
                    {
                        //Copy the file from the backupfolder to the working folder
                        string destFile = workingFolder + Path.GetFileName(file);
                        if (!File.Exists(destFile))
                        {
                            File.Copy(file, destFile,true);
                        } 
                    }
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
        public void WriteSettingsToFile(string text,WriteToText fileType)
        {
            CreateFolders();
            string fileName;
            if (fileType==WriteToText.Settings)
            {
                fileName = settingsFile;
            }
            else
            {
                fileName = backupFile;
            }
            File.WriteAllText(GetFolderByName(Folders.Settings) + fileName, text);
        }

       

        /// <summary>
        /// Reads all value from the settings file
        /// </summary>
        /// <returns>
        /// A boolean value indicating file status
        /// </returns>
        public bool ReadSettingsFromFile(WriteToText fileType)
        {
            string fileName;
            if (fileType == WriteToText.Settings)
            {
                fileName = settingsFile;
            }
            else
            {
                fileName = backupFile;
            }

            CreateFolders();
            string path = GetFolderByName(Folders.Settings) + fileName;
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
