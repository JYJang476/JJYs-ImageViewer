using ImageViewer.Enum;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ImageViewer.Logic
{
    internal class ImageEditLogic
    {
        private System.Drawing.Image originalImage = null;
        private float zoomFactor=1.0f;

        private System.Drawing.PointF lastMousePos = new System.Drawing.PointF();
        private System.Drawing.PointF mouseOffset = new System.Drawing.PointF();

        private System.Drawing.Size imagePanelSize = new System.Drawing.Size(0, 0);

        private Stack<int> viewStatusStack = new Stack<int>();

        private int viewerStatus = ViewerStatusEnum.NORMAL;

        public ImageEditLogic()
        {
            this.viewStatusStack.Push(ViewerStatusEnum.NORMAL);
        }

        public void setImagePanelSize(System.Drawing.Size size)
        {
            imagePanelSize = size;
        }

        public void setOriginImage(System.Drawing.Image image)
        {
            zoomFactor = 1.0f; // 줌값은 초기화
            this.originalImage = image;
        }

        public void drawImage(Graphics graphics)
        {
            if (originalImage != null)
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphics.TranslateTransform(mouseOffset.X, mouseOffset.Y);
                graphics.ScaleTransform(this.zoomFactor, this.zoomFactor);
                graphics.DrawImage(originalImage, 0, 0, this.imagePanelSize.Width, this.imagePanelSize.Height);
            }
        }

        public void zoomImage(Boolean zoomInOut, System.Drawing.Point mousePoint)
        {
            // 현재의 뷰어상태를 스택에 백업
            this.viewStatusStack.Push(this.viewerStatus);

            // 마우스 줌상태인지 확인
            if (this.viewerStatus == 1.0f) this.setViewerStatus(ViewerStatusEnum.NORMAL);
            else if (this.zoomFactor != 1.0f) this.setViewerStatus(ViewerStatusEnum.ZOOM);

            float oldZoom = this.zoomFactor;

            if (zoomInOut)
                this.zoomFactor *= 1.1f; // 확대
            else
                this.zoomFactor /= 1.1f; // 축소

            this.zoomFactor = Math.Max(1.0f, Math.Min(this.zoomFactor, 20f));

            this.mouseOffset.X = mousePoint.X - ((mousePoint.X - this.mouseOffset.X) * (this.zoomFactor / oldZoom));
            this.mouseOffset.Y = mousePoint.Y - ((mousePoint.Y - this.mouseOffset.Y) * (this.zoomFactor / oldZoom));

            // 시점 정리
            LimitOffset();
        }

        public void moveImage(System.Drawing.Point mousePoint)
        {
            this.mouseOffset.X += mousePoint.X - this.lastMousePos.X;
            this.mouseOffset.Y += mousePoint.Y - this.lastMousePos.Y;

            this.lastMousePos = mousePoint;

            // 시점 정리
            LimitOffset();
        }

        public void initImageMove(System.Drawing.Point mousePoint)
        {
            this.lastMousePos = mousePoint;
        }

        public void setViewerStatus(int status) { this.viewerStatus = status; }

        public int getViewerStatus() {
            return this.viewerStatus; 
        }

        public void roolBackStatus() { 
            if (this.viewStatusStack.Count > 0) this.setViewerStatus(this.viewStatusStack.Pop()); 
        }

        private void LimitOffset()
        {
            float scaledWidth = this.imagePanelSize.Width * this.zoomFactor;
            float scaledHeight = this.imagePanelSize.Height * this.zoomFactor;

            // 이미지가 패널보다 큰 경우
            if (scaledWidth > this.imagePanelSize.Width)
            {
                float minX = this.imagePanelSize.Width - scaledWidth;
                float maxX = 0;

                this.mouseOffset.X = Math.Max(minX, Math.Min(maxX, this.mouseOffset.X));
            }
            else
            {
                // 이미지가 패널보다 작으면 가운데 정렬
                this.mouseOffset.X = (this.imagePanelSize.Width - scaledWidth) / 2;
            }

            if (scaledHeight > this.imagePanelSize.Height)
            {
                float minY = this.imagePanelSize.Height - scaledHeight;
                float maxY = 0;

                this.mouseOffset.Y = Math.Max(minY, Math.Min(maxY, this.mouseOffset.Y));
            }
            else
            {
                this.mouseOffset.Y = (this.imagePanelSize.Height - scaledHeight) / 2;
            }
        }
    }
}
