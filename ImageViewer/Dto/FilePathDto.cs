using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageViewer
{
    class FilePathDto
    {
        private String filePath;
        private String rootPath;
        private LinkedList<String> folderName;

        public FilePathDto(String rootPath, String path)
        {
            this.folderName = new LinkedList<string>();
            LinkedListNode<String> currentNode = this.folderName.First;
            this.rootPath = rootPath;
            this.filePath = path;
            if (rootPath != null && !rootPath.Equals(""))
            {
                this.filePath = path.Replace(rootPath, "");
                currentNode = this.folderName.AddFirst(rootPath);
            }

            List<String> urlList = this.filePath.Split(new char[] { '\\', '/' }).ToList();

            foreach (String folder in urlList)
            {
                if (currentNode != null && folder != null && !folder.Equals(""))
                    currentNode = this.folderName.AddAfter(currentNode, folder);
                else if (currentNode == null)
                    currentNode = this.folderName.AddFirst(folder);
            }
        }

        public LinkedList<String> getFolderName()
        {
            return this.folderName;
        }

        public String getFilePath()
        {
            return this.filePath;
        }

        public String getRootPath()
        {
            return this.rootPath;
        }

        public String getFullPath()
        {
            if (rootPath == null || rootPath.Equals(""))
                return filePath;

            return rootPath + "\\" + filePath;
        }

        public bool isLastPath(LinkedListNode<string> targetNode)
        {
            return targetNode == folderName.Last;
        }

        public String getSelectionPath(String path)
        {
            if (path.Equals(this.rootPath))
                return path;

            LinkedListNode<string> thisNode = folderName.First;
            String output = "";

            while (!thisNode.Equals(folderName.Last))
            {
                output += thisNode.Value + "\\";
                if (thisNode.Value == path)
                    break;
                thisNode = thisNode.Next;
            }

            return rootPath + output;
        }
    }
}
