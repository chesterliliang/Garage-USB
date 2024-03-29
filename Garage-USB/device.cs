﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Garage_USB
{
    public class device
    {
        public static int ERR_FAIL = -1;
        public static int ERR_OK = 0;

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_DownCOS", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_DownCOS(char[] sCosPath);
        public static int download_firmware(string path)
        {
            int rtn = ERR_FAIL;
            char[] path_c =path.ToCharArray();
            Console.WriteLine("enter download_firmware!");
            rtn = PAPRO_DownCOS(path_c);
            Console.WriteLine("leave download_firmware! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_WriteSN", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_WriteSN(char[] sSN, int dev_type, char[] serialname);
        public static int set_sn_activiate(string sn, int dev_type,string serialname)
        {
            int rtn = ERR_FAIL;
            char[] sn_c = sn.ToCharArray();
            Console.WriteLine("enter set_sn_activiate!");
            if (serialname == null)
            {
                char[] void_prt = new char[2];
                void_prt[0] = (char)0;
                void_prt[1] = (char)0;//inner strlen<4 then                                                                                                   not set
                rtn = PAPRO_WriteSN(sn_c, dev_type, void_prt);
            }
            else
            {
                char[] c_name = serialname.ToCharArray();
                rtn = PAPRO_WriteSN(sn_c, dev_type, c_name);
            }

            
            Console.WriteLine("leave set_sn_activiate ! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Connect", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Connect(int type, char[] serialname);

        public static unsafe int connect(int type, string serialname)
        {
            Console.WriteLine("enter connet! " + type.ToString());
            int rtn = ERR_FAIL;
            
            if (serialname == null)
            {
                char[] void_prt = new char[2];
                void_prt[0] = (char)0;
                void_prt[1] = (char)0;//inner strlen<4 then                                                                                                   not set
                rtn = PAPRO_Connect(type, void_prt);
            }
            else
            {
                char[] c_name = serialname.ToCharArray();
                rtn = PAPRO_Connect(type, c_name);
            }
            Console.WriteLine("leave connet! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;

        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_DisConnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_DisConnect();
        public static int disconnect()
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter disconnect!");
            rtn = PAPRO_DisConnect();
            Console.WriteLine("leave disconnect! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }


        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Calibrate", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Calibrate();
        public static int calibrate()
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter calibrate!");
            rtn = PAPRO_Calibrate();
            Console.WriteLine("leave calibrate! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Capture_Frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Capture_Frame(char mode, byte[] frame, ref int len);

        public static int capture_frame(int mode, byte[] frame, ref int len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter capture_frame! mode="+mode.ToString());
            char c = (char)mode;
            rtn = PAPRO_Capture_Frame(c,frame,ref len);
            //if(rtn==ERR_FAIL)
            Console.WriteLine("leave capture_frame! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Config", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Config(char mode, byte[] list, ref int len);
        public static int config(int mode, byte[] list,ref int len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter config! mode=" + mode.ToString());
            char c = (char)mode;
            rtn = PAPRO_Config(c, list, ref len);
            Console.WriteLine("leave config! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Gainth", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Gainth( char mode, byte[] para);

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_COSinfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_COSinfo( char mode, byte[] info);

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetVersion(byte[] version, ref int len);
        public static int version(byte[] version, ref int len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter version!");
            rtn = PAPRO_GetVersion(version,ref len);
            Console.WriteLine("leave version! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Write", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Write(char[] buf, int offset, int len);
        public static int write_flash(char[] buf, int offset, int len)
        {
            int rtn = ERR_FAIL;
            byte[] data = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
                data[i] = (byte)buf[i];
            Console.WriteLine("enter write_flash!");
            rtn = PAPRO_Write(buf, offset,len);
            Console.WriteLine("leave write_flash! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Read", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Read( int offset, byte[] buf, ref int len);
        public static int read_flash(char[] buf, int offset, ref int len)
        {
            int rtn = ERR_FAIL;
            byte[] data = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
                data[i] = (byte)buf[i];
            Console.WriteLine("enter read_flash!");
            rtn = PAPRO_Read(offset, data, ref len);
            Console.WriteLine("leave read_flash! rv = 0x" + String.Format("{0:X000}", rtn));
            for (int i = 0; i < buf.Length; i++)
                buf[i] = (char)data[i];
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetDevInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetDevInfo(byte[] info,ref int len);
        public static int devinfo(byte[] version, ref int len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter devinfo!");
            rtn = PAPRO_GetDevInfo(version, ref len);
            Console.WriteLine("leave devinfo! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_Abort", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_Abort();
        public static int abort()
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter abort!");
            rtn = PAPRO_Abort();
            Console.WriteLine("leave abort! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetBgImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetBgImage(byte[] bgimg);

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetLicenseCounter", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetLicenseCounter(ref int count);
        public static int get_license_count(ref int count)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter get_license_count!");
            rtn = PAPRO_GetLicenseCounter(ref count);
            Console.WriteLine("leave get_license_count! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetActivateData", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetActivateData(byte[] rand, int rand_len, byte[] code, ref int code_len);
        public static int get_activate_data(byte[] rand, int rand_len, byte[] code, ref int code_len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter get_activate_data!");
            rtn = PAPRO_GetActivateData(rand, rand_len,code,ref code_len);
            Console.WriteLine("leave get_activate_data! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        public static int get_bg(byte[] bgimg)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter get_bg!");
            rtn = PAPRO_GetBgImage(bgimg);
            Console.WriteLine("leave get_bg! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

        public static string bytesToHexString(byte[] bArr, int len)
        {
            string result = string.Empty;
            for (int i = 0; i < len; i++)//逐字节变为16进制字符，以%隔开
            {
                result += Convert.ToString(bArr[i], 16).ToUpper().PadLeft(2, '0');
            }
            return result;
        }
        public static string gen_sn(int station,string keycode)
        {
            string s_keycode = keycode.ToString();
            string s_station = "";
            if (station < 10)
                s_station = "0" + station.ToString();
            else
                s_station = station.ToString();

            Random ra = new Random();
            string sn = "";
            for (int i=0;i<10; i++)
            {
                int tmp = ra.Next(1, 9);
                sn += tmp.ToString();
            }
            return s_keycode + s_station + sn;
            
            
        }

        [DllImport("./bin/product.dll", EntryPoint = "PAPRO_GetImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int PAPRO_GetImage(byte[] bgimg, ref int len);

        public static int get_img(byte[] bgimg, ref int len)
        {
            int rtn = ERR_FAIL;
            Console.WriteLine("enter get_img!");
            rtn = PAPRO_GetImage(bgimg, ref len);
            Console.WriteLine("leave get_img! rv = 0x" + String.Format("{0:X000}", rtn));
            return rtn;
        }

    }
}
