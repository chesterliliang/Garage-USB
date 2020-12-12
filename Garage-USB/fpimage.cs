using System;
using System.Collections.Generic;
using System.Text;


namespace Garage_USB
{
    public class fpimage
    {

        public static void bmp_frame(byte[] frame, byte[] bg )
        {
            for(int i=0; i< config.sensor_width*config.sensor_height;i++)
            {
                int tmp = frame[i] - bg[i];
                frame[i] = range_pixel(tmp);
                frame[i] = (byte)(0xff - frame[i]);
            }
        }
        public static byte range_pixel(int px)
        {
            byte rv = 0;
            if (px < 0)
                px = 0;
            else if (px > 0xff)
                px = 0xff;
            rv = (byte)px;
            return rv;
        }

        public static byte to_intensity(byte pixel)
        {
            int px = 0xff - pixel;
            px = (px * 16 - 8) / 24;
            byte rv = (byte)(0xff - range_pixel(px));
            return rv;
        }
        public static void back2intensity(byte[] raw)
        {
            for (int i = 0; i < 9216; i++)
            {
                raw[i + 1078] = to_intensity(raw[i + 1078]);
            }
        }

        public static byte gain_pixel(byte pixel,int gain)
        {
            int px = 0xff - pixel;
            px = (px * gain + 8) / 16;
            byte rv = (byte)(0xff - range_pixel(px));
            return rv;
        }

        public static void gain_frame(byte[] frame, byte[] gained, int gain)
        {
            for (int i = 0; i < config.sensor_width*config.sensor_height; i++)
            {
                gained[i] = gain_pixel(frame[i], gain);
            }
        }
        public static void gain_image(byte[] raw, byte[] img, int gain)
        {
            for (int i = 0; i <1078 ; i++)
                img[i] = raw[i];
            for (int i = 0; i < config.sensor_width * config.sensor_height; i++)
            {
                img[i + 1078] = gain_pixel(raw[i + 1078], gain);
            }
        }

        public static int merge_frames(byte[] frames, int count, byte[] merged)
        {
            int[] int_avg_frame = new int[config.sensor_width * config.sensor_height];
            // average 10 frame
            for (int i = 0; i < config.sensor_width * config.sensor_height; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    int_avg_frame[i] += frames[i + config.sensor_width * config.sensor_height * j];
                }
                merged[i] = (byte)(int_avg_frame[i] / count);
            }

            return average_frame(merged);
        }
        public static int average_frame(byte[] image)
        {
            long avg = 0;
            long sum = 0;
            for(int i=0;i< config.sensor_width * config.sensor_height; i++)
            {
                sum += image[i];
            }

            avg = sum / (config.sensor_width * config.sensor_height);
            return (int)avg;
        }

        public static float average_noise(byte[] image)
        {
            float avg = 0;
            float sum = 0;
            for (int i = 0; i < config.sensor_width * config.sensor_height; i++)
            {
                sum += image[i];
            }

            avg = sum / (config.sensor_width * config.sensor_height);
            return avg;
        }



        public static int[] get_otsu(byte[] pPicture)
        {
            int width = config.sensor_width;
            int height = config.sensor_height;
            int[] para = new int[8];
            int max_value = 255;


            float otsu, Wb, ub, Wf, uf, max_otsu = 0;
            int i, th, max_th = 0, bg_num = 0, fg_num = 0,
                       nB, nF, sumB, sumF;
            byte bg_avg = 0, fg_avg = 0;

            for (th = 0; th < 256; th++)
            {

                nB = 0;
                nF = 0;
                sumB = 0;
                sumF = 0;

                for (i = 0; i < width * height; i++)
                {
                    if (pPicture[i] > th)
                    {
                        nF++;
                        sumF += pPicture[i];
                    }
                    else
                    {
                        nB++;
                        sumB += pPicture[i];
                    }
                }

                if ((nB == 0) || (nF == 0))
                {
                    continue;
                }

                Wb = (float)nB / ((float)width * height);
                Wf = (float)nF / ((float)width * height);

                ub = (float)sumB / (float)nB; /* avg of background */
                uf = (float)sumF / (float)nF; /* avg of foreground */

                otsu = Wb * Wf * (ub - uf) * (ub - uf);

                if (otsu > max_otsu)
                {
                    max_otsu = otsu;
                    max_th = th;

                    bg_avg = (byte)ub;
                fg_avg = (byte)uf;
                bg_num = nB;
                fg_num = nF;
                }
             }

            for (i = 0; i < width * height; i++)
            {
                if (max_value > pPicture[i])
                    max_value = pPicture[i];
            }

            para[0] = bg_num;
            para[1] = fg_num;
            para[2] = bg_avg;
            para[3] = fg_avg;
            para[4] = max_th;
            para[5] = 255 - max_value;
            return para;


        }
    }
}
