using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public partial class frmAllUsers : Form
    {
        IUserData user = new UserData();
        List<UserModel> users = new List<UserModel>();
        string userAccess;

        private async Task LoadAllUsers()
        {
            lsvUsers.Items.Clear();
            var myusers = await user.GetUsers();
            users = myusers.ToList();
            cboAccess.SelectedIndex = -1;
            btnChangeType.Enabled = false;
            btnDelete.Enabled = false;
            if (users.Count > 0)
            {
                //Populate users into the listview box
                foreach (var user in users)
                {
                    ListViewItem item = new ListViewItem(user.Name);
                    item.SubItems.Add(user.AccessType);
                    lsvUsers.Items.Add(item);

                }

            }
        }

        public frmAllUsers()
        {
            InitializeComponent();
            LoadAllUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {

                    var selectedUser = users[lsvUsers.FocusedItem.Index];
                    selectedUser.AccessType = cboAccess.Text.Trim();
                    await user.DeleteUser(selectedUser.Id);
                    await LoadAllUsers();
                    Logger.WriteToFile(Logger.FullName, $"Deleted the user {selectedUser.Name}");
                    MessageBox.Show("User has been successfully deleted", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured. \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnChangeType_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to change access type?", "Change user", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var selectedUser = users[lsvUsers.FocusedItem.Index];
                    selectedUser.AccessType = cboAccess.Text.Trim();
                    await user.ChangeAccess(selectedUser);
                    await LoadAllUsers();
                    Logger.WriteToFile(Logger.FullName, $"Changed {selectedUser.Name} access level to {selectedUser.AccessType}");
                    MessageBox.Show("Access Type has been successfully changed", "Change", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured while changing access. \n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lsvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvUsers.FocusedItem != null)
            {
                var selectedUser = users[lsvUsers.FocusedItem.Index];
                btnDelete.Enabled = true;
                if (selectedUser.AccessType.ToLower() == "administrator")
                {
                    cboAccess.SelectedIndex = 0;
                    userAccess = "administrator";
                }
                else
                {
                    cboAccess.SelectedIndex = 1;
                    userAccess = "user";
                }
                btnChangeType.Enabled = false;
            }
        }

        private void cboAccess_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboAccess.Text.ToLower() != userAccess && lsvUsers.FocusedItem != null)
            {
                btnChangeType.Enabled = true;

            }
            else
            {
                btnChangeType.Enabled = false;

            }
        }
    }
}
