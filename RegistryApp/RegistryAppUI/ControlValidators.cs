
using Bunifu.Framework.UI;
using System.Windows.Forms;

namespace RegistryAppUI
{
    public static class ControlValidators
    {
        public static void ValidateLength(BunifuMetroTextbox textbox, KeyPressEventArgs e,int length)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (textbox.Text.Length == length)
                e.Handled = true;
        }

     
    }
}
