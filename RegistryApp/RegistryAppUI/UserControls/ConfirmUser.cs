using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RegistryAppUI.UserControls
{
    public partial class ConfirmUser : UserControl
    {
        public ConfirmUser()
        {
            InitializeComponent();
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
          ControlValidators.ValidateLength(txtUserName, e,100);
        }
    }
}
