using System;

namespace ImageViewer
{
    class ImageType
    {
        private String type;
        private String path;

        public ImageType(String type, String path)
        {
            this.Type = type;
            this.path = path;
        }

        public String Path { get => path; set => path = value; }
        public String Type { get => type; set => type = value; }
    }
}
