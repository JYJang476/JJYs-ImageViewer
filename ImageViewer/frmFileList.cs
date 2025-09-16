using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        private bool isExtended = false;

        private const int BOTH_PADDING = 10;

        private const int ITEM_SIZE = 80;

        private const int BOTH_PATH_PADDING = 3;

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

        private void initFileList(int page, String path)
        {
            int rowsMaxCount = (this.imgList.Width - BOTH_PADDING) / ITEM_SIZE;
            int itemPadding = ((this.imgList.Width - BOTH_PADDING) - (ITEM_SIZE * rowsMaxCount + BOTH_PADDING)) / rowsMaxCount;
            int itemCount = 0;

            this.imgList.Controls.Clear();
            this.fileInfos = fileLogic.GetFileInfos(path);

            foreach (FileInfo fileInfo in this.fileInfos)
            {
                if (fileInfo.Extension.Length > 1 && ImageTypeEnum.getImageTypes().Contains(fileInfo.Extension.Substring(1, fileInfo.Extension.Length - 1))) {
                    int itemCountInRow = itemCount % rowsMaxCount;
                    int itemX = itemCountInRow * (ITEM_SIZE + itemPadding);
                    int itemY = itemCount++ / rowsMaxCount * (ITEM_SIZE + itemPadding);

                    Panel imgItem = CreateItemPanel(new Point(itemX, itemY), fileInfo.Name, fileInfo.FullName);
                    this.imgList.Controls.Add(imgItem);
                }
            }
        }

        private void initPathBox(FilePathDto filePathDto)
        {
            LinkedList<string> folderNames = filePathDto.getFolderName();
            LinkedListNode<string> thisNode = folderNames.First;

            this.pRoot.Controls.Clear();

            int startPosition = 12;

            for (int i = 0; i < folderNames.Count; i++)
            {
                if (thisNode.Value == null || thisNode.Value.Equals(""))
                {
                    thisNode = thisNode.Next;
                    continue;
                }

                String currentNodeValue = thisNode.Value;

                Button newButton = CreatePathButton(currentNodeValue, new Point(startPosition, 9));
                newButton.Click += new EventHandler((object buttonObject, EventArgs buttonArgs) =>
                {
                    String thisPath = filePathDto.getSelectionPath(currentNodeValue);
                    initPathBox(new FilePathDto(null, thisPath));
                    initFileList(1, thisPath);
                });
                pRoot.Controls.Add(newButton);

                startPosition += newButton.Width + BOTH_PATH_PADDING;

                Label newLabel = CreatePathArrowLabel(new Point(startPosition, 9));
                pRoot.Controls.Add(newLabel);

                startPosition += newLabel.Width + BOTH_PATH_PADDING;

                thisNode = thisNode.Next;
            }

            this.Text = filePathDto.getFullPath();
        } 

        private Button CreatePathButton(String path, Point location)
        {
            Button pathButton = new Button();
            pathButton.FlatStyle = FlatStyle.Flat;
            pathButton.FlatAppearance.BorderColor = Color.White;
            pathButton.BackColor = Color.White;
            pathButton.AutoSize = true;
            pathButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pathButton.Location = location;
            pathButton.Text = path;

            return pathButton;
        }

        private Label CreatePathArrowLabel(Point location)
        {
            Label pathArrowLabel = new Label();
            pathArrowLabel.Text = ">";
            pathArrowLabel.Size = new Size(10, pathArrowLabel.Height);
            pathArrowLabel.BackColor = Color.White;
            pathArrowLabel.TextAlign = ContentAlignment.MiddleCenter;
            pathArrowLabel.Location = location;

            return pathArrowLabel;
        }

        private Panel CreateItemPanel(Point itemLocation, String labelText, String filePath)
        {
            MouseEventHandler mouseClickHandler = new MouseEventHandler((object mouseSender, MouseEventArgs mouseEvent) =>
            {
                if (this.frmMain != null && mouseEvent.Button == MouseButtons.Left)
                    this.frmMain.LoadPicture(filePath);
            });

            // 아이템 패널 생성
            Panel itemPanel = new Panel();
            MouseEventHandler mouseDownHandler = new MouseEventHandler((object mouseSender, MouseEventArgs mouseEvent) =>
            {
                if (mouseEvent.Button == MouseButtons.Left)
                    itemPanel.BackColor = SystemColors.Control;
            });

            MouseEventHandler mouseUpHandler = new MouseEventHandler((object mouseSender, MouseEventArgs mouseEvent) =>
            {
                if (mouseEvent.Button == MouseButtons.Left)
                    itemPanel.BackColor = Color.Transparent;
            });

            itemPanel.Size = new Size(80, 80);
            itemPanel.Location = itemLocation;
            itemPanel.MouseDoubleClick += mouseClickHandler;
            itemPanel.MouseDown += mouseDownHandler;
            itemPanel.MouseUp += mouseUpHandler;

            // PictureBox 생성
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(80, 60);  // 위쪽 60px
            pictureBox.Location = new Point(0, 0);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            LoadImageFile(pictureBox, filePath);
            pictureBox.MouseDoubleClick += mouseClickHandler;
            pictureBox.MouseDown += mouseDownHandler;
            pictureBox.MouseUp += mouseUpHandler;

            // Label 생성
            Label label = new Label();
            ToolTip toolTip = new ToolTip();
            label.Text = LimitFileNameLength(labelText);
            label.Size = new Size(80, 20);  // 아래쪽 20px
            label.Dock = DockStyle.Bottom;
            label.Margin = new Padding(0, 10, 0, 0);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.MouseDoubleClick += mouseClickHandler;
            label.MouseDown += mouseDownHandler;
            label.MouseUp += mouseUpHandler;
            toolTip.SetToolTip(label, labelText);

            // 아이템 패널에 추가
            itemPanel.Controls.Add(pictureBox);
            itemPanel.Controls.Add(label);

            return itemPanel;
        }

        private string LimitFileNameLength(string name)
        {
            if (name.Length > 13)
            {
                return name.Substring(0, 10) + "...";
            }
            return name;
        }

        private void LoadImageFile(PictureBox pictureBox, string path)
        {
            string imageType = ImageTypeEnum.ofType(new FileInfo(path).Extension.Replace(".", ""));

            if (imageType.Equals("gif"))
            {
                pictureBox.Image = fileLogic.LoadFirstFrameOfGif(path);
            }
            else if(imageType.Equals("tga"))
            {
                pictureBox.Image = fileLogic.LoadTargaImage(path, true);
            }
            else if(!imageType.Equals(""))
            {
                pictureBox.Image = fileLogic.LoadDefaultImage(path, true);
            }
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
            {
                e.Node.Nodes.Clear();
                LoadSubDirectories(e.Node, (string)e.Node.Tag);
            }
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

                    if (di.GetDirectories().Length > 0)
                        subNode.Nodes.Add("");

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
                    currentTreeNode.Nodes.Clear();
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
            initPathBox(new FilePathDto("", this.targetPath));
            initFileList(1, this.targetPath);

            // 파일 뷰어 불러오기
            specialFolderEnum.initList();

            // TreeView 이벤트 연결
            this.tvFolders.BeforeExpand += tvFolders_BeforeExpand;

            Dictionary<string, string> existSpecialFolder = specialFolderEnum.getExistSpecialFolders();
            List<TreeNode> subNodes = new List<TreeNode>();

            foreach (var folder in existSpecialFolder)
            { 
                TreeNode thisNode = this.tvFolders.Nodes.Add(folder.Key);
                thisNode.Tag = folder.Value;

                if (folder.Value.Equals("My Computer"))
                {
                    foreach (string drive in Environment.GetLogicalDrives())
                    {
                        TreeNode subNode = thisNode.Nodes.Add(drive);
                        subNode.Tag = drive;
                        subNodes.Add(subNode);
                    }
                    continue;
                }

                if (!isExtended && specialFolderEnum.compareFolderPath(thisNode.Tag.ToString(), this.targetPath)
                    && tvFolders.SelectedNode == null)
                {
                    // 특정 경로까지 확장
                    ExpandToPath(thisNode, folder.Value, this.targetPath);
                    isExtended = true;
                    break;
                } else
                    thisNode.Nodes.Add("");
            }

            if (!isExtended)
            {
                foreach (TreeNode node in subNodes)
                {
                    if (!isExtended && specialFolderEnum.compareFolderPath(node.Tag.ToString(), this.targetPath)
                            && tvFolders.SelectedNode == null)
                    {
                        // 특정 경로까지 확장
                        ExpandToPath(node, node.Tag.ToString(), this.targetPath);
                        isExtended = true;
                        node.Parent.Expand();
                        break;
                    }
                    else
                        node.Nodes.Add("");
                }
            }
        }

        private void tvFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && tvFolders.SelectedNode != null)
            {
                string selectionPath = tvFolders.SelectedNode.Tag.ToString();
                initPathBox(new FilePathDto(null, selectionPath));
                initFileList(1, selectionPath);
            }
        }

        private void frmFileList_Resize(object sender, EventArgs e)
        {
            imgList.Width = this.Width - 305;
        }
    }
}
