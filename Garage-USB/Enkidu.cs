using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Linq;

namespace Garage_USB
{
    public class Enkidu
    {
        Form1 ui_form;
        std s;
        int std_delay = 16;
        public Enkidu(Form ui)
        {
            ui_form = (Form1)ui;
            s = new std();
        }

        public void log_apdu(byte[] data, int len)
        {
 
            StringBuilder ret = new StringBuilder();
            for(int i=0; i<len;i++)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", data[i]);
            }
            var hex = ret.ToString();
            Console.WriteLine(hex);
        }

        private int handle_cmd(byte[] apdu, int len, ref byte[] data, ref int data_len, int first)
        {
            byte[] cmd = s.get_cmd(1, len, apdu);
            byte[] res = new byte[1024 * 1024]; int res_len = 0;
            int rtn = def.RTN_FAIL;
            if(first==0)
                log_apdu(cmd, cmd.Length);
            if (first!=2)
                rtn = ui_form.g.c.send_cmd(cmd, cmd.Length, ref res, ref res_len, std_delay);
            else
                rtn = ui_form.g.c.send_cmd(cmd, cmd.Length, ref res, ref res_len, std_delay*300);

            if (rtn != def.RTN_OK)
            {
                Console.WriteLine("send_cmd error"+rtn.ToString());
                return rtn;
            }
            if (first==0)
                log_apdu(res, res_len);
            if (first!=2)
                rtn = s.get_result(res, res_len, ref data, ref data_len);
            else
            {
                Buffer.BlockCopy(res, 0, data, 0, res_len);
                data_len = res_len;
                return def.RTN_OK;
            }
            if (first == 0)
                Console.WriteLine("get_result rtn="+ rtn.ToString());
            return rtn;
        }

        private int enkidu_download()
        {
            return device.download_firmware(ui_form.g.firmware_path);
        }
        private int enkidu_getversion()
        {
            byte[] apdu = { 0xfb };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[7]);
                return rtn;
            }
            string version = Encoding.UTF8.GetString(data.Take(data_len).ToArray());
            if(version.Substring(0,8)!=config.version)
            {
                ui_form.BeginInvoke(new ThreadStart(delegate ()
                {
                    ui_form.lb_version.Text = version;
                }));
                call_fail(ui_form.g.BIN.BIN_CODE[21]);
                return def.RTN_FAIL;
            }

            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.lb_version.Text = version;
            }));
            return def.RTN_OK;

        }
        private int enkidu_cancel()
        {
            byte[] apdu = { 0x30 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                Console.WriteLine("detect transfer error");
                return rtn;
            }
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                //ui_form.lb_sn.Text = Encoding.UTF8.GetString(data.Take(data_len).ToArray());
                ui_form.lb_noise.Text = "1";
            }));
            return def.RTN_OK;
        }
        public int enkidu_kill()
        {
            if(ui_form.tb_admin.Text!="57625900")
            {
                call_fail(ui_form.g.BIN.BIN_CODE[34]);
                return def.RTN_FAIL;
            }
            reset_power();
            int rtn = ui_form.g.c.read_buffer();
            byte[] apdu = { 0x1A,0x01 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                Console.WriteLine("kill transfer error");
                call_fail(ui_form.g.BIN.BIN_CODE[34]);
                return rtn;
            }
            rtn = ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.lb_err_message.Text = "Cleared!";
                ui_form.tb_admin.Text = "";
                ui_form.tb_admin.Focus();
            }));
           

            return def.RTN_OK;
        }

        private int enkidu_getrand(ref byte[] rand)
        {
            byte[] apdu = { 0x14 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[25]);
                return rtn;
            }
            Buffer.BlockCopy(data, 0, rand, 0, 4);
            rand[4] = (byte)~rand[0];
            rand[5] = (byte)~rand[1];
            rand[6] = (byte)~rand[2];
            rand[7] = (byte)~rand[3];

            return def.RTN_OK;
        }
        private int enkidu_active(byte[] code)
        {
            byte[] apdu = new byte[1+28];
            apdu[0] = 0xfd;
            Buffer.BlockCopy(code, 0, apdu, 1, 28);
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[2]);
                return rtn;
            }
            return def.RTN_OK;
        }

        private int enkidu_writesn(byte[] sn)
        {
            byte[] apdu = new byte[17];
            apdu[0] = 0xfc;
            Buffer.BlockCopy(sn, 0, apdu, 1, 16);
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[26]);//TODO: always here
                return rtn;
            }
            return def.RTN_OK;

        }
        public string bytetohex(byte[] byteArray)//byte[]转HEXString
        {
            // string str = "";
            var str = new System.Text.StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
            {
                str.Append(String.Format("{0:X}", byteArray[i]));//var拼接
            }
            string s = str.ToString();
            return s;

        }
        private int enkidu_getsn()
        {

            byte[] apdu = { 0x34, 0 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[8]);
                return rtn;
            }
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                //ui_form.lb_sn.Text = Encoding.UTF8.GetString(data.Take(data_len).ToArray());
                ui_form.lb_sn.Text = bytetohex(data.Take(data_len).ToArray());
            }));
            return def.RTN_OK;

        }

        private int enkidu_checksensor()
        {
            byte[] apdu = { 0x36 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[4]);
                return rtn;
            }
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                //ui_form.lb_sn.Text = Encoding.UTF8.GetString(data.Take(data_len).ToArray());
                ui_form.lb_noise.Text = "1";
            }));
            return def.RTN_OK;
        }

        private int enkidu_setsleep()
        {
            byte[] apdu = { 0x33 };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 0);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[23]);
                return rtn;
            }
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                //ui_form.lb_sn.Text = Encoding.UTF8.GetString(data.Take(data_len).ToArray());
                ui_form.lb_noise.Text = "1";
            }));
            return def.RTN_OK;
        }

        private int enkidu_getimg()
        {
            byte[] apdu_up = { 0x0a };
            byte[] apdu_get = { 0x29 };
            byte[] get_res = new byte[1024 * 1024]; int get_len = 0;
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = def.RTN_FAIL;
            int counter = 200;
            //loop to get enroll img
            
            Thread.Sleep(100);
            do
            {
                counter--;
                if (counter == 0)
                {
                    call_fail(ui_form.g.BIN.BIN_CODE[20]);
                    return def.RTN_TIMEOUT;
                }
                Thread.Sleep(16);
                rtn = handle_cmd(apdu_get, apdu_get.Length, ref get_res, ref get_len, 1);
                if (rtn == 0x02)
                    continue;
                else if (rtn == 0)
                    break;
                else
                {
                    call_fail(ui_form.g.BIN.BIN_CODE[10]);
                    return rtn;
                }
            }
            while (true);
            ui_form.ui_tr.Stop();
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                Console.Beep(2766, 200);
            }));


            rtn = handle_cmd(apdu_up, apdu_up.Length, ref data, ref data_len, 2);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[10]);
                return rtn;
            }
            byte[] buffer = new byte[192 * 192 * 2];
            //0~11 07
            for(int i=0;i<144;i++)
            {
                Buffer.BlockCopy(   data.Skip(12).Take(data_len-12).ToArray(), 
                                    9+i*(128+11), 
                                    buffer, 
                                    i*128, 128);
            }

            byte[] img_buf = new byte[192 * 192];
            for(int i=0;i<192*192/2; i++)
            {
                img_buf[2*i] = (byte)(buffer[i] & 0xf0);
                img_buf[2*i+1] = (byte)(((buffer[i] & 0x0f)<<4)&0xf0);
            }

            int[] para = fpimage.get_otsu(img_buf);
            int rv = para[3] - para[2];
            int avg = fpimage.average_frame(img_buf);
            Image img = bmp_helper.format_from_bytes(img_buf, 0);
            img.Save(csv_log.path + ui_form.lb_sn.Text + ".bmp");
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.img_preview.Image = img;
                ui_form.lb_rv.Text = rv.ToString();
                ui_form.lb_graylevel.Text = avg.ToString();
            }));
            return def.RTN_OK;
        }

        //===========enkidu process=============
        //power up sensor vcc and mcu vcc
        //get version, writesn, getsn, checksensor
        //sensor vcc power off
        //get current result
        //wait for finger
        //once on power on mcu vcc
        //delay 20ms and re-confirm the touch out
        //get image
        //final
        //=======================================
        public int reset_power()
        {
            int rtn = def.RTN_FAIL;
            rtn = ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            Console.WriteLine("Power off both!");
            Thread.Sleep(100);
            ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.UP);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return def.RTN_FAIL;
            }
            Console.WriteLine("Power On! both");
            Thread.Sleep(100);
            return def.RTN_OK;
        }
        public int work()
        {
            int rtn = def.RTN_FAIL;
            init_prime_work();

            rtn = reset_power();
            if (rtn != def.RTN_OK)
            {
                return def.RTN_FAIL;
            }
            set_process(def.stage_power_up);

            rtn = ui_form.g.c.read_buffer();
            Console.WriteLine("read buffer result " + rtn.ToString());

            //goto TEST;
            //check if there is bootloader device
            rtn = enkidu_cancel();
            Console.WriteLine("detect result "+ rtn.ToString());
            if (rtn== def.RTN_OK)
                goto TEST;
            else if (rtn == def.RTN_TIMEOUT)
                goto DOWNLOAD;
            else if(rtn==1)
                goto ACTIVE;
            else
            {
                call_fail(ui_form.g.BIN.BIN_CODE[31]);
                return def.RTN_FAIL;
            }    

            
           
        DOWNLOAD:
            set_process(def.stage_download);
            if (enkidu_download()!=def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[1]);
                return def.RTN_FAIL;
            }
            rtn = reset_power();
            if (rtn != def.RTN_OK)
            {
                return def.RTN_FAIL;
            }


        ACTIVE:

            set_process(def.stage_restart);
            rtn = ui_form.g.c.read_buffer();
            if (rtn != def.RTN_OK)
            {
                Console.WriteLine("read buffer result " + rtn.ToString());
                call_fail(ui_form.g.BIN.BIN_CODE[1]);
                return def.RTN_FAIL;

            }
            int act_counter = 0;
            rtn = device.get_license_count(ref act_counter);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[27]);
                return rtn;
            }
            ui_form.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                ui_form.lb_act_count.Text = act_counter.ToString();
            }));
            if (act_counter==0)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[28]);
                return rtn;
            }
            //wirte sn
             byte[] sn = new byte[16];
             char[] c_sn = device.gen_sn(config.station, config.keycode).ToArray();
            for (int k = 0; k < 16; k++)
                sn[k] = (byte)c_sn[k];
                
             rtn = enkidu_writesn(sn);
             if (rtn != def.RTN_OK)
                 return rtn;

            byte[] rand = new byte[8];
            rtn = enkidu_getrand(ref rand);
            if (rtn != def.RTN_OK)
                return rtn;
            //get code here
            byte[] code = new byte[64];
            int code_len = 64;
            rtn = device.get_activate_data(rand, 8, code, ref code_len);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[27]);
                return rtn;
            }
                
            rtn = enkidu_active(code);
            if (rtn != def.RTN_OK)
                return rtn;


            ui_form.g.c.open_port(config.working_port);
        TEST:
            set_process(def.stage_calibrate);
            Console.WriteLine("Getversion");
            rtn = enkidu_getversion();
            if (rtn != def.RTN_OK)
                return rtn;
            Console.WriteLine("get sn");
            rtn = enkidu_getsn();
            if (rtn != def.RTN_OK)
                return rtn;
            Console.WriteLine("check sensor");
            rtn = enkidu_checksensor();
            if (rtn != def.RTN_OK)
                return rtn;
            Console.WriteLine("set sleep");
            ui_form.set_process(def.stage_press);
            rtn = enkidu_setsleep();
            if (rtn != def.RTN_OK)
                return rtn;
            rtn = ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            if (rtn != def.RTN_OK)
            {
                return def.RTN_FAIL;
            }

            Thread.Sleep(16);
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                Console.Beep(2766, 200);
            }));
            ui_form.ui_tr.Start();
           
            Console.WriteLine("Press finger!");
            float[] cv = ui_form.g.con.fn_get_cv(1,1);
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.lb_current.Text = Convert.ToInt32(cv[0]).ToString();
            }));
            if(cv[1]> config.c_th_low)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[32]);
                return rtn;
            }


            rtn = ui_form.g.con.wait_touch_out(500);
            if (rtn == def.RTN_ON)//high at first
            {
                call_fail(ui_form.g.BIN.BIN_CODE[18]);
                return rtn;
            }
            else if (rtn == def.RTN_TIMEOUT)//no response
            {
                call_fail(ui_form.g.BIN.BIN_CODE[19]);
                return rtn;
            }
            else if(rtn== def.RTN_FAIL)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[15]);
                return rtn;
            }
            Console.WriteLine("Finger detect!");
            rtn = ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.UP);
            if (rtn != def.RTN_OK)
            {
                return def.RTN_FAIL;
            }

            Thread.Sleep(32);

            Console.WriteLine("wait touch out!");
            rtn = ui_form.g.con.wait_touch_out(500);
            if (rtn == def.RTN_FAIL)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[19]);
                return rtn;
            }
            float[] cv2 = ui_form.g.con.fn_get_cv(3,0);
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.lb_voltage.Text = Convert.ToInt32(cv2[1]).ToString();
            }));
            if (cv2[1] > config.c_th_high)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[32]);
                return rtn;
            }

            Console.WriteLine("enkidu_getversion!");
            rtn = enkidu_getversion();
            if (rtn != def.RTN_OK)
                return rtn;
            Console.WriteLine("enkidu_getimg!");
            rtn = enkidu_getimg();
            if (rtn != def.RTN_OK)
                return rtn;

            final_prime_work();
            return def.RTN_OK;
        }
        int prime_dispatch(int func, int p1)
        {
            int rtn = def.RTN_FAIL;
            ui_form.g.await = 1;
            ui_form.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                switch (func)
                {
                    case def.FUNC_DOWNLOAD:
                        rtn = ui_form.g.prime_download();
                        break;
                    case def.FUNC_ACTIVE:
                        rtn = ui_form.g.prime_active();
                        break;
                    case def.FUNC_SINGAL:
                        rtn = ui_form.g.prime_signal(p1);
                        break;
                    case def.FUNC_FINAL:
                        rtn = ui_form.g.prime_final();
                        break;
                    default:
                        break;
                }
            }));
            while (ui_form.g.await == 1)
            {
                Thread.Sleep(10);
            }
            if (rtn != def.RTN_OK)
                call_fail(ui_form.g.BIN.BIN_CODE[rtn]);
            return rtn;
        }
        public void init_prime_work()
        {
            ui_form.g.button_down = 0;
            ui_form.g.working = 1;
            ui_form.g.active_opt_done = 0;
            ui_form.g.download_opt_done = 0;
            ui_form.set_process(def.stage_start);
            ui_form.init_tips();
            ui_form.init_lbs();
            ui_form.img_preview.Image = null;
            ui_form.g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.DOWN);
            ui_form.count_tr.Stop();
            ui_form.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                ui_form.lb_counter.Text = "0秒";
                ui_form.lb_counter.ForeColor = Color.Black;
            }));
           
            ui_form.done_counter = 0;
        }
        public void call_fail(int code)
        {
            ui_form.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                ui_form.g.call_fail(code);
                ui_form.lb_err_message.Text = ui_form.g.BIN.message(code);
                ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
                if (ui_form.thread != null)
                {
                    ui_form.g.ram_counter_bad++;
                    ui_form.lb_pf.Text = ui_form.g.ram_counter_good.ToString() + "/" + ui_form.g.ram_counter_bad.ToString();
                }
                ui_form.btn_start.BackColor = Color.White;
                ui_form.btn_tip_blink.BackColor = Color.Red;
                ui_form.thread = null;
                ui_form.g.working = 0;
                ui_form.ui_tr.Stop();
                ui_form.count_tr.Start();
                ui_form.done_counter = 0;
            }));
        }

        public void set_process(int stage)
        {
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                ui_form.g.set_process(stage);
            }));
        }
        public void final_prime_work()
        {
            ui_form.set_process(def.stage_result);
            ui_form.set_process(def.stage_result_ok);
            prime_dispatch(def.FUNC_FINAL, 0);
            ui_form.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                ui_form.lb_pf.Text = ui_form.g.ram_counter_good.ToString() + "/" + ui_form.g.ram_counter_bad.ToString();
                ui_form.btn_start.BackColor = Color.White;
            }));
            ui_form.count_tr.Start();
            ui_form.done_counter = 0;

            Thread.Sleep(4500);
            ui_form.set_process(def.stage_start);
            ui_form.init_tips();
            ui_form.init_lbs();
            ui_form.img_preview.Image = null;
            ui_form.thread = null;

        }

    }
}
