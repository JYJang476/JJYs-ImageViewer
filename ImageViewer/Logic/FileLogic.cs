using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Processing;

namespace ImageViewer
{
    class FileLogic
    {
        const int THUMNAIL_WIDTH = 64;
        const int THUMNAIL_HEIGHT = 64;

        public List<FileInfo> GetFileInfos(String directoryPath)
        {
            return new DirectoryInfo(directoryPath).GetFiles().ToList();
        }

        public byte[] getFileData(String path)
        {
            return File.ReadAllBytes(path);
        }

        public ImageType getFileType(String path)
        {
            FileInfo fileInfo = new FileInfo(path);
            ImageType imageType = new ImageType(fileInfo.Extension.Replace(".", ""), fileInfo.Name);

            return imageType;
        }

        public Bitmap LoadFirstFrameOfGif(string path)
        {
            using (System.Drawing.Image gifImage = System.Drawing.Image.FromFile(path))
            {
                // 첫 번째 프레임만 선택
                FrameDimension dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);

                gifImage.SelectActiveFrame(dimension, 0);

                return new Bitmap(gifImage.GetThumbnailImage(THUMNAIL_WIDTH, THUMNAIL_HEIGHT, () => false, IntPtr.Zero));
            }
        }

        public System.Drawing.Image LoadAnimatingGif(string path)
        {
            return System.Drawing.Image.FromFile(path);
        }

        public Bitmap LoadDefaultImage(string path, bool isThumnail)
        {
            System.Drawing.Image bufferImage = new Bitmap(path).GetThumbnailImage(THUMNAIL_WIDTH, THUMNAIL_HEIGHT, () => false, IntPtr.Zero);
            if (isThumnail)
                return new Bitmap(bufferImage);

            return new Bitmap(path);
        }

        public  Bitmap LoadTargaImage(string path, bool isThumnail)
        {
            // ImageSharp로 TGA 이미지 로드
            using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(path, new TgaDecoder()))
            {
                if (isThumnail)
                {
                    // 비율 유지하면서 축소 크기 계산
                    float scale = Math.Min((float)THUMNAIL_WIDTH / image.Width, (float)THUMNAIL_HEIGHT / image.Height);
                    int newWidth = (int)(image.Width * scale);
                    int newHeight = (int)(image.Height * scale);

                    // ImageSharp 내부에서 리사이징
                    image.Mutate(x => x.Resize(newWidth, newHeight));
                }

                Bitmap bitmap = ConvertToBitmap(image);

                return bitmap;
            }
        }

        private Bitmap ConvertToBitmap(SixLabors.ImageSharp.Image<Rgba32> image)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < image.Height; y++)
            {
                Span<Rgba32> pixelRowSpan = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = pixelRowSpan[x];
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }
    }
}
