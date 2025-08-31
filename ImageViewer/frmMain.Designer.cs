
namespace ImageViewer
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.imgViewer = new System.Windows.Forms.PictureBox();
            this.menuImg = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.같은폴더내의이미지탐색ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.imgViewer)).BeginInit();
            this.menuImg.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgViewer
            // 
            this.imgViewer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imgViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgViewer.Location = new System.Drawing.Point(0, 24);
            this.imgViewer.Name = "imgViewer";
            this.imgViewer.Size = new System.Drawing.Size(798, 654);
            this.imgViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgViewer.TabIndex = 0;
            this.imgViewer.TabStop = false;
            this.imgViewer.Click += new System.EventHandler(this.imgViewer_Click);
            // 
            // menuImg
            // 
            this.menuImg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem});
            this.menuImg.Location = new System.Drawing.Point(0, 0);
            this.menuImg.Name = "menuImg";
            this.menuImg.Size = new System.Drawing.Size(798, 24);
            this.menuImg.TabIndex = 1;
            this.menuImg.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.열기ToolStripMenuItem,
            this.같은폴더내의이미지탐색ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.열기ToolStripMenuItem.Text = "열기";
            this.열기ToolStripMenuItem.Click += new System.EventHandler(this.열기ToolStripMenuItem_Click);
            // 
            // 같은폴더내의이미지탐색ToolStripMenuItem
            // 
            this.같은폴더내의이미지탐색ToolStripMenuItem.Name = "같은폴더내의이미지탐색ToolStripMenuItem";
            this.같은폴더내의이미지탐색ToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.같은폴더내의이미지탐색ToolStripMenuItem.Text = "같은 폴더내의 이미지 탐색";
            this.같은폴더내의이미지탐색ToolStripMenuItem.Click += new System.EventHandler(this.같은폴더내의이미지탐색ToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 678);
            this.Controls.Add(this.imgViewer);
            this.Controls.Add(this.menuImg);
            this.MainMenuStrip = this.menuImg;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.imgViewer)).EndInit();
            this.menuImg.ResumeLayout(false);
            this.menuImg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgViewer;
        private System.Windows.Forms.MenuStrip menuImg;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 열기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 같은폴더내의이미지탐색ToolStripMenuItem;
    }
}

