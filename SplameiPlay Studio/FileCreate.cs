/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class FileCreate : Form
    {
        List<string> saveFileFilter = new List<string>()
        {
            "SplameiPlay file|*.splameiplay",
            "Installer file|*.spinstaller",
            "Theme file|*.sptheme",
            "SplameiPlay file|*.splameiplay",
            "SplameiPlay file|*.splameiplay",
            "SplameiPlay file|*.splameiplay",
            "SplameiPlay file|*.splameiplay"
        };

        List<GlobalData.fileTypePreset> fileTypePresetLookup = new List<GlobalData.fileTypePreset>()
        {
            GlobalData.fileTypePreset.Splameiplay,
            GlobalData.fileTypePreset.Installer,
            GlobalData.fileTypePreset.Theme,
            GlobalData.fileTypePreset.Policy,
            GlobalData.fileTypePreset.PlayData,
            GlobalData.fileTypePreset.PublicData,
            GlobalData.fileTypePreset.ReleaseData,
        };

        public GlobalData.fileTypePreset typePreset = GlobalData.fileTypePreset.Splameiplay;
        public string path = string.Empty;

        public FileCreate()
        {
            InitializeComponent();
        }

        private void FileCreate_Load(object sender, EventArgs e)
        {

        }

        private void filePresetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filePresetList.SelectedIndex != -1)
            {
                saveFileDialog1.Filter = saveFileFilter[filePresetList.SelectedIndex];
                browseButton.Enabled = true;
            }
            else
            {
                browseButton.Enabled = false;
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            fileLocationTextBox.Text = saveFileDialog1.FileName;
        }

        private void fileLocationTextBox_TextChanged(object sender, EventArgs e)
        {
            createButton.Enabled = !string.IsNullOrEmpty(fileLocationTextBox.Text);
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            path = fileLocationTextBox.Text;
            typePreset = fileTypePresetLookup[filePresetList.SelectedIndex];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FileCreate_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            using (Process.Start("https://docs.veemo.uk")) { }
        }
    }
}
