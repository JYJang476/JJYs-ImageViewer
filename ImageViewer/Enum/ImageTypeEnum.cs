using System;
using System.Collections.Generic;
using System.Reflection;

namespace ImageViewer
{
    static class ImageTypeEnum
    {
        public const String TYPE_PNG = "png";
        public const String TYPE_GIF = "gif";
        public const String TYPE_JPG = "jpg";
        public const String TYPE_JPEG = "jpeg";
        //public const String TYPE_TGA = "tga";


        static public String ofType(String type)
        {
            Type imageTypeEnum = typeof(ImageTypeEnum);

            FieldInfo[] imageType = imageTypeEnum.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in imageType)
            {
                if (field.GetValue(null).Equals(type))
                {
                    return (String) field.GetValue(null);
                }
            }

            return "";
        }

        static public List<String> getImageTypes()
        {
            Type imageTypeEnum = typeof(ImageTypeEnum);

            FieldInfo[] fieldInfos = imageTypeEnum.GetFields(BindingFlags.Public | BindingFlags.Static);

            List<String> output = new List<string>();

            foreach (FieldInfo field in fieldInfos)
            {
                output.Add((String)field.GetValue(null));
            }
            
            return output;
        }

        static public String getImageTypeString(String separator)
        {
            String output = "";

            if (separator == null)
                separator = " ";

            Type imageTypeEnum = typeof(ImageTypeEnum);

            FieldInfo[] fieldInfos = imageTypeEnum.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in fieldInfos)
            {
                output += (String)field.GetValue(null) + " " + separator;
            }

            return output.Substring(0, output.Length - 2);
        }
    }
}
