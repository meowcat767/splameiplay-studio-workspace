/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */
namespace SplameiPlay.Studio;

public class InstallerInfo
{
    public string FileVer { get; set; } = "1.0";
    public GlobalData.fileTypePreset Type { get; set; } = GlobalData.fileTypePreset.Installer;
    
    public string Name { get; set; }
    public string Author { get; set; }
    
    public bool SupportsWindows { get; set; }
    
    public string ExeName { get; set; }
    
    public string Url { get; set; }
    public string NoticesUrl { get; set; }
    public string ProjectUrl { get; set; }
    public string TermsUrl { get; set; }
}