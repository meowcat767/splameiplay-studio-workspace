/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SplameiPlay.Studio
{
    public class GlobalData
    {
        public enum fileTypePreset
        {
            Splameiplay = 0,
            Installer = 1,
            Theme = 2,
            Policy = 3,
            PlayData = 4,
            PublicData = 5,
            ReleaseData = 6
        }

        public static string getDirectoryMd5Hash(string path)
        {
            using (var sha256Obj = SHA256.Create())
            {
                var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).OrderBy(f => f, StringComparer.OrdinalIgnoreCase);

                foreach (var filename in files)
                {
                    var relPath = getRelativePath(path, filename);
                    var pathBytes = Encoding.UTF8.GetBytes(relPath.Replace("\\", "/"));
                    sha256Obj.TransformBlock(pathBytes, 0, pathBytes.Length, null, 0);

                    using (var stream = File.OpenRead(filename))
                    {
                        var buffer = new byte[8192];
                        int bytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            sha256Obj.TransformBlock(buffer, 0, bytesRead, null, 0);
                        }
                    }
                }

                sha256Obj.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                return bytesToHexStr(sha256Obj.Hash);
            }
        }

        public static string getFileHash(string filePath)
        {
            using (var sha256Obj = SHA256.Create())
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var hash = sha256Obj.ComputeHash(stream);
                    return bytesToHexStr(hash);
                }
            }
        }

        public static string bytesToHexStr(byte[] bytes)
        {
            var stringBuilder = new StringBuilder(bytes.Length * 2);
            foreach (var byteObj in bytes)
            {
                stringBuilder.Append(byteObj.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string getRelativePath(string basePath, string fullPath)
        {
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                basePath += Path.DirectorySeparatorChar;
            }

            Uri fromUri = new Uri(fullPath);
            Uri toUri = new Uri(fullPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
