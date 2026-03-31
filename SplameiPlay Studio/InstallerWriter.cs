/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */
namespace SplameiPlay.Studio;

public class InstallerWriter
{
    public static string GenerateManifest(InstallerInfo info)
    {
        return 
$@"FileVer={info.FileVer}
Type={(int)info.Type}

Name={info.Name}
Author={info.Author}

SupportsWindows={info.SupportsWindows}

ExeName={info.ExeName}
Url={info.Url}
NoticesUrl={info.NoticesUrl}
ProjectUrl={info.ProjectUrl}
TermsUrl={info.TermsUrl}            
";
    }
}