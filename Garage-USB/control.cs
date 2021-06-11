using System;
using System.Threading;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

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
            USB = 3,
            LED = 4,
            POWER_MCU = 5,
        }

        public byte channel = 0;
        //a5000901010000aaaa
        public byte[] power_up = { 0xa5, 0x00, 0x09, 0, 0x01, 0x00, 0x00, 0xAA, 0xAA };//1 well ...
        //a5000901020000aaaa
        public byte[] power_down = { 0xa5, 0x00, 0x09, 0, 0x02, 0x00, 0x00, 0xAA, 0xAA };//2 in 1st board, channel is set

        public byte[] power_up_mcu = { 0xa5, 0x00, 0x09, 0x2, 0x01, 0x00, 0x00, 0xAA, 0xAA };//1 // 2nd board 1 means m1, 2 means m2
        //a5000901020000aaaa
        public byte[] power_down_mcu = { 0xa5, 0x00, 0x09, 0x2, 0x02, 0x00, 0x00, 0xAA, 0xAA };//2 .. be careful

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

        //a5000901080001aaaa
        public byte[] led_high = { 0xa5, 0x00, 0x09, 0, 0x0b, 0x00, 0x01, 0xAA, 0xAA };//14
        //a5000901080002aaaa
        public byte[] led_low = { 0xa5, 0x00, 0x09, 0, 0x0b, 0x00, 0x02, 0xAA, 0xAA };//15

        public byte[] touch_start = { 0xa5, 0x00, 0x09, 1, 0x0c, 0x00, 0x01, 0xAA, 0xAA };

        public byte[] touch_pull = { 0xa5, 0x00, 0x09, 1, 0x0c, 0x00, 0x02, 0xAA, 0xAA };

        public byte[] touch_stop= { 0xa5, 0x00, 0x09, 1, 0x0c, 0x00, 0x03, 0xAA, 0xAA };

        public byte[] get_version = { 0xa5, 0x00, 0x09, 1, 0xfd, 0x00, 0x00, 0xAA, 0xAA };

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
            led_high[3] = channel;
            led_low[3] = channel;
        }

        public int close_port()
        {
            if (sp != null)
            {
                sp.Close();
                return def.RTN_OK;
            }
            return def.RTN_FAIL;
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
            sp = new SerialPort(com_number);
            sp.BaudRate = 115200;
            sp.DataBits = 8;
            sp.StopBits = System.IO.Ports.StopBits.One;
            sp.Parity = System.IO.Ports.Parity.None;
            sp.ReadBufferSize = 128;
            sp.DtrEnable = true;
            sp.RtsEnable = true;


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
                else if(command == (int)COMMAND.LED)
                {
                    if (mode == (int)MODE.UP)
                        sp.Write(led_high, 0, 9);
                    else if (mode == (int)MODE.DOWN)
                        sp.Write(led_low, 0, 9);
                }
                else if(command == (int)COMMAND.POWER_MCU)
                {
                    if (mode == (int)MODE.UP)
                        sp.Write(power_up_mcu, 0, 9);
                    else if (mode == (int)MODE.DOWN)
                        sp.Write(power_down_mcu, 0, 9);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("fn_power_up write fail switch_control" + err.Message +" " + command.ToString());
                return def.RTN_FAIL;
            }

            while (sp.BytesToRead == 0)
            {
                Thread.Sleep(16);//TODO timeout
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
        public int get_vr(ref int version)
        {
            try
            {
                sp.Write(get_version, 0, 9);
            }
            catch (Exception err)
            {
                Console.WriteLine("get_version write fail " + err.Message);
                return def.RTN_FAIL;
            }
            int counter = 10;
            while (sp.BytesToRead == 0)
            {
                Thread.Sleep(16);
                counter--;
                if (counter == 0)
                    return def.RTN_TIMEOUT;
            }

            Thread.Sleep(100);
            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            version = readBuffer[6];
            return def.RTN_OK;
        }


        public float[] fn_get_cv(int ch, int mode)
        {
            float[] array = new float[2];
            get_cv[3] = (byte)ch;
            if(mode==1)//uA
            {
                get_cv[6] = 1;
            }
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
                sp.Write(get_cv, 0, 9);
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
                    Thread.Sleep(20);
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
                Thread.Sleep(16);

                if (should_leave == 1)
                {
                    should_leave = 0;
                    break;
                }
                   
            }
            //stop_pull_button();

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
        public int wait_touch_out(int timeout_ms)
        {
            int rtn = def.RTN_FAIL;
            try
            {
                sp.Write(touch_start, 0, 9);
            }
            catch (Exception err)
            {
                Console.WriteLine("wait_touch_out write fail " + err.Message);
                return def.RTN_FAIL;
            }
            while (sp.BytesToRead == 0)
                Thread.Sleep(16);

            byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
            int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            int status = (int)readBuffer[4];
            if (status == 1)
            {
                return def.RTN_ON;
            }
            do
            {
                try
                {
                    sp.Write(touch_pull, 0, 9);
                }
                catch (Exception err)
                {
                    Console.WriteLine("wait_touch_out write fail " + err.Message);
                    return def.RTN_FAIL;
                }
                while (sp.BytesToRead == 0)
                    Thread.Sleep(16);

                readBuffer = new byte[sp.ReadBufferSize + 1];
                count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
                status = (int)readBuffer[4];
                if (status == 1)
                {
                    rtn = def.RTN_OK;
                    break;
                }
                timeout_ms--;
                if(timeout_ms==0)
                {
                    rtn = def.RTN_TIMEOUT;
                    break;
                }
                Thread.Sleep(16);

            } while (true);

            try
            {
                sp.Write(touch_stop, 0, 9);
            }
            catch (Exception err)
            {
                Console.WriteLine("wait_touch_out write fail " + err.Message);
                return def.RTN_FAIL;
            }
            while (sp.BytesToRead == 0)
                Thread.Sleep(16);

            readBuffer = new byte[sp.ReadBufferSize + 1];
            count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
            return rtn;

        }

        // <summary>
        /// 通过vid，pid获得串口设备号
        /// </summary>
        /// <param name="vid">vid</param>
        /// <param name="pid">pid</param>
        /// <returns>串口号</returns>
        public string GetPortNameFormVidPid(string vid, string pid)
        {
            //{4d36e978-e325-11ce-bfc1-08002be10318}

            Guid GUID_DEVINTERFACE_DFU = new Guid(0x4d36e978, 0xe325, 0x11ce, 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18);

            Guid classGuid = Guid.Empty;

            Guid myGUID = Guid.Empty;
            string enumerator = "USB";

            IntPtr hDevInfo = Win32.SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, Win32.DIGCF_DEVICEINTERFACE | Win32.DIGCF_PRESENT);
            //if (hDevInfo.ToInt32() == Win32.INVALID_HANDLE_VALUE)
            if(false)
            {
                Console.WriteLine("read hardware information error");
            }
            else
            {
                SP_DEVINFO_DATA devInfoData = new SP_DEVINFO_DATA();
                devInfoData.cbSize = 32;
                devInfoData.classGuid = Guid.Empty;
                devInfoData.devInst = 0;
                devInfoData.reserved = IntPtr.Zero;
                bool result = Win32.SetupDiEnumDeviceInfo(hDevInfo, 0, devInfoData);
                if (false == result)
                {
                    int error = Marshal.GetLastWin32Error();
                    //if (error != Win32.ERROR_NO_MORE_ITEMS)
                      //  throw new Win32Exception(error);
                }

                SP_DEVICE_INTERFACE_DATA ifData = new SP_DEVICE_INTERFACE_DATA();
                ifData.cbSize = (uint)Marshal.SizeOf(ifData);
                ifData.Flags = 0;
                ifData.InterfaceClassGuid = Guid.Empty;
                ifData.Reserved = IntPtr.Zero;

                bool result2 = Win32.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref classGuid, 0, ifData);
                if (result2 == false)
                {
                    int error = Marshal.GetLastWin32Error();
                   // if (error != Win32.ERROR_NO_MORE_ITEMS)
                       // throw new Win32Exception(error);
                }

                uint needed;

                // This returns: needed=160, result3=false and error=122 ("The data area passed to a system call is too small")
                bool result3 = Win32.SetupDiGetDeviceInterfaceDetail(hDevInfo, ifData, null, 0, out needed, null);
                if (result3 == false)
                {
                    int error = Marshal.GetLastWin32Error();
                }

               // IntPtr detailDataBuffer = IntPtr.Zero;
                SP_DEVICE_INTERFACE_DETAIL_DATA ifDetailsData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                ifDetailsData.devicePath = new byte[needed - 4];
                ifDetailsData.cbSize = (uint)Marshal.SizeOf(ifDetailsData);

                IntPtr detailDataBuffer = Marshal.AllocHGlobal((int)needed);
                Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
                uint nBytes = needed;

                bool result4 = Win32.SetupDiGetDeviceInterfaceDetail(hDevInfo, ifData, ifDetailsData, nBytes, out needed, null);
                if (result4 == false)
                {
                    int error = Marshal.GetLastWin32Error();
                    //if (error != Win32.ERROR_NO_MORE_ITEMS)
                    //    throw new Win32Exception(error);
                }

                IntPtr pDevicePathName = new IntPtr(detailDataBuffer.ToInt32() + 4);
                String devicePathName = Marshal.PtrToStringAuto(pDevicePathName);
            }
            return null;
        }


    }
}
