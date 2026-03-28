/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Windows.Forms;

namespace SplameiPlay.Studio
{
    public partial class PleaseWait : Form
    {
        public Action cancelAction;

        public PleaseWait()
        {
            InitializeComponent();

            setMessage("");
        }

        public void setMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                label1.Text = "Please wait while SplameiPlay Studio's code executes";
            }
            else
            {
                label1.Text = message;
            }
        }

        public void setCanCancel(Action actionOnCancel)
        {
            if (actionOnCancel == null)
            {
                cancelButton.Enabled = false;
                cancelAction = null;
            }
            else
            {
                cancelButton.Enabled = true;
                cancelAction = actionOnCancel;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (cancelAction != null)
            {
                cancelAction();
            }
        }
    }
}
