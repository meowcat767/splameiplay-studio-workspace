/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Diagnostics;
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

        private void Form1_Load(object sender, EventArgs e) { }

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

                    string[] tmp = new string[] { "Version=1.1", "Type=Installer", "TypeVersion=1.0", "" };
                    switch (fileTypePreset)
                    {
                        case GlobalData.fileTypePreset.Installer:
                            tmp = new string[] { "Version=1.1", "Type=Installer", "TypeVersion=1.1", "", "Name=Project Name", "Author=Your name", "ExeName=Example.exe", "Url=https://example.com", "NoticesUrl=https://example.com", "ProjectUrl=https://example.com" };
                            break;
                        case GlobalData.fileTypePreset.Theme:
                            tmp = new string[] { "Version=1.1", "Type=Theme", "TypeVersion=1.1", "", "Name=SplameiPlay Docs Demo", "BckColorOne=663b18", "BckColorTwo=8f2c21" };
                            break;
                        case GlobalData.fileTypePreset.Policy:
                            tmp = new string[] { "Version=1.1", "Type=Policy", "TypeVersion=1.1", "", "RestrictStore=true", "RestrictCustomProjects=true", "DisableRecap=false", "DisableDiscordRP=true", "ForceOnlineMode=true", "InstallLocation=C:/Splamei", "/InstallProjects", "/ApplyPolicyFor", "/IgnorePolicyFor" };
                            break;
                        case GlobalData.fileTypePreset.PlayData:
                            tmp = new string[] { "Version=1.1", "Type=ProjectPlayData", "TypeVersion=1.0", "", "Name=Project Name", "Author=Project author", "CurrentUrl=https://example.com", "EarlyAccess=false", "/MinSplameiplay", "MinSplameiplayVer=1000", "RequiredSplameiplayEdition=All", "/Windows", "MinMajorVer=10", "MinMinorVer=0", "MinBuildVer=19042", "MinRamMb=1500", "MinCpuSpeedMHz=2000" };
                            break;
                        case GlobalData.fileTypePreset.PublicData:
                            tmp = new string[] { "Version=1.1", "Type=ProjectPublicData", "TypeVersion=1.0", "", "DescriptionShort=Short Description", "DescriptionLong=Long Description", "Locale=en,ja", "Author=Your name(s)", "TermsShort=Short terms", "TermsLong=Long terms", "/Windows", "Specs=Windows Specs", "/AgeRating", "Age=7", "/Urls", "NoticeUrl=https://example.com", "MoreInfoUrl=https://example.com" };
                            break;
                        case GlobalData.fileTypePreset.ReleaseData:
                            tmp = new string[] { "Version=1.1", "Type=ProjectReleaseData", "TypeVersion=1.0", "", "Hash=Project zip hash", "ExeName=Example.exe", "Downloadable=true", "DynamicDownloadable=false" };
                            break;
                    }

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
                this.Show();
                if (!editor.IsDisposed)
                {
                    editor.Close();
                }

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

        private void aboutButton_Click(object sender, EventArgs e)
        {
            using (About about = new About())
            {
                about.ShowDialog();
            }
        }

        private void splameiDocsButton_Click(object sender, EventArgs e)
        {
            using (Process.Start("https://docs.veemo.uk/")) { }
        }
    }
}
