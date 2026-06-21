using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ImageViewer.Logic;
using System.Diagnostics;
using ImageViewer.Enum;

namespace ImageViewer
{
    public partial class frmMain : Form
    {
        private FileLogic fileLogic = new FileLogic();
        private ImageEditLogic editLogic = new ImageEditLogic();
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
            editLogic.setImagePanelSize(imgViewer.Size);

            // 더블버퍼링 추가
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null,
                imgViewer,
                new object[] { true });

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

            imgViewer.MouseWheel += new MouseEventHandler(imgViewer_MouseWheel);
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
            FileInfo fileInfo = new FileInfo(path);
            this.targetFolderPath = fileInfo.DirectoryName;
            this.Text = fileInfo.Name;

            string imageType = ImageTypeEnum.ofType(new FileInfo(path).Extension.Replace(".", ""));

            if (imageType.Equals("gif"))
            {
                editLogic.setOriginImage(fileLogic.LoadAnimatingGif(path));
            }
            else if (imageType.Equals("tga"))
            {
                editLogic.setOriginImage(fileLogic.LoadTargaImage(path, false));
            } else if (!imageType.Equals(""))
            {
                editLogic.setOriginImage(fileLogic.LoadDefaultImage(path, false));
            }

            editLogic.drawImage(imgViewer.CreateGraphics());
        }

        private void 같은폴더내의이미지탐색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFileList frmFileList = new frmFileList();
            frmFileList.setTargetPath(this.targetFolderPath);
            frmFileList.setMainForm(this);
            frmFileList.Show();
        }

        private void imgViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            // 현재 상태가 줌상태라면
            if (editLogic.getViewerStatus() == ViewerStatusEnum.ZOOM) this.Cursor = Cursors.Hand; // 커서를 손모양으로 변경
            else
                this.Cursor = Cursors.Default;

            bool isZoom = e.Delta > 0;
            editLogic.zoomImage(isZoom, e.Location); // 확대

            imgViewer.Invalidate();
        }

        private void imgViewer_Paint(object sender, PaintEventArgs e)
        {
            editLogic.drawImage(e.Graphics);
        }

        private void imgViewer_Resize(object sender, EventArgs e)
        {
            editLogic.setImagePanelSize(this.imgViewer.Size);
        }

        private void imgViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (editLogic.getViewerStatus() != ViewerStatusEnum.MOVING)
            {
                editLogic.initImageMove(e.Location);
                editLogic.setViewerStatus(ViewerStatusEnum.MOVING);
            }
        }

        private void imgViewer_MouseUp(object sender, MouseEventArgs e)
        {
            if (editLogic.getViewerStatus() == ViewerStatusEnum.MOVING) editLogic.roolBackStatus();
        }

        private void imgViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && editLogic.getViewerStatus() == ViewerStatusEnum.MOVING)
            {
                editLogic.moveImage(e.Location);
                imgViewer.Invalidate();
            }
        }
    }
}
