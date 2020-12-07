using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace Garage_USB
{
    public class bmp_helper
    {
        public static int width = 0;
        public static int height = 0;
        private static int header_len = 1078;

        private static byte[] template;

        public static void init(int w, int h, string ref_path)
        {
            width = w;
            height = h;
            if (template == null)
            {
                template = new byte[header_len + width * height];
            }
            try
            {
                FileStream fs = new FileStream(ref_path, FileMode.Open);
                fs.Read(template, 0, header_len + width * height);
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("bmp_init error " + e.Message);
            }

        }
        public static Image load_from_file(string path)
        {
            byte[] buf = new byte[header_len + width * height];
            Image image = null;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Read(buf, 0, header_len + width * height);
                fs.Close();
                MemoryStream ms = new MemoryStream(buf);
                image = Image.FromStream(ms);
            }
            catch(Exception e)
            {
                Console.WriteLine("load_from_file error " + e.Message);
                return null;
            }
           
            return image;
        }

        public static Image format_from_bytes(byte[] buf,int gain)
        {
            byte[] bmp = new byte[header_len + width * height];
            Buffer.BlockCopy(template, 0, bmp, 0, header_len + width * height);
            Buffer.BlockCopy(buf, 0, bmp, header_len, width * height);
            if(gain>0)
                fpimage.gain_image(bmp, bmp, gain);
            MemoryStream ms = new MemoryStream(bmp);
            Image image = Image.FromStream(ms);
            return image;

        }
    }
}
