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

        public Bitmap LoadFirstFrameOfGif(string path, bool isThumnail)
        {
            using (System.Drawing.Image gifImage = System.Drawing.Image.FromFile(path))
            {
                // 프레임 차원을 가져옵니다 (보통 시간 기반 애니메이션)
                FrameDimension dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);

                // 첫 번째 프레임만 선택
                gifImage.SelectActiveFrame(dimension, 0);

                // 첫 프레임을 복사해서 PictureBox에 표시
                if (isThumnail)
                    return new Bitmap(gifImage.GetThumbnailImage(64, 64, () => false, IntPtr.Zero));

                return new Bitmap(gifImage);
            }
            return null;
        }

        public Bitmap LoadDefaultImage(string path, bool isThumnail)
        {
            System.Drawing.Image bufferImage = new Bitmap(path).GetThumbnailImage(64, 64, () => false, IntPtr.Zero);
            if (isThumnail)
                return new Bitmap(bufferImage);

            return new Bitmap(path);
        }

        public  Bitmap LoadTargaImage(string path, bool isThumnail)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            // ImageSharp로 TGA 이미지 로드
            using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(path, new TgaDecoder()))
            {
                if (isThumnail)
                {
                    // 비율 유지하면서 축소 크기 계산
                    float scale = Math.Min((float)64 / image.Width, (float)64 / image.Height);
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
