/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class ToolSelect : Form
    {
        string resultHash = string.Empty;
        bool gotHash = false;

        PleaseWait wait = new PleaseWait();

        public ToolSelect()
        {
            InitializeComponent();
        }

        private void getFileHash_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void getDirectoryHash_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (wait.IsDisposed) { wait = new PleaseWait(); }

                resultHash = string.Empty;
                gotHash = false;

                if (string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath)) { return; }

                timer1.Start();
                wait.setMessage("Please wait while we get that directories hash");

                var task = Task.Run(() =>
                {
                    resultHash = GlobalData.getDirectoryMd5Hash(folderBrowserDialog1.SelectedPath);
                    gotHash = true;
                });

                wait.Shown += async (s, args) =>
                {
                    await task;
                    wait.Close();
                };

                wait.ShowDialog(this);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (wait.IsDisposed) { wait = new PleaseWait(); }

            resultHash = string.Empty;
            gotHash = false;

            if (string.IsNullOrEmpty(openFileDialog1.FileName)) { return; }

            timer1.Start();
            wait.setMessage("Please wait while we get that file's hash");

            var task = Task.Run(() =>
            {
                resultHash = GlobalData.getFileHash(openFileDialog1.FileName);
                gotHash = true;
            });

            wait.Shown += async (s, args) =>
            {
                await task;
                wait.Close();
            };

            wait.ShowDialog(this);
        }

        private void ToolSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            wait.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gotHash)
            {
                textBox1.Text = resultHash;
                gotHash = false;
                timer1.Stop();

                wait.Close();
            }
        }

        private void ToolSelect_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            using (Process.Start("https://docs.veemo.uk")) { }
        }
    }
}
