/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Windows.Forms;
using SplameiPlay.SDK.Files;

namespace SplameiPlay.Studio
{
    public partial class Form1 : Form
    {
        public Editor editor = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolsButton_Click(object sender, EventArgs e)
        {
            using (ToolSelect toolSelect = new ToolSelect())
            {
                toolSelect.ShowDialog();
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            using (FileCreate fileCreate = new FileCreate())
            {
                if (fileCreate.ShowDialog() == DialogResult.OK)
                {
                    GlobalData.fileTypePreset fileTypePreset = fileCreate.typePreset;
                    string path = fileCreate.path;

                    string[] tmp = new string[] { "Version=1.1", "Type=Installer", "TypeVersion=1.0", "", "Test", "Test2=hello", "Test3=10", "Test4=false" };
                    try
                    {
                        var data = SplameiPlayFiles.ReadSyntax(tmp);

                        editor = new Editor(data, path);
                        editor.Show();

                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[StartForm] Unable to create file! - " + ex);
                        MessageBox.Show($"Something went wrong when setting up your new file. Please contact us for support\n\nException:\n{ex}", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Hide();

                var data = SplameiPlayFiles.ReadFile(openFileDialog1.FileName);

                editor = new Editor(data, openFileDialog1.FileName);
                editor.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[StartForm] Unable to open file! - " + ex);
                MessageBox.Show($"We can't open that file. Please make sure the syntax is correct and the file exists\n\nException:\n{ex}", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (editor != null)
            {
                if (!editor.IsDisposed)
                {
                    editor.Dispose();
                }
            }
        }
    }
}
