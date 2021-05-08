using System;
using System.Threading;
using System.IO.Ports;
using System.Linq;
using System.Text;


namespace Garage_USB
{
    public class com
    {
        public SerialPort sp;
        

        public com()
        {

        }
        public int open_port(string com_number)
        {
            if (sp != null)
            {
                try
                {
                    Console.WriteLine("Close before Open!");
                    sp.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Close before Open failed! " + e.Message);
                }
            }
            sp = null;
            sp = new SerialPort(com_number);
            sp.BaudRate = 57600;
            sp.DataBits = 8;
            sp.StopBits = System.IO.Ports.StopBits.One;
            sp.Parity = System.IO.Ports.Parity.None;
            sp.ReadBufferSize = 1024*1024;
            sp.RtsEnable = true;
            sp.DtrEnable = true;


            try
            {
                sp.Open();
            }
            catch (Exception err)
            {
                Console.WriteLine("com open fail! com=" + com_number + " " + err.Message);
                return def.RTN_FAIL;
            }

            return def.RTN_OK;
        }

        public int read_buffer()
        {
            byte[] data = new byte[1024 * 1024 + 1];
            int time_out = 100;
            while (sp.BytesToRead == 0)
            {
                time_out--;
                Thread.Sleep(16);
                if (time_out == 0)
                    return def.RTN_TIMEOUT;
            }
            int res_len = sp.Read(data, 0, sp.ReadBufferSize);
           
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < res_len; i++)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", data[i]);
            }
            var hex = ret.ToString();
            Console.WriteLine(hex);

            return def.RTN_OK;
        }

        public int send_cmd(byte[] cmd, int len, ref byte[] res, ref int res_len, int lap)
        {
            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            int time_out = 1024*256;
            try
            {
                sp.Write(cmd, 0, len);
            }
            catch (Exception err)
            {
                Console.WriteLine("send_cmd write fail " + err.Message);
                return def.RTN_FAIL;
            }
            while (sp.BytesToRead == 0)
            {
                time_out--;
                if (time_out == 0)
                    return def.RTN_TIMEOUT;
            }
            Thread.Sleep(lap);
            res_len = sp.Read(res, 0, sp.ReadBufferSize);


            return def.RTN_OK;
        }


    }
}
