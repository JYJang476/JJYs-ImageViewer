using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageViewer.Logic
{
    class SpecialFolderLogic
    {
        private Dictionary<string, string> existSpecialFolders = new Dictionary<string, string>();

        // Known folder ID for Downloads folder
        private static readonly Guid DownloadsFolderGuid = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll")]
        private static extern int SHGetKnownFolderPath(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
                uint dwFlags,
                IntPtr hToken,
                out IntPtr ppszPath);

        private static string GetDownloadsPath()
        {
            IntPtr outPath;
            int result = SHGetKnownFolderPath(DownloadsFolderGuid, 0, IntPtr.Zero, out outPath);

            if (result != 0)
                throw new ExternalException("Unable to retrieve the Downloads folder path", result);

            string path = Marshal.PtrToStringUni(outPath);
            Marshal.FreeCoTaskMem(outPath);
            return path;
        }

        public Boolean compareFolderPath(string specialFolderPath, string path)
        {
            return specialFolderPath.Length < path.Length && path.Substring(0, specialFolderPath.Count()).Equals(specialFolderPath);
        }

        public Dictionary<string, string> getExistSpecialFolders()
        {
            return this.existSpecialFolders;
        }
    }
}
