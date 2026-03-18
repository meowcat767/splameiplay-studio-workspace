using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            using (Process.Start("https://docs.veemo.uk")) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Load(object sender, EventArgs e)
        {
            label2.Text = $"Version {Application.ProductVersion} - Pre-release 1";
        }
    }
}
