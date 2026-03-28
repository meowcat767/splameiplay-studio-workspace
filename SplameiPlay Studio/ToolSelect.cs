/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class ToolSelect : Form
    {
        string resultHash = string.Empty;
        bool gotHash = false;

        PleaseWait wait = new PleaseWait();

        Task hashTask = null;
        CancellationTokenSource cts = null;

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

                cts = new CancellationTokenSource();

                resultHash = string.Empty;
                gotHash = false;

                if (string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath)) { return; }

                timer1.Start();
                wait.setMessage("Please wait while we get that directories hash");

                hashTask = Task.Run(() =>
                {
                    try
                    {
                        resultHash = GlobalData.getDirectoryMd5Hash(folderBrowserDialog1.SelectedPath, cts.Token);
                        gotHash = true;
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine("[ToolSelect] Stopped getting the hash (task run exception)");
                    }
                }, cts.Token);

                wait.setCanCancel(cancelHash);

                wait.Shown += async (s, args) =>
                {
                    try
                    {
                        await hashTask;
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine("[ToolSelect] Stopped getting the hash (wait show catch)");
                    }
                    finally
                    {
                        wait.Close();
                    }
                };

                wait.ShowDialog(this);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (wait.IsDisposed) { wait = new PleaseWait(); }

            cts = new CancellationTokenSource();

            resultHash = string.Empty;
            gotHash = false;

            if (string.IsNullOrEmpty(openFileDialog1.FileName)) { return; }

            timer1.Start();
            wait.setMessage("Please wait while we get that file's hash");

            hashTask = Task.Run(() =>
            {
                resultHash = GlobalData.getFileHash(openFileDialog1.FileName);
                gotHash = true;
            });

            wait.Shown += async (s, args) =>
            {
                await hashTask;
                wait.Close();
            };

            wait.ShowDialog(this);
        }

        private void ToolSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            wait.Dispose();

            if (hashTask != null && !hashTask.IsCompleted)
            {
                cts?.Cancel();
            }

            if (hashTask != null)
            {
                hashTask.Dispose();
            }

            if (cts != null)
            {
                cts.Dispose();
            }
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

        private async void cancelHash()
        {
            cts?.Cancel();

            try
            {
                wait.setMessage("");
                wait.setCanCancel(null);
                await hashTask;
            }
            catch (OperationCanceledException) { }
            finally
            {
                wait.Close();
            }
        }
    }
}
