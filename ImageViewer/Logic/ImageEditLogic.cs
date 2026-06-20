using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer.Logic
{
    internal class ImageEditLogic
    {
        private System.Drawing.Image originalImage = null;
        private float zoomFactor=1.0f;

        private float offsetX = 0;
        private float offsetY = 0;

        private System.Drawing.Size imagePanelSize = new System.Drawing.Size(0, 0);

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

                graphics.TranslateTransform(offsetX, offsetY);
                graphics.ScaleTransform(this.zoomFactor, this.zoomFactor);
                graphics.DrawImage(originalImage, 0, 0, this.imagePanelSize.Width, this.imagePanelSize.Height);
            }
        }

        public void zoomImage(Boolean zoomInOut, System.Drawing.Point mousePoint)
        {
            float oldZoom = this.zoomFactor;

            if (zoomInOut)
                this.zoomFactor *= 1.1f; // 확대
            else
                this.zoomFactor /= 1.1f; // 축소

            this.zoomFactor = Math.Max(1.0f, Math.Min(this.zoomFactor, 20f));

            this.offsetX = mousePoint.X - ((mousePoint.X - this.offsetX) * (this.zoomFactor / oldZoom));
            this.offsetY = mousePoint.Y - ((mousePoint.Y - this.offsetY) * (this.zoomFactor / oldZoom));
        }
    }
}
