/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using SplameiPlay.SDK.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class Editor : Form
    {
        Dictionary<string, Dictionary<string, SplameiPlayFiles.fileValueData>> data = null;
        string path = string.Empty;
        string type = string.Empty;
        float typeVersion = 1.0f;

        bool isSaved = false;

        string currentSection;
        string currentKey;
        TreeNode currentNode;

        List<SplameiPlayFiles.fileValueDataType> keyDataTypes = new List<SplameiPlayFiles.fileValueDataType>()
        {
            SplameiPlayFiles.fileValueDataType.String,
            SplameiPlayFiles.fileValueDataType.Int,
            SplameiPlayFiles.fileValueDataType.Float,
            SplameiPlayFiles.fileValueDataType.Bool,
            SplameiPlayFiles.fileValueDataType.Unknown
        };

        public Editor(SplameiPlayFiles.readFileResult fileData, string pathL)
        {
            InitializeComponent();

            data = fileData.data;
            path = pathL;

            type = fileData.type;
            typeTextBox.Text = type;
            typeVersion = fileData.typeVersion;
            typeVersionNum.Value = (decimal)typeVersion;
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            refreshView();

            newKeyDataTypeDropdown.SelectedIndex = 0;

            this.Text = $"SplameiPlay Studio [{path}*]";
        }

        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            Environment.Exit(0);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentNode = e.Node;

            resetSidebar();

            if (e.Node.Tag is SplameiPlayFiles.fileValueData valueData)
            {
                keyName.Enabled = true;
                keyName.Text = e.Node.Text;

                removeKeyButton.Enabled = true;
                removeSectionButton.Enabled = false;
                addKeyButton.Enabled = false;
                newKeyDataTypeDropdown.Enabled = false;

                currentSection = e.Node.Parent.Text;
                currentKey = e.Node.Text;

                if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Bool)
                {
                    keyBooleanValue.Checked = valueData.boolValue;
                    keyBooleanValue.Enabled = true;
                }
                else if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Float)
                {
                    keyFloatValue.Value = decimal.Parse(valueData.floatValue.ToString());
                    keyFloatValue.Enabled = true;
                }
                else if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Int)
                {
                    keyIntValue.Value = valueData.intValue;
                    keyIntValue.Enabled = true;
                }
                else if (valueData.dataType == SplameiPlayFiles.fileValueDataType.String)
                {
                    if (!string.IsNullOrEmpty(valueData.stringValue))
                    {
                        keyStringValue.Lines = valueData.stringValue.Split(new string[] { "\n" }, StringSplitOptions.None);
                    }
                    keyStringValue.Enabled = true;
                }
            }
            else
            {
                sectionName.Enabled = true;
                sectionName.Text = e.Node.Text;

                removeKeyButton.Enabled = false;
                removeSectionButton.Enabled = true;
                addKeyButton.Enabled = true;
                newKeyDataTypeDropdown.Enabled = true;

                currentSection = e.Node.Text;
                currentKey = null;
            }
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (currentSection == null) { return; }

            if (currentNode.Parent == null)
            {
                string newSection = sectionName.Text;

                if (currentSection == "Global")
                {
                    MessageBox.Show("You cannot rename the Global section as it is required by the syntax", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!validName(newSection))
                {
                    MessageBox.Show("That section name is invalid and not usable. Please make sure it only contains alphanumerical characters and no spaces", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (newSection != currentSection)
                {
                    var sectionData = data[currentSection];
                    data.Remove(currentSection);
                    data[newSection] = sectionData;

                    currentNode.Text = newSection;
                    currentSection = newSection;
                }
            }
            else
            {
                string newKey = keyName.Text;

                if (!validName(newKey))
                {
                    MessageBox.Show("That key name is invalid and not usable. Please make sure it only contains alphanumerical characters and no spaces", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var valueData = data[currentSection][currentKey];

                if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Int)
                {
                    valueData.intValue = (int)keyIntValue.Value;
                }
                else if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Bool)
                {
                    valueData.boolValue = keyBooleanValue.Checked;
                }
                else if (valueData.dataType == SplameiPlayFiles.fileValueDataType.Float)
                {
                    valueData.floatValue = (float)keyFloatValue.Value;
                }
                else
                {
                    valueData.stringValue = keyStringValue.Text;
                }

                if (newKey != currentKey)
                {
                    data[currentSection].Remove(currentKey);
                    data[currentSection][currentKey] = valueData;

                    currentNode.Tag = valueData;

                    currentNode.Text = newKey;
                    currentKey = newKey;
                }
                else
                {
                    data[currentSection][currentKey] = valueData;

                    currentNode.Tag = valueData;
                }
            }

            isSaved = false;
            updateTitle();
        }

        private void refreshView()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            foreach (var section in data)
            {
                TreeNode sectionNode = new TreeNode(section.Key);

                foreach (var key in section.Value.Keys)
                {
                    TreeNode keyNode = new TreeNode(key);

                    keyNode.Tag = section.Value[key];

                    sectionNode.Nodes.Add(keyNode);
                }

                treeView1.Nodes.Add(sectionNode);
            }

            treeView1.EndUpdate();
        }

        private void resetSidebar()
        {
            sectionName.Enabled = false;
            sectionName.Text = string.Empty;

            keyName.Enabled = false;
            keyName.Text = string.Empty;

            keyStringValue.Enabled = false;
            keyStringValue.Text = string.Empty;

            keyIntValue.Enabled = false;
            keyIntValue.Value = 0;

            keyBooleanValue.Enabled = false;
            keyBooleanValue.Checked = false;

            keyFloatValue.Enabled = false;
            keyFloatValue.Value = 0;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            refreshView();
        }

        private bool validName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z0-9_ -]+$") && !name.Contains(" ");
        }

        private void addSectionButton_Click(object sender, EventArgs e)
        {
            string newSection = sectionKeyName.Text;

            if (!validName(newSection))
            {
                MessageBox.Show("That section name is invalid and not usable. Please make sure it only contains alphanumerical characters and no spaces", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int i = 1;
            while (data.ContainsKey(newSection))
            {
                newSection = sectionKeyName.Text + i++;
            }

            data[newSection] = new Dictionary<string, SplameiPlayFiles.fileValueData>();

            TreeNode node = new TreeNode(newSection);
            treeView1.Nodes.Add(node);

            treeView1.SelectedNode = node;

            isSaved = false;
            updateTitle();
        }

        private void addKeyButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) { return; }

            TreeNode sectionNode;

            if (treeView1.SelectedNode.Parent == null)
            {
                sectionNode = treeView1.SelectedNode;
            }
            else
            {
                MessageBox.Show("To add a key, select the section that it should be added to", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string section = sectionNode.Text;

            string newKeyName = sectionKeyName.Text;
            int i = 1;

            while (data[section].ContainsKey(newKeyName))
            {
                newKeyName = sectionKeyName.Text + i++;
            }

            SplameiPlayFiles.fileValueData value = new SplameiPlayFiles.fileValueData();
            value.dataType = keyDataTypes[newKeyDataTypeDropdown.SelectedIndex];

            data[section][newKeyName] = value;

            TreeNode node = new TreeNode(newKeyName);
            node.Tag = value;
            sectionNode.Nodes.Add(node);

            treeView1.SelectedNode = node;

            isSaved = false;
            updateTitle();
        }

        private void removeKeyButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) { return; }

            if (MessageBox.Show("Are you sure you want to delete this key? This cannot be undone!", "SplameiPlay Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TreeNode node = treeView1.SelectedNode;

                if (node.Parent != null)
                {
                    string section = node.Parent.Text;
                    string key = node.Text;

                    data[section].Remove(key);
                    node.Parent.Nodes.Remove(node);
                }

                isSaved = false;
                updateTitle();
            }
        }

        private void removeSectionButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) { return; }

            if (treeView1.SelectedNode.Text == "Global")
            {
                MessageBox.Show("You can't delete the Global section as it's required by the syntax", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this section? This cannot be undone!", "SplameiPlay Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TreeNode node = treeView1.SelectedNode;

                if (node.Parent == null)
                {
                    string section = node.Text;

                    data.Remove(section);
                    node.Nodes.Remove(node);
                }

                isSaved = false;
                updateTitle();
            }
        }

        void saveFile(string path)
        {
            if (!validName(typeTextBox.Text))
            {
                MessageBox.Show("The type provided is not valid. Please make sure it contains only alphanumerical characters with no spaces.", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SplameiPlayFiles.SaveFile(path, typeTextBox.Text, (float)typeVersionNum.Value, data);

                isSaved = true;
                updateTitle();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Editor] Failed to save file! - " + ex.Message);
                MessageBox.Show("Something went wrong while saving the file. Please check the syntax and try again.", "SplameiPlay Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(path);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        void updateTitle()
        {
            if (isSaved)
            {
                this.Text = $"SplameiPlay Studio [{path}]";
            }
            else
            {
                this.Text = $"SplameiPlay Studio [{path}*]";
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel) { return; }

            path = saveFileDialog1.FileName;
            saveFile(path);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void typeTextBox_TextChanged(object sender, EventArgs e)
        {
            isSaved = false;
            updateTitle();
        }

        private void typeVersionNum_ValueChanged(object sender, EventArgs e)
        {
            isSaved = false;
            updateTitle();
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (About about = new About())
            {
                about.ShowDialog();
            }
        }

        private void splameiDocsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Process.Start("https://docs.veemo.uk")) { }
        }
    }
}

public class KeyValuePairData
{
    public string section { get; set; }
    public string key { get; set; }
    public SplameiPlayFiles.fileValueData valueData { get; set; }
}