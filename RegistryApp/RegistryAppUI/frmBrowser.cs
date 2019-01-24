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
    public partial class frmBrowser : Form
    {
     
        public frmBrowser()
        {
            InitializeComponent();
        }

        public frmBrowser(string url)
        {
            InitializeComponent();
            OpenLink(url);
            txtLink.Text = url;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string url = txtLink.Text.Trim();
            OpenLink(url);
        }

        private void OpenLink(string url)
        {
            if (!url.StartsWith("http://") && (!url.StartsWith("https://")))
            {
                url = "http://" + url;
            }

            try
            {
                webBrowser1.Navigate(new Uri(url));
            }
            catch (Exception)
            {

                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack)
            {
                webBrowser1.GoBack();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward)
            {
                webBrowser1.GoForward();
            }
        }

        private void txtLink_Enter(object sender, EventArgs e)
        {

        }

        private void txtLink_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string url = txtLink.Text.Trim();
                OpenLink(url);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
