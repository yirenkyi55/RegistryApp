using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using RegistryLibrary;
using RegistryLibrary.Infrastructure;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using RegistryLibrary.Models;

namespace RegistryAppUI
{
    public partial class frmBackupRestore : Form
    {
        public frmBackupRestore()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isValidControl(bool restore = false)
        {
            bool valid = true;

            string restoreString = restore ? "restore" : "backup";

            if (txtPath.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtPath, $"Please specify {restoreString} path");
                valid = false;
            }


            return valid;
        }



        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowser.SelectedPath;
            }
        }

        private void BackupFiles()
        {

        }

        private void BackupDatabase(string sourcePath)
        {
            string databaseDir = Application.StartupPath + @"\Data\";
            string backupFolder = sourcePath + @"\database\";

            if (Directory.Exists(backupFolder))
            {
                Directory.Delete(backupFolder, true);
            }

            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }


            if (Directory.Exists(databaseDir))
            {
                var files = Directory.GetFiles(databaseDir);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        GC.Collect();
                        File.Copy(file, backupFolder + Path.GetFileName(file), true);
                    }
                }
            }
        }


        private void RestoreDatabase(string sourcePath)
        {
            string databasePath = Application.StartupPath + @"\Data\";
            string backupFolder = sourcePath + @"\database\";
            //First check if the backup folder exists..
            if (!Directory.Exists(backupFolder))
            {
                MessageBox.Show("The folder to restore from does not contain backup folder\n Please check backup folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var files = Directory.GetFiles(backupFolder);

            if (files.Length > 0)
            {
                int countDataFile = 0;
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.ToLower() == "registrydb.mdf")
                    {
                        countDataFile++;
                    }
                }

                if (countDataFile == 0)
                {
                    MessageBox.Show("The database to restore from does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Lets delete all working database 
                if (Directory.Exists(databasePath))
                {
                    Directory.Delete(databasePath, true);
                }

                //Lets create a new working database directory
                Directory.CreateDirectory(databasePath);


                foreach (var file in files)
                {
                    //We can now copy the files to the database path.
                    var databaseItem = databasePath + Path.GetFileName(file);

                    if (!File.Exists(databaseItem))
                    {
                        File.Copy(file, databaseItem);
                    }

                }

            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {

            progressBar.Value = 0;
            try
            {
                if (isValidControl())
                {
                    lblStatus.Text = "Backing Up Database. Please Wait...";
                    FileSettings settings = new FileSettings();
                    settings.BackupFolders(txtPath.Text);
                    BackupDatabase(txtPath.Text);
                    //Backup();
                    progressBar.Value = 100;
                    lblPercent.Text = "100 %";
                    lblStatus.Text = "Backup Complete";
                    MessageBox.Show("Database has been successfully bucked up", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Sorry an error occured while backing up database. \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnRestore_Click(object sender, EventArgs e)
        {

            progressBar.Value = 0;
            try
            {
                if (isValidControl(true))
                {
                    lblStatus.Text = "Restoring Database. Please Wait...";
                    FileSettings settings = new FileSettings();
                    settings.RestoreFolders(txtPath.Text);
                    RestoreDatabase(txtPath.Text);
                    //Restore();
                    progressBar.Value = 100;
                    lblPercent.Text = "100 %";
                    MessageBox.Show("Database has been successfully restored", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sorry an error occured while restoring database.\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Backup()
        {
            Server dbServer = new Server(new ServerConnection(new SqlConnection(GlobalConfig.ConnString())));
            Backup dbBackup = new Backup()
            {
                Action = BackupActionType.Database,
                Database = "RegistryDB"
            };

            dbBackup.Devices.AddDevice(txtPath.Text + @"\RegistryDB.bak", DeviceType.File);
            dbBackup.Initialize = true;
            dbBackup.PercentComplete += DbBackup_PercentComplete;
            dbBackup.Complete += DbBackup_Complete;
            dbBackup.SqlBackupAsync(dbServer);
        }

        private void DbBackup_Complete(object sender, ServerMessageEventArgs e)
        {
            if (e.Error != null)
            {
                lblStatus.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = e.Error.Message;
                });
            }

        }

        private void DbBackup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            progressBar.Invoke((MethodInvoker)delegate
            {
                progressBar.Value = e.Percent;
                progressBar.Update();
            });

            lblPercent.Invoke((MethodInvoker)delegate
            {
                lblPercent.Text = $"{e.Percent} %";
            });
        }


        private void Restore()
        {

            Server dbServer = new Server(new ServerConnection(new SqlConnection(GlobalConfig.ConnString())));

            Restore dbRestore = new Restore()
            {
                Database = "RegistryDB",
                Action = RestoreActionType.Database,
                ReplaceDatabase = true,
                NoRecovery = false
            };
            dbRestore.Devices.AddDevice(txtPath.Text + @"\RegistryDB.bak", DeviceType.File);
            dbRestore.PercentComplete += DbRestore_PercentComplete;
            dbRestore.Complete += DbRestore_Complete;
            dbRestore.SqlRestore(dbServer);
        }

        private void DbRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            if (e.Error != null)
            {
                lblStatus.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = e.Error.Message;
                });

            }
        }

        private void DbRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            progressBar.Invoke((MethodInvoker)delegate
            {
                progressBar.Value = e.Percent;
                progressBar.Update();
            });

            lblPercent.Invoke((MethodInvoker)delegate
            {
                lblPercent.Text = $"{e.Percent} %";
            });
        }
        private void frmBackupRestore_Load(object sender, EventArgs e)
        {

        }

        private void frmBackupRestore_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
