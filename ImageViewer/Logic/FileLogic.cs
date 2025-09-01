using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageViewer
{
    class FileLogic
    {
        public List<FileInfo> GetFileInfos(String directoryPath)
        {
            return new DirectoryInfo(directoryPath).GetFiles().ToList();
        }

        public byte[] getFileData(String path)
        {
            return File.ReadAllBytes(path);
        }

        public ImageType getFileType(String path)
        {
            FileInfo fileInfo = new FileInfo(path);
            ImageType imageType = new ImageType(fileInfo.Extension.Replace(".", ""), fileInfo.Name);

            return imageType;
        }
    }
}
