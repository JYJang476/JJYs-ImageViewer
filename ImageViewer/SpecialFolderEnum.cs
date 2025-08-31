using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageViewer
{
    class SpecialFolderEnum
    {
        private static Dictionary<string, Environment.SpecialFolder> specialFolders = new Dictionary<string, Environment.SpecialFolder>
        {
            { "바탕화면", Environment.SpecialFolder.Desktop },
            { "다운로드", Environment.SpecialFolder.Personal },
            { "사진", Environment.SpecialFolder.MyPictures },
            { "음악", Environment.SpecialFolder.MyMusic },
            { "비디오", Environment.SpecialFolder.MyVideos },
            { "문서", Environment.SpecialFolder.MyDocuments }
        };

        // Known folder ID for Downloads folder
        private static readonly Guid DownloadsFolderGuid = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr ppszPath);

        private Dictionary<string, string> existSpecialFolders = new Dictionary<string, string>();

        public void initList()
        {
            foreach (var folder in specialFolders)
            {
                string path = Environment.GetFolderPath(folder.Value);
                if (folder.Key == "다운로드")
                    path = GetDownloadsPath();
                if (Directory.Exists(path))
                    existSpecialFolders.Add(folder.Key, path);
            }
        }

        private string GetDownloadsPath()
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
            return path.Substring(0, specialFolderPath.Count()).Equals(specialFolderPath);
        }

        public Dictionary<string, string> getExistSpecialFolders()
        {
            return this.existSpecialFolders;
        }
    }
}
