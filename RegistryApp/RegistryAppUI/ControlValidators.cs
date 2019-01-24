
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

        public static void ValidateMaterial(this BunifuMaterialTextbox textBox, int maxNumber, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (textBox.Text.Trim().Length == maxNumber)
            {
                e.Handled = true;
            }
        }


    }
}
