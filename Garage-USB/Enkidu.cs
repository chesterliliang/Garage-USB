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

        private int handle_cmd(byte[] apdu, int len, ref byte[] data, ref int data_len, int first)
        {
            byte[] cmd = s.get_cmd(1, len, apdu);
            byte[] res = new byte[1024 * 1024]; int res_len = 0;
            int rtn = def.RTN_FAIL;
            if(first!=2)
                rtn = ui_form.g.c.send_cmd(cmd, cmd.Length, ref res, ref res_len, std_delay);
            else
                rtn = ui_form.g.c.send_cmd(cmd, cmd.Length, ref res, ref res_len, std_delay*300);

            if (rtn != def.RTN_OK)
            {
                //call_fail(ui_form.g.BIN.BIN_CODE[22]);
                return def.RTN_FAIL;
            }
            if(first!=2)
                rtn = s.get_result(res, res_len, ref data, ref data_len);
            else
            {
                Buffer.BlockCopy(res, 0, data, 0, res_len);
                data_len = res_len;
                return def.RTN_OK;
            }

            return rtn;
        }
        private int enkidu_getversion()
        {
            byte[] apdu = { 0xfb };
            byte[] data = new byte[1024 * 1024]; int data_len = 0;
            int rtn = handle_cmd(apdu, apdu.Length, ref data, ref data_len, 1);
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

        private void enkidu_writesn()
        {

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
                rtn = handle_cmd(apdu_get, apdu_get.Length, ref get_res, ref get_len, 0);
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
        public int work()
        {
            int rtn = def.RTN_FAIL;
            init_prime_work();

            ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            Console.WriteLine("Power off both!");
            Thread.Sleep(32);
            ui_form.g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.UP);
            Console.WriteLine("Power On! both");
            Thread.Sleep(100);

            ui_form.g.c.open_port(config.working_port);

            set_process(def.stage_power_up);
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
            set_process(def.stage_restart);
            Console.WriteLine("set sleep");
            rtn = enkidu_setsleep();
            if (rtn != def.RTN_OK)
                return rtn;
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            Thread.Sleep(16);
            ui_form.BeginInvoke(new ThreadStart(delegate ()
            {
                Console.Beep(2766, 200);
            }));
            set_process(def.stage_calibrate);
            ui_form.ui_tr.Start();
           
            Console.WriteLine("Press finger!");

            rtn = ui_form.g.con.wait_touch_out(500);
            if (rtn != def.RTN_OK)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[18]);
                return rtn;
            }
            Console.WriteLine("Finger detect!");
            ui_form.g.con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.UP);

            Thread.Sleep(32);

            Console.WriteLine("wait touch out!");
            rtn = ui_form.g.con.wait_touch_out(500);
            if (rtn == def.RTN_FAIL)
            {
                call_fail(ui_form.g.BIN.BIN_CODE[19]);
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
            ui_form.thread = null;
        }

    }
}
