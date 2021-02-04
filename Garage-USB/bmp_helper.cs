using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Garage_USB
{
    public class bmp_helper
    {
        public static int width = 0;
        public static int height = 0;
        private static int header_len = 1078;
        private static byte[] template;

        public static void image_view_init(PictureBox img_preview)
        {
            if (config.sensor_width == 100)
            {
                img_preview.Width = (int)(config.sensor_width * 3.2);
                img_preview.Height = (int)(config.sensor_height * 3.2);
                img_preview.Left = 85;
                img_preview.Top = 22;
            }
            else if (config.sensor_width == 160)
            {
                img_preview.Width = config.sensor_width * 2;
                img_preview.Height = config.sensor_height * 2;
                img_preview.Left = 85;
                img_preview.Top = 22;
            }
            else if (config.sensor_width == 144)
            {
                img_preview.Width = config.sensor_width * 2;
                img_preview.Height = config.sensor_height * 2;
                img_preview.Left = 100;
                img_preview.Top = 100;

            }
            else if (config.sensor_width == 103)
            {
                img_preview.Width = config.sensor_width * 3;
                img_preview.Height = config.sensor_height * 3;
                img_preview.Left = 90;
                img_preview.Top = 88;
            }
        }

        public static void init(fang g)
        {
            string ref_path = g.ref_path + @"\files\";
            width = config.sensor_width;
            height = config.sensor_height;
            //init bmp template

            ref_path += def.template_hearder + config.sensor_width.ToString() + def.template_tail;

            Console.WriteLine("bmp template path " + ref_path);

            g.bkg_img = new byte[config.sensor_width * config.sensor_height];

            template = null;
            template = new byte[header_len + width * height];
            try
            {
                FileStream fs = new FileStream(ref_path, FileMode.Open);
                fs.Read(template, 0, header_len + width * height);
                fs.Close();
                fs.Dispose();
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
