using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
