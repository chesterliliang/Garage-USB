using System;
using System.Threading;
using System.IO.Ports;
using System.Linq;


namespace Garage_USB
{
    public class control
    {
        public SerialPort sp;
        public enum MODE : int
        {
            UP = 1,
            DOWN = 2
        }
        public enum COMMAND : int
        {
            POWER = 1,
            S0 = 2,
            USB = 3
        }

        public static byte channel = 3;
        //a5000901010000aaaa
        public byte[] power_up = { 0xa5, 0x00, 0x09, channel, 0x01, 0x00, 0x00, 0xAA, 0xAA };//1
        //a5000901020000aaaa
        public static byte[] power_down = { 0xa5, 0x00, 0x09, channel, 0x02, 0x00, 0x00, 0xAA, 0xAA };//2
        //返回数据：A50009012F00188DD7 其中：0x012F=303 为电压值3.03*100 0x0018=24mA电流值 
        //a50009010a0000aaaa
        public static byte[] get_cv = { 0xa5, 0x00, 0x09, channel, 0x0a, 0x00, 0x00, 0xAA, 0xAA };//3
        //a5000901070001aaaa
        public static byte[] s0_high = { 0xa5, 0x00, 0x09, channel, 0x07, 0x00, 0x01, 0xAA, 0xAA };//4
        //a5000901070002aaaa
        public static byte[] s0_low = { 0xa5, 0x00, 0x09, channel, 0x07, 0x00, 0x02, 0xAA, 0xAA };//5
        //a5000901080001aaaa
        public static byte[] usb_en_high = { 0xa5, 0x00, 0x09, channel, 0x08, 0x00, 0x01, 0xAA, 0xAA };//6
        //a5000901080002aaaa
        public static byte[] usb_en_low = { 0xa5, 0x00, 0x09, channel, 0x08, 0x00, 0x02, 0xAA, 0xAA };//7
        public control()
        {
        }


        public int open_port(string com_number)
        {
            sp = new SerialPort(config.comport);
            sp.BaudRate = 115200;
            sp.DataBits = 8;
            sp.StopBits = System.IO.Ports.StopBits.One;
            sp.Parity = System.IO.Ports.Parity.None;
            sp.ReadBufferSize = 128;


            try
            {
                sp.Open();
            }
            catch (Exception err)
            {
                Console.WriteLine("com open fail! com=" + config.comport + " " + err.Message);
                return def.RTN_FAIL;
            }

            return def.RTN_OK;
        }

        public int switch_control(int command, int mode)
        {
            try
            {
                if (command == (int)COMMAND.POWER)
                {
                    if (mode == (int)MODE.UP)
                        sp.Write(power_up, 0, 9);
                    else if (mode == (int)MODE.DOWN)
                        sp.Write(power_down, 0, 9);
                }
                else if (command == (int)COMMAND.S0)
                {
                    if (mode == (int)MODE.UP)
                        sp.Write(s0_high, 0, 9);
                    else if (mode == (int)MODE.DOWN)
                        sp.Write(s0_low, 0, 9);
                }
                else if (command == (int)COMMAND.USB)
                {
                    if (mode == (int)MODE.UP)
                        sp.Write(usb_en_high, 0, 9);
                    else if (mode == (int)MODE.DOWN)
                        sp.Write(usb_en_low, 0, 9);
                }

            }
            catch (Exception err)
            {
                Console.WriteLine("fn_power_up write fail " + err.Message);
                return def.RTN_FAIL;
            }

            while (sp.BytesToRead == 0)
            {
                Thread.Sleep(1);
            }

            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            Thread.Sleep(100);
            int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            byte[] sw1sw2 = readBuffer.Skip(3).Take(2).ToArray();
            if (sw1sw2[0] == 0 && sw1sw2[1] == 0)
                return def.RTN_OK;
            else
                return def.RTN_FAIL;
        }

        public float[] fn_get_cv()
        {
            float[] array = new float[2];
            try
            {
                sp.Write(get_cv, 0, 9);
            }
            catch (Exception err)
            {
                Console.WriteLine("fn_power_up write fail " + err.Message);
                return null;
            }
            while (sp.BytesToRead == 0)
            {
                Thread.Sleep(1);
            }
            Thread.Sleep(100);
            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            byte[] v = readBuffer.Skip(3).Take(2).ToArray();
            byte[] c = readBuffer.Skip(5).Take(2).ToArray();

            int v_i = v[0] * 256 + v[1];
            float v_f = (float)(v_i / 100.0);
            array[0] = v_f;

            int c_i = c[0] * 256 + c[1];
            float c_f = (float)c_i;
            array[1] = c_f;
            return array;
        }
    }
}
