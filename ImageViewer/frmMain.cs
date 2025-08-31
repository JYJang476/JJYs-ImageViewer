using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                this.targetFolderPath = new FileInfo(openFileDialog.FileName).DirectoryName;
                LoadPicture(openFileDialog.FileName);
            }
        }

        private void imgViewer_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // 경로를 파라메터로 가져온 후 이미지 데이터로 이미지 표시
            /*
            string[] args = Environment.GetCommandLineArgs();

            if (args.Count() == 0)
                return;

            String imageFIlePath = args[0];
            LoadPicture(imageFIlePath);*/
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
            this.imgViewer.Image = Image.FromFile(path);
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
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
