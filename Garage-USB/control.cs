using System;
using System.Threading;
using System.IO.Ports;
using System.Linq;


namespace Garage_USB
{
    public class control
    {
        public SerialPort sp;
        public int should_leave = 0;
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

        public byte channel = 0;
        //a5000901010000aaaa
        public byte[] power_up = { 0xa5, 0x00, 0x09, 0, 0x01, 0x00, 0x00, 0xAA, 0xAA };//1
        //a5000901020000aaaa
        public byte[] power_down = { 0xa5, 0x00, 0x09, 0, 0x02, 0x00, 0x00, 0xAA, 0xAA };//2
        //返回数据：A50009012F00188DD7 其中：0x012F=303 为电压值3.03*100 0x0018=24mA电流值 
        //a50009010a0000aaaa
        public byte[] get_cv = { 0xa5, 0x00, 0x09, 0, 0x0a, 0x00, 0x00, 0xAA, 0xAA };//3
        //a5000901070001aaaa
        public  byte[] s0_high = { 0xa5, 0x00, 0x09, 0, 0x07, 0x00, 0x01, 0xAA, 0xAA };//4
        //a5000901070002aaaa
        public  byte[] s0_low = { 0xa5, 0x00, 0x09, 0, 0x07, 0x00, 0x02, 0xAA, 0xAA };//5
        //a5000901080001aaaa
        public  byte[] usb_en_high = { 0xa5, 0x00, 0x09, 0, 0x08, 0x00, 0x01, 0xAA, 0xAA };//6
        //a5000901080002aaaa
        public byte[] usb_en_low = { 0xa5, 0x00, 0x09, 0, 0x08, 0x00, 0x02, 0xAA, 0xAA };//7

        //a5000901060001aaaa
        public byte[] button_monitor_start = { 0xa5, 0x00, 0x09, 0, 0x06, 0x00, 0x01, 0xAA, 0xAA };//8
        //a5000901060001aaaa
        public byte[] button_monitor_pull = { 0xa5, 0x00, 0x09, 0, 0x06, 0x00, 0x02, 0xAA, 0xAA };//9
        //a5000901060001aaaa
        public byte[] button_monitor_stop = { 0xa5, 0x00, 0x09, 0, 0x06, 0x00, 0x03, 0xAA, 0xAA };//10
        //a5000901040000aaaa
        public byte[] check_current= { 0xa5, 0x00, 0x09, 0, 0x04, 0x00, 0x00, 0xAA, 0xAA };//11
        //a5000901050001aaaa
        public byte[]start_key_clear = { 0xa5, 0x00, 0x09, 0, 0x05, 0x00, 0x01, 0xAA, 0xAA };//12
        //a5000901050002aaaa
        public byte[] start_key_check = { 0xa5, 0x00, 0x09, 0, 0x05, 0x00, 0x02, 0xAA, 0xAA };//13

        public control()
        {
            set_channel();
        }

        public void set_channel()
        {
            channel = (byte)config.channel;
            power_up[3] = channel;
            power_down[3] = channel;
            get_cv[3] = channel;
            s0_high[3] = channel;
            s0_low[3] = channel;
            usb_en_high[3] = channel;
            usb_en_low[3] = channel;
            button_monitor_start[3] = channel;
            button_monitor_pull[3] = channel;
            button_monitor_stop[3] = channel;
            check_current[3] = channel;
            start_key_clear[3] = channel;
            start_key_check[3] = channel;
        }


        public int open_port(string com_number)
        {
            if(sp!=null)
            {
                try
                {
                    Console.WriteLine("Close before Open!");
                    sp.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Close before Open failed! " + e.Message);
                }
            }
            sp = null;    
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

        public int fn_check_current()
        {
            try
            {
                sp.Write(check_current, 0, 9);
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

            Thread.Sleep(100);
            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            int v1 = (int)readBuffer[4];
            int v2 = (int)readBuffer[3];
            if (v1==0&&v2==0)//voltage is zero
                return def.RTN_FAIL;
            return def.RTN_OK;
        }

        public int check_button_short()
        { 
           byte[] readBuffer = null;
           int count = 0;
           stop_pull_button();
           try
           {
              sp.Write(button_monitor_start, 0, 9);
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
           readBuffer = new byte[sp.ReadBufferSize + 1];
           count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
           int status = (int)readBuffer[4];

           if (status == 1)
           {
              return def.RTN_FAIL;
           }
            return def.RTN_OK;
        }

        public int stop_pull_button()
        {
            byte[] readBuffer = null;
            int count = 0;
            try
            {
                sp.Write(button_monitor_stop, 0, 9);
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
            readBuffer = new byte[sp.ReadBufferSize + 1];
            count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
          
            return def.RTN_OK;
        }

        public int wait_button(int timeout_ms)
        {
            int got = 0;
            byte[] readBuffer = null;
            int count = 0;

            //pull
            while (timeout_ms>0 || timeout_ms<0)
            {
                try
                {
                    sp.Write(button_monitor_pull, 0, 9);
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
                readBuffer = new byte[sp.ReadBufferSize + 1];
                count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
                int status = (int)readBuffer[4];
                if(status==1)
                {
                    got = 1;
                    break;
                }
                if(timeout_ms>0)
                    timeout_ms--;
                Thread.Sleep(1);

                if (should_leave == 1)
                {
                    should_leave = 0;
                    break;
                }
                   
            }
            stop_pull_button();

            if (got == 1)
                return def.RTN_OK;

            return def.RTN_FAIL;
        }
        public int wait_start_key(int timeout_ms)
        {
            int got = 0;
            byte[] readBuffer = null;
            int count = 0;

            try
            {
                sp.Write(start_key_clear, 0, 9);
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

            readBuffer = new byte[sp.ReadBufferSize + 1];
            count = sp.Read(readBuffer, 0, sp.ReadBufferSize);

            //pull
            while (timeout_ms > 0 || timeout_ms < 0)
            {
                try
                {
                    sp.Write(start_key_check, 0, 9);
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

                readBuffer = new byte[sp.ReadBufferSize + 1];
                count = sp.Read(readBuffer, 0, sp.ReadBufferSize);

                int status = (int)readBuffer[4];
                if (status == 1)
                {
                    got = 1;
                    break;
                }
                if (timeout_ms > 0)
                    timeout_ms--;
                Thread.Sleep(1);
                if(should_leave==1)
                {
                    should_leave = 0;
                    return def.RTN_FAIL;
                }
                    
            }

            if (got == 1)
                return def.RTN_OK;

            return def.RTN_FAIL;
        }
    }
}
