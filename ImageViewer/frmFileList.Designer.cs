
namespace ImageViewer
{
    partial class frmFileList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tvFolders = new System.Windows.Forms.TreeView();
            this.pRoot = new System.Windows.Forms.Panel();
            this.imgList = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tvFolders
            // 
            this.tvFolders.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tvFolders.Location = new System.Drawing.Point(-2, 38);
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.Size = new System.Drawing.Size(285, 600);
            this.tvFolders.TabIndex = 1;
            this.tvFolders.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFolders_BeforeExpand);
            // 
            // pRoot
            // 
            this.pRoot.Dock = System.Windows.Forms.DockStyle.Top;
            this.pRoot.Location = new System.Drawing.Point(0, 0);
            this.pRoot.Name = "pRoot";
            this.pRoot.Size = new System.Drawing.Size(851, 39);
            this.pRoot.TabIndex = 2;
            this.pRoot.Paint += new System.Windows.Forms.PaintEventHandler(this.pRoot_Paint);
            // 
            // imgList
            // 
            this.imgList.Location = new System.Drawing.Point(289, 38);
            this.imgList.Name = "imgList";
            this.imgList.Size = new System.Drawing.Size(562, 600);
            this.imgList.TabIndex = 3;
            // 
            // frmFileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 643);
            this.Controls.Add(this.imgList);
            this.Controls.Add(this.pRoot);
            this.Controls.Add(this.tvFolders);
            this.Name = "frmFileList";
            this.Text = "frmFileList";
            this.Load += new System.EventHandler(this.frmFileList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TreeView tvFolders;
        private System.Windows.Forms.Panel pRoot;
        private System.Windows.Forms.Panel imgList;
    }
}