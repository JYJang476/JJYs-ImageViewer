using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageViewer
{
    public partial class frmFileList : Form
    {
        private SpecialFolderEnum specialFolderEnum = new SpecialFolderEnum();

        private String targetPath = null;

        private List<FileInfo> fileInfos = null;

        private FileLogic fileLogic = new FileLogic();

        private frmMain frmMain = null;

        private int BOTH_PADDING = 10;

        private int ITEM_SIZE = 80;

        public frmFileList()
        {
            InitializeComponent();
        }

        private void frmFileList_Load(object sender, EventArgs e)
        {
            if (this.targetPath == null) {
                MessageBox.Show("대상 폴더가 지정되지 않았습니다.", "Error", MessageBoxButtons.OK);
                this.Close();
            }
        }

        private void initFileList(int page)
        {
            int rowsMaxCount = (this.imgList.Width - BOTH_PADDING) / ITEM_SIZE;
            int colsMaxCount = (this.imgList.Height - BOTH_PADDING) / (ITEM_SIZE + BOTH_PADDING) + 1;
            int itemPadding = ((this.imgList.Width - BOTH_PADDING) - (ITEM_SIZE * rowsMaxCount + BOTH_PADDING)) / rowsMaxCount;
            int itemCount = 0;

            int fileListStart = colsMaxCount * page;

            foreach (FileInfo fileInfo in this.fileInfos)
            {
                if (ImageTypeEnum.getImageTypes().Contains(fileInfo.Extension.Substring(1, fileInfo.Extension.Length - 1))) {
                    int itemCountInRow = itemCount % rowsMaxCount;
                    int itemX = itemCountInRow * (ITEM_SIZE + itemPadding);
                    int itemY = itemCount++ / rowsMaxCount * (ITEM_SIZE + itemPadding);

                    Panel imgItem = CreateItemPanel(Image.FromFile(fileInfo.FullName), new Point(itemX, itemY), fileInfo.Name, fileInfo.FullName);
                    this.imgList.Controls.Add(imgItem);
                }

                // item 더블클릭 시 frmMain에서 파일을 확인할 수 있도록 하는 코드
            }

            UpdateScrollBar(this.fileInfos.Count);
        }

        private void UpdateScrollBar(int itemTotalCount)
        {
            int contentHeight = itemTotalCount * (ITEM_SIZE + BOTH_PADDING);
        }

        private Panel CreateItemPanel(Image image, Point itemLocation, String labelText, String filePath)
        {
            MouseEventHandler handler = new MouseEventHandler((object mouseSender, MouseEventArgs mouseEvent) =>
            {
                if (this.frmMain != null && mouseEvent.Button == MouseButtons.Left)
                    this.frmMain.LoadPicture(filePath);
            });

            // 아이템 패널 생성
            Panel itemPanel = new Panel();
            itemPanel.Size = new Size(80, 80);
            itemPanel.Location = itemLocation;
            itemPanel.BorderStyle = BorderStyle.FixedSingle;
            itemPanel.MouseDoubleClick += handler;

            // PictureBox 생성
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(80, 60);  // 위쪽 60px
            pictureBox.Location = new Point(0, 0);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = image;
            pictureBox.MouseDoubleClick += handler;

            // Label 생성
            Label label = new Label();
            label.Text = labelText;
            label.Size = new Size(80, 20);  // 아래쪽 20px
            label.Location = new Point(0, 60);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.MouseDoubleClick += handler;

            // 아이템 패널에 추가
            itemPanel.Controls.Add(pictureBox);
            itemPanel.Controls.Add(label);

            return itemPanel;
        }

        public void setTargetPath(String path)
        {
            this.targetPath = path;
        }

        public void setMainForm(frmMain frmMain)
        {
            this.frmMain = frmMain;
        }

        private void tvFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode currentNode = e.Node.FirstNode;

            if (e.Node.Nodes.Count == 1 && currentNode.Text.Equals(""))
                LoadSubDirectories(e.Node, (string) e.Node.Tag);
        }

        private void LoadSubDirectories(TreeNode node, string path)

        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

            try
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    TreeNode subNode = new TreeNode(di.Name);
                    subNode.Tag = di.FullName;

                    node.Nodes.Add(subNode);
                }
            }
            catch { /* 권한 오류 등 무시 */ }
        }

        private void ExpandToPath(TreeNode rootNode, string rootPath, string filePath)
        {
            FilePathDto filePathDto = new FilePathDto(rootPath, filePath);
            LinkedListNode<string> thisNode = filePathDto.getFolderName().First;
            TreeNode currentTreeNode = rootNode;

            Stack<TreeNode> pathStack = new Stack<TreeNode>();
            pathStack.Push(currentTreeNode);

            while (pathStack.Peek() != null)
            {
                currentTreeNode = pathStack.Pop();
                String comparePath = currentTreeNode.Parent == null ? (String) currentTreeNode.Tag : currentTreeNode.Text;

                if (thisNode != null && comparePath.Equals(thisNode.Value))
                {
                    LoadSubDirectories(currentTreeNode, (string)currentTreeNode.Tag);
                    pathStack.Push(currentTreeNode.FirstNode);
                    thisNode = thisNode.Next;
                    currentTreeNode.Expand();
                    continue;
                }

                pathStack.Push(currentTreeNode.NextNode);
            }

            if (currentTreeNode.Tag.ToString().Equals(this.targetPath))
                this.tvFolders.SelectedNode = currentTreeNode;
        }

        private void frmFileList_Shown(object sender, EventArgs e)
        {
            this.fileInfos = fileLogic.GetFileInfos(this.targetPath);
            initFileList(1);

            // 파일 뷰어 불러오기
            specialFolderEnum.initList();

            // TreeView 이벤트 연결
            this.tvFolders.BeforeExpand += tvFolders_BeforeExpand;

            foreach (var folder in specialFolderEnum.getExistSpecialFolders())
            {
                TreeNode thisNode = this.tvFolders.Nodes.Add(folder.Key);
                thisNode.Tag = folder.Value;

                if (specialFolderEnum.compareFolderPath(folder.Value, this.targetPath)
                    && tvFolders.SelectedNode == null)
                {
                    // 특정 경로까지 확장
                    ExpandToPath(thisNode, folder.Value, targetPath);
                }
            }
        }
    }
}
