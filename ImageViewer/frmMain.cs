using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageViewer
{
    public partial class frmMain : Form
    {
        private FileLogic fileLogic = new FileLogic();
        private String targetFolderPath = null;
        public frmMain()
        {
            InitializeComponent();
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files | " + getFilterString(ImageTypeEnum.getImageTypes());
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != null && openFileDialog.FileName.Length > 0)
            {
                LoadPicture(openFileDialog.FileName);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // 경로를 파라메터로 가져온 후 이미지 데이터로 이미지 표시
            
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 0)
                return;

            foreach (string arg in args)
            {
                if (isValidImageFile(arg))
                {
                    LoadPicture(arg);
                    break;
                }
            }
        }

        private Boolean isValidImageFile(String fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            String fileType = fileInfo.Extension.Replace(".", "");

            return !ImageTypeEnum.ofType(fileType).Equals("");
        }

        private String getFilterString(List<String> fileTypes)
        {
            String output = "";

            foreach (String imageType in fileTypes)
            {
                output += "*." + imageType + ";";
            }

            return output.Substring(0, output.Length - 1);
        }

        public void LoadPicture(String path)
        {
            this.targetFolderPath = new FileInfo(path).DirectoryName;
            string imageType = ImageTypeEnum.ofType(new FileInfo(path).Extension.Replace(".", ""));

            if (imageType.Equals("gif"))
            {
                this.imgViewer.Image = fileLogic.LoadAnimatingGif(path);
            }
            else if (imageType.Equals("tga"))
            {
                this.imgViewer.Image = fileLogic.LoadTargaImage(path, false);
            } else if (!imageType.Equals(""))
            {
                this.imgViewer.Image = fileLogic.LoadDefaultImage(path, false);
            }
        }

        private void 같은폴더내의이미지탐색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFileList frmFileList = new frmFileList();
            frmFileList.setTargetPath(this.targetFolderPath);
            frmFileList.setMainForm(this);
            frmFileList.Show();
        }
    }
}
