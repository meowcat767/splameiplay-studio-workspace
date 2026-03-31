/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
#if !LINUX
using System.Windows.Forms;
#endif

namespace SplameiPlay.Studio
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if !LINUX || WINE
        [STAThread]
#endif
        static void Main(string[] args)
        {
#if !LINUX || WINE
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#else
            if (args.Length > 0)
            {
                if (args[0] == "--hash" && args.Length > 1)
                {
                    string path = args[1];
                    try
                    {
                        if (System.IO.Directory.Exists(path))
                        {
                            Console.WriteLine(GlobalData.getDirectoryMd5Hash(path));
                        }
                        else if (System.IO.File.Exists(path))
                        {
                            Console.WriteLine(GlobalData.getFileHash(path));
                        }
                        else
                        {
                            Console.WriteLine("File or directory not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error calculating hash: " + ex.Message);
                    }
                    return;
                }
            }

            Console.WriteLine("SplameiPlay Studio is currently only supported on Windows.");
            Console.WriteLine("This Linux build is intended for development and CLI tools only.");
            Console.WriteLine();
            Console.WriteLine("Available CLI tools:");
            Console.WriteLine("  --hash <path>    Get the hash of a file or directory.");
#endif
        }
    }
}
