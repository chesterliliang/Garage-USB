using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;




namespace Garage_USB
{
    public partial class Form1 : Form
    {

        //System.Timers.Timer live_tr = new System.Timers.Timer();

        System.Timers.Timer ui_tr = new System.Timers.Timer();
        byte[] bkg_img = null;
        ThreadStart threadStart = null;
        ThreadStart threadLive = null;
        int live_state = 0;
        ThreadStart threadPress = null;
        ThreadStart threadMP = null;
        int working = 0;

        Thread thread = null;
        Thread thread_live = null;
        Thread thread_press = null;
        Thread thread_mp = null;
        int press_counter = 0;
        DataTable dt = new DataTable();
        DataTable dt_config = new DataTable();
        int ssm_state = 0;
        string log_path = "";
        int ram_counter_good = 0;
        int ram_counter_bad = 0;
        control con;
        int event_device_changed = 0;
        string firmware_path = "";
        int button_down = 0;
        int preview_noise = 0;

        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("App start up!");
            threadStart = new ThreadStart(do_prime);
            threadLive = new ThreadStart(do_preview);
            threadPress = new ThreadStart(do_press);
            threadMP = new ThreadStart(do_mp);

            load_config();
            con = new control();
            open_create_log();
            firmware_path = Application.StartupPath + @"\" + config.keycode + def.firmware_file;
            Console.WriteLine("firmware path = " + firmware_path);
            //init bmp template
            bmp_helper.init(config.sensor_width, config.sensor_height, Application.StartupPath + def.template_name);

            csv_log.path = Application.StartupPath + @"\img\";
            Console.WriteLine("img storage path = " + csv_log.path);


            bkg_img = new byte[config.sensor_width * config.sensor_height];
            //set 
            ui_tr.AutoReset = true;
            ui_tr.Elapsed += new ElapsedEventHandler(ui_tr_event);
            ui_tr.Interval = 200;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            lb_project.Text = config.project_name;
            lb_product.Text = config.product_code;
            lb_station.Text = config.station.ToString();
            lb_pf.Text = ram_counter_good.ToString() + "/" + ram_counter_bad.ToString();
            btn_tip_blink.BackColor = Color.White;
            set_process(def.stage_start, def.RTN_OK);
            tb_com.Text = config.comport;
            if(con.open_port(config.comport)==def.RTN_OK)
                tb_com.BackColor = Color.Green;
            else
                tb_com.BackColor = Color.Red;

            btn_start.BackColor = Color.White;
            btn_start.Text = "Click Button to Start";

            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
            con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);
            if (config.auto_start==1)
            {
                Console.WriteLine("auto started!");
                thread_mp = new Thread(threadMP);
                thread_mp.IsBackground = true;
                thread_mp.Start();
                cb_mp.Checked = true;
            }


        }
        private void init_lbs()
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_voltage.Text = "-1";
                lb_current.Text = "-1";
                lb_sn.Text = "-1";
                lb_parameter.Text = "-1";
                lb_version.Text = "-1";
                lb_rv.Text = "-1";
                lb_graylevel.Text = "-1";
                lb_bin.Text = "-1";
                lb_snr.Text = "-1";
                lb_dr.Text = "-1";
                lb_noise.Text = "-1";
            }));
        }
        private void init_tips()
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_result.BackColor = Color.White;
                btn_tip_blink.BackColor = Color.White;
                btn_result.Text = "作業中--勿碰模組";
            }));
           
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            
            if(cb_mp.Checked==false)
            {
                btn_start.BackColor = Color.Blue;
                if (thread != null)
                    return;
                thread = new Thread(threadStart);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                if (thread_mp != null)
                    return;

                thread_mp = new Thread(threadMP);
                thread_mp.IsBackground = true;
                thread_mp.Start();
            }
           
        }

        public void do_mp()
        {
            int rtn = 0;
            con.should_leave = 0;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
            con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);

            while (cb_mp.Checked==true)
            {
                if (working == 1)
                    continue;

                btn_start.BackColor = Color.Purple;

                this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                {
                    btn_result.Text = "推-開始按鈕";
                    btn_result.BackColor = Color.White;
                }));


                rtn = con.wait_start_key(-1);
                if (rtn == def.RTN_FAIL)
                {
                    call_fail(def.BIN_CODE_15);
                    return;
                }
                set_process(def.stage_start, def.RTN_OK);
                init_tips();
                init_lbs();
                img_preview.Image = null;

                con.stop_pull_button();
                rtn = con.check_button_short();
                if (rtn == def.RTN_FAIL)
                {
                    call_fail(def.BIN_CODE_12);
                    return;
                }

                do_prime();

               /* this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                {
                    btn_result.Text = "壓-模組按鍵";
                }));

                rtn = con.wait_button(-1);
                if (rtn == def.RTN_OK)
                    do_prime();
                else
                {
                    call_fail(def.BIN_CODE_18);
                    return;
                }
               */

            }
            thread_mp = null;
        }

        public  void do_prime()
        {
            int rtn = device.ERR_FAIL;
            int need_calirate = 0;
            button_down = 0;
            working = 1;
            set_process(def.stage_start, def.RTN_OK);
            init_tips();
            init_lbs();
            img_preview.Image = null;

            event_device_changed = 0;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP) ;
            Thread.Sleep(50);

            rtn = con.fn_check_current();
            if (rtn == def.RTN_FAIL)
            {
                call_fail(def.BIN_CODE_17);
                return;
            }

            float[] cv = con.fn_get_cv();
            if(cv!=null)
            {
                if (cv[1] < 1) //pin connect fail
                {
                    Console.WriteLine("Power up but no current!");
                    call_fail(def.BIN_CODE_F2);
                    return;
                }
                if (cv[1] > 100)//<50
                {
                    Console.WriteLine("too much current!");
                    call_fail(def.BIN_CODE_F4);
                    return;
                }

            }
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_voltage.Text = cv[0].ToString();
                lb_current.Text = cv[1].ToString();
            }));
            int wait_first_connect_counter = 0;
            while (event_device_changed<2)
            {
                Thread.Sleep(1);
                wait_first_connect_counter++;
                if (wait_first_connect_counter > 3000)
                {
                    Console.WriteLine("wait_first_connect_counter timeout");
                    break;
                }
                   
            }
            Console.WriteLine("leave waiting device while!");
            set_process(def.stage_power_up, def.RTN_OK);
            //============================================================//

            //to check if downloaded
            rtn = device.disconnect();
            rtn = device.connect(config.firmware_type);
            if (rtn == device.ERR_OK)
            {// already downloaded
                need_calirate = 1;
                //check if activated
                rtn = device.abort();
                if (rtn == device.ERR_OK)
                {//activated
                    goto TEST;
                }
                else
                {
                    goto ACTIVE;
                }
            }


                //============================================================//

                ui_tr.Start();
            rtn = device.download_firmware(firmware_path);
            ui_tr.Stop();

            if(rtn==def.RTN_OK)
            {
                set_process(def.stage_download, def.RTN_OK);
            }    
            else
            {
                set_process(def.stage_download, def.RTN_FAIL);
                call_fail(def.BIN_CODE_F3);
                return;
            }
               
            //============================================================//


            event_device_changed = 0;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);

            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_tip_restart.BackColor = Color.Yellow;
            }));
            //event_device_changed = 0;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(300);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_tip_restart.BackColor = Color.Blue;
            }));
            float[] cv2 = con.fn_get_cv();
            if (cv2 != null)
            {
                if (cv2[1] < 1)
                {
                    Console.WriteLine("Power up but no current!");
                    call_fail(def.BIN_CODE_F2);
                    return;
                }
                if (cv2[1] > 100)//<30
                {
                    Console.WriteLine("cos too much porwer!");
                    call_fail(def.BIN_CODE_F4);
                    return;
                }

            }
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_voltage.Text = cv2[0].ToString();
                lb_current.Text = cv2[1].ToString();
            }));
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_tip_restart.BackColor = Color.Purple;
            }));
            int wait_cos_plugin_counter = 0;
            while (event_device_changed < 2)
            {
                Thread.Sleep(1);
                wait_cos_plugin_counter++;
                if(wait_cos_plugin_counter>4000)// clear sensor
                {
                    Console.WriteLine("change device counter timeout");
                    break;
                }
            }
            set_process(def.stage_restart, def.RTN_OK);

            //============================================================//
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_tip_calibrate.BackColor = Color.Yellow;
            }));
            
            Thread.Sleep(2000);// cut down

        ACTIVE:
            rtn = device.disconnect();
            rtn = device.connect(config.firmware_type);
            if(rtn!=device.ERR_OK)
            {
                Console.WriteLine("thread connect after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_01);
                thread = null;
                return;
            }

            rtn = device.set_sn_activiate(device.gen_sn(config.station,config.keycode),config.dev_type);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread set_sn_activiate after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_10);
                thread = null;
                return;
            }

            TEST:
            byte[] version = new byte[64];
            int len = 64;
            rtn = device.version(version, ref len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread version after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_02);
                thread = null;
                return;
            }
            string strversion = System.Text.Encoding.UTF8.GetString(version);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_version.Text = strversion;
            }));

            byte[] info = new byte[38];
            int len_info = 0;
            rtn = device.devinfo(info, ref len_info);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread devinfo after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_02);
                thread = null;
                return;
            }
            string strsn = System.Text.Encoding.UTF8.GetString(info.Skip(6).Take(16).ToArray());
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_sn.Text = strsn;
            }));

            if (cb_calibrate.Checked == true||need_calirate==1)
            {
                rtn = device.calibrate();
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("thread calibrate after restart failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_04);
                    thread = null;
                    return;
                }
            }
          
            
            byte[] list = new byte[7];
            rtn = device.config(0, list);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread config after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_03);
                thread = null;
                return;
            }
            //string strconfig = System.Text.Encoding.UTF8.GetString(list);
            string strconfig = device.bytesToHexString(list, 7);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_parameter.Text = strconfig;
            }));
            rtn = device.get_bg(bkg_img);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread get_bg after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_05);
                return;
            }


            rtn = check_noise();
            if(rtn!=def.RTN_OK)
            {
                return;
            }


            set_process(def.stage_calibrate, def.RTN_OK);
            Console.Beep(2766, 200);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                img_preview.Image = null;
                btn_tip_press.BackColor = Color.Blue;
                btn_tip_blink.BackColor = Color.Blue;
            }));
            //restart and calbrate
            //======================================================//

            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_result.Text = "拉-橡膠手柄-確保到位";
            }));
            press_counter = 80;
            ui_tr.Start();
            byte[] frame_data = new byte[config.sensor_width * config.sensor_height];
            //preview image
            while (press_counter>0)
            {
                rtn = preview_image(frame_data);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_06);
                    return;
                }
                press_counter--;
            }

            if (average_check(frame_data) == def.RTN_FAIL)
            {
                return;
            }

            if (button_down == 0)
            {
                Console.WriteLine("button down check failed");
                set_process(def.stage_press, def.RTN_FAIL);
                call_fail(def.BIN_CODE_16);
                thread_press = null;
                return;
            }

            if (check_result())
            {
                set_process(def.stage_result, def.RTN_OK);
                set_process(def.stage_result_ok, def.RTN_OK);
                log_info();
                con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            }
            ram_counter_good++;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_pf.Text = ram_counter_good.ToString() + "/" + ram_counter_bad.ToString();
            }));
            Thread.Sleep(500);
            btn_start.BackColor = Color.White;
            thread = null;
            working = 0;
        }
        public int check_noise()
        {
            int rtn = def.RTN_FAIL;
            //do bg and empty image check
            byte[] empty_frames = new byte[config.sensor_width * config.sensor_height * 10];
            byte[] empty_frame = new byte[config.sensor_width * config.sensor_height];
            //get 10 frames
            for (int i = 0; i < 10; i++)
            {
                rtn = device.capture_frame(0, empty_frame);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 emptet capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_07);
                    return def.RTN_FAIL;
                }
                fpimage.bmp_frame(empty_frame, bkg_img);
                Buffer.BlockCopy(empty_frame, 0, empty_frames, i * config.sensor_width * config.sensor_height, config.sensor_width * config.sensor_height);
            }
            fpimage.merge_frames(empty_frames, 10, empty_frame);

            float empty_avg = 255;
            if (cb_enhance.Checked)
            {
                int sw_gain = Convert.ToInt32(tb_gain.Text);
                fpimage.gain_frame(empty_frame, empty_frame, sw_gain);
            }

            empty_avg = fpimage.average_noise(empty_frame);
            int[] para = fpimage.get_otsu(empty_frame);
            int rv = para[3] - para[2];
            if (empty_avg < 245 || rv > 10)
            {
                Console.WriteLine("too much noise");
                Image img = bmp_helper.format_from_bytes(empty_frame, 0);
                this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                {
                    img_preview.Image = img;
                    lb_rv.Text = rv.ToString();
                    int nAvg = (int)empty_avg;
                    lb_graylevel.Text = nAvg.ToString();


                }));
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_14);
                return def.RTN_FAIL;
            }
            float noise = 255 - empty_avg;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_noise.Text = noise.ToString("f2");
            }));
            return def.RTN_OK;
        }

        public int get_all_info(int mode)
        {
            float[] cv2 = con.fn_get_cv();
            int rtn = def.RTN_FAIL;
            if (cv2 != null && mode !=0)
            {
                if (cv2[1] < 1)
                {
                    Console.WriteLine("Power up but no current!");
                    call_fail(-2);
                    return def.RTN_FAIL;
                }
                if (cv2[1] > 100)
                {
                    Console.WriteLine("cos too much porwer!");
                    call_fail(16);
                    return def.RTN_FAIL;
                }

            }
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_voltage.Text = cv2[0].ToString();
                lb_current.Text = cv2[1].ToString();
            }));
            if (mode == 0)
                return def.RTN_OK;
            byte[] list = new byte[7];
            rtn = device.config(0, list);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread config after restart failed");
                return def.RTN_FAIL;
            }

            string strconfig = device.bytesToHexString(list, 7);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_parameter.Text = strconfig;
            }));
            byte[] info = new byte[38];
            int len = 0;
            rtn = device.devinfo(info, ref len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread devinfo after restart failed");
                call_fail(def.BIN_CODE_02);
                return def.RTN_FAIL;
            }
            string strsn = System.Text.Encoding.UTF8.GetString(info.Skip(6).Take(16).ToArray());
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_sn.Text = strsn;
            }));

            byte[] version = new byte[64];
            int len_version = 64;
            rtn = device.version(version,ref len_version);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread version after restart failed");
                call_fail(def.BIN_CODE_02);
                return def.RTN_FAIL;
            }
            string strversion = System.Text.Encoding.UTF8.GetString(version);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_version.Text = strversion;
            }));

            return def.RTN_OK;
        }

        public int preview_image(byte[] frame_data)
        {
            int rtn = def.RTN_FAIL;
            if (preview_noise == 0) 
            {
                rtn = check_noise();
                if (rtn != def.RTN_OK)
                {
                    Console.WriteLine("preview_image check noise failed");
                    return def.RTN_FAIL; ;
                }
                preview_noise = 1;
            }
          

            rtn = device.capture_frame(0, frame_data);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("capture_frame failed");
                return def.RTN_FAIL;
            }
            fpimage.bmp_frame(frame_data, bkg_img);
            

            Image img;
            if (cb_enhance.Checked)
            {
                int sw_gain = Convert.ToInt32(tb_gain.Text);
                img = bmp_helper.format_from_bytes(frame_data, sw_gain);
                fpimage.gain_frame(frame_data, frame_data, sw_gain);
            }
            else
                img = bmp_helper.format_from_bytes(frame_data, 0);

            int[] para = fpimage.get_otsu(frame_data);
            int rv = para[3] - para[2];
            int avg = fpimage.average_frame(frame_data);

            int signal = 255 - para[2];
            double f_noise = float.Parse(lb_noise.Text);
            double snr = 20 * Math.Log10(signal / f_noise);
            int max_value = para[5];
            double max_min = max_value * 1.0 / f_noise;
            double dr = 20 * Math.Log10(max_min);


            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                img_preview.Image = img;
                if(thread==null)
                {
                    lb_rv.Text = rv.ToString();
                    lb_graylevel.Text = avg.ToString();
                    if(avg<250)
                    {
                        lb_snr.Text = snr.ToString("f2");
                        lb_dr.Text = dr.ToString("f0");
                    }
                    else
                    {
                        lb_snr.Text = "-1";
                        lb_dr.Text = "-1";
                    }
                   
                }
 
            }));
            if(button_down==0)
            {
                con.stop_pull_button();
                con.check_button_short();
                rtn = con.wait_button(2);
                if (rtn == def.RTN_OK)
                {
                    button_down = 1;
                }
                else
                {
                    button_down = 0;
                }
            }
            return def.RTN_OK;
        }

        public int average_check(byte[] frame_data)
        {
            int rtn = def.RTN_FAIL;
            byte[] frame10 = new byte[10 * config.sensor_width * config.sensor_height];
            byte[] avg_frame = new byte[config.sensor_width * config.sensor_height];
            int avg = 0;

            //avg = fpimage.merge_frames(frame10, 10, avg_frame);

            //get 10 frames
            for (int i = 0; i < 10; i++)
            {
                rtn = device.capture_frame(0, frame_data);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_07);
                    return def.RTN_FAIL;
                }
                fpimage.bmp_frame(frame_data, bkg_img);
                Buffer.BlockCopy(frame_data, 0, frame10, i * config.sensor_width * config.sensor_height, config.sensor_width * config.sensor_height);
            }
            avg = fpimage.merge_frames(frame10, 10, avg_frame);
          
            // do report
            Image avg_img; 
            if (cb_enhance.Checked)
            {
                int sw_gain = Convert.ToInt32(tb_gain.Text);
                avg_img = bmp_helper.format_from_bytes(avg_frame, sw_gain);
                fpimage.gain_frame(avg_frame, avg_frame, sw_gain);
                avg = fpimage.average_frame(avg_frame);
            }        
            else
                avg_img = bmp_helper.format_from_bytes(avg_frame, 0);

            int[] para = fpimage.get_otsu(avg_frame);   
            int rv = para[3] - para[2];

            int signal = 255 - para[2];
            double f_noise = float.Parse(lb_noise.Text);
            double snr = 20 * Math.Log10(signal / f_noise);
            int max_value = para[5];
            double max_min = max_value * 1.0 / f_noise;
            double dr = 20 * Math.Log10(max_min);

            avg_img.Save(csv_log.path + lb_sn.Text + ".bmp");

            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                img_preview.Image = avg_img;
                lb_rv.Text = rv.ToString();
                lb_graylevel.Text = avg.ToString();
                lb_snr.Text = snr.ToString("f2");
                lb_dr.Text = dr.ToString("f0");

            }));
            ui_tr.Stop();
            if (avg > config.avg_th)
            {
                call_fail(def.BIN_CODE_21);
                Console.WriteLine("press failed avg=" + avg.ToString());
                set_process(def.stage_press, def.RTN_FAIL);
                return def.RTN_FAIL;
            }
            if (rv < config.rv_th)
            {
                call_fail(def.BIN_CODE_22);
                Console.WriteLine("press failed rv=" + rv.ToString());
                set_process(def.stage_press, def.RTN_FAIL);
                return def.RTN_FAIL;
            }
            set_process(def.stage_press, def.RTN_OK);
            return def.RTN_OK;
        }

        public void do_preview()
        {
            byte[] image = new byte[config.sensor_width * config.sensor_height];
            while(true)
            {
                if (live_state == 0)
                {
                    Console.WriteLine("do preview abort!");
                    thread_live = null; 
                    return;
                }
                   
                if(preview_image(image)==def.RTN_FAIL)
                {
                    Console.WriteLine("do preview fail!");
                    thread_live = null;
                    return;
                }
            }   
        }

        public void do_press()
        {
            int rtn = def.RTN_FAIL;
            press_counter = 80;
            ui_tr.Start();
            byte[] frame_data = new byte[config.sensor_width * config.sensor_height];
            //preview image
            while (press_counter > 0)
            {
                rtn = preview_image(frame_data);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_06);
                    thread_press = null;
                    return;
                }
                press_counter--;
            }
           

            if (average_check(frame_data) == def.RTN_FAIL)
            {
                thread_press = null;
                return;
            }

            //check button again for final
            if (button_down == 0)
            {
                Console.WriteLine("button down check failed");
                set_process(def.stage_press, def.RTN_FAIL);
                call_fail(def.BIN_CODE_15);
                thread_press = null;
                return;
            }


            if (check_result())
            {
                set_process(def.stage_result, def.RTN_OK);
                set_process(def.stage_result_ok, def.RTN_OK);
                log_info();
            }
            thread_press = null;

        }

        private void ui_tr_event(object source, System.Timers.ElapsedEventArgs e)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                if (ssm_state == def.stage_start)
                {
                    if (btn_tip_download.BackColor == Color.White)
                        btn_tip_download.BackColor = Color.Blue;
                    else
                        btn_tip_download.BackColor = Color.White;
                }
                else if (ssm_state == def.stage_restart)
                {
                    if (btn_tip_calibrate.BackColor == Color.White)
                    {
                        btn_tip_calibrate.BackColor = Color.Blue;
                    }
                    else if (btn_tip_calibrate.BackColor == Color.Blue)
                    {
                        btn_tip_calibrate.BackColor = Color.White;
                    }
                }
                else if(ssm_state == def.stage_calibrate)
                {
                    if (btn_tip_press.BackColor == Color.White)
                    {
                        btn_tip_press.BackColor = Color.Blue;
                        if(button_down==0)
                            btn_tip_blink.BackColor = Color.Blue;
                        else
                            btn_tip_blink.BackColor = Color.Green;
                    }
                    else if (btn_tip_press.BackColor == Color.Blue)
                    {
                        btn_tip_press.BackColor = Color.White;
                        btn_tip_blink.BackColor = Color.White;
                    }
                }
            }));
        }



        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == def.WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case def.DBT_DEVNODES_CHANGED:
                            event_device_changed += 1;
                            Console.WriteLine("device changed! "+ event_device_changed.ToString());
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref m);
        }
        private bool check_result()
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_result.Text = "PASS";
                btn_result.BackColor = Color.Green;
                Console.Beep(2766, 100);
                Thread.Sleep(50);
                Console.Beep(2766, 100); 
            }));
            return true;
        }

        private void call_fail(int code)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {   
                Console.Beep(3766, 500);
                lb_bin.Text = code.ToString();
                btn_result.BackColor = Color.Red;
                btn_result.Text = "Fail";
                if (code != -2&& code !=19)
                {
                    log_info();
                }
                con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                Console.WriteLine("call fail! " + code.ToString() + " stage=" + ssm_state);
                if (thread != null)
                {
                    ram_counter_bad++;
                    this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                    {
                        lb_pf.Text = ram_counter_good.ToString() + "/" + ram_counter_bad.ToString();
                    }));
                }
                btn_start.BackColor = Color.White;
                thread = null;
                working = 0;
                ui_tr.Stop();
            }));
            Thread.Sleep(1500);
        }
            private void set_process(int stage,int code)
        {
            ssm_state = stage;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                if (stage==def.stage_start)
                {
                    btn_tip_powerup.BackColor = Color.White;
                    btn_tip_download.BackColor = Color.White;
                    btn_tip_restart.BackColor = Color.White;
                    btn_tip_calibrate.BackColor = Color.White;
                    btn_tip_press.BackColor = Color.White;
                    btn_tip_result.BackColor = Color.White;
                    tb_log.Text = "Start Testing\r\n";
                    return;
                }
                if(stage==def.stage_power_up)
                {
                    if(code==def.RTN_OK)
                    {
                        tb_log.Text += "Power Up test Pass\r\n";
                        btn_tip_powerup.BackColor = Color.Green;
                    }    
                    else
                    {
                        tb_log.Text += "Power Up test fail\r\n";
                        btn_tip_powerup.BackColor = Color.Red;
                    }
                        
                }
                if (stage == def.stage_download)
                {
                    if (code == def.RTN_OK)
                    {
                        tb_log.Text += "Download ok\r\n";
                        btn_tip_download.BackColor = Color.Green;
                    }
                    else
                    {
                        tb_log.Text += "Download fail\r\n";
                        btn_tip_download.BackColor = Color.Red;
                    }
                }
                if (stage == def.stage_restart)
                {
                    if (code == def.RTN_OK)
                    {
                        tb_log.Text += "Restart ok\r\n";
                        btn_tip_restart.BackColor = Color.Green;
                    }
                    else
                    {
                        tb_log.Text += "Restart fail\r\n";
                        btn_tip_restart.BackColor = Color.Red;
                    }
                }
                if (stage == def.stage_calibrate)
                {
                    if (code == def.RTN_OK)
                    {
                        tb_log.Text += "calibrate ok\r\n";
                        btn_tip_calibrate.BackColor = Color.Green;
                    }
                    else
                    {
                        tb_log.Text += "calibrate fail\r\n";
                        btn_tip_calibrate.BackColor = Color.Red;
                    }
                }
                if (stage == def.stage_press)
                {
                    if (code == def.RTN_OK)
                    {
                        tb_log.Text += "press ok\r\n";
                        btn_tip_press.BackColor = Color.Green;
                    }
                    else
                    {
                        tb_log.Text += "press fail\r\n";
                        btn_tip_press.BackColor = Color.Red;
                    }
                }
                if (stage == def.stage_result)
                {
                    if (code == def.RTN_OK)
                    {
                        tb_log.Text += "test ok\r\n";
                        btn_tip_result.BackColor = Color.Green;
                    }
                    else
                    {
                        tb_log.Text += "test fail\r\n";
                        btn_tip_result.BackColor = Color.Red;
                    }
                }
            }));
        }

        void load_config()
        {
            string config_path = Application.StartupPath + @"\config.csv";
            Console.WriteLine("config_path !"+ config_path);
            FileStream config_fs = new FileStream(config_path, FileMode.Open, FileAccess.Read);
            csv_log.csv2dt(config_fs, 0, dt_config);
            config.project_name = dt_config.Rows[0]["Project"].ToString();
            config.product_code = dt_config.Rows[0]["Product"].ToString();
            config.sensor_width = Convert.ToInt32(dt_config.Rows[0]["sensor_width"]);
            config.sensor_height = Convert.ToInt32(dt_config.Rows[0]["sensor_height"]);
            config.avg_th = Convert.ToInt32(dt_config.Rows[0]["avg_th"]);
            config.rv_th = Convert.ToInt32(dt_config.Rows[0]["rv_th"]);
            config.c_th_high = Convert.ToInt32(dt_config.Rows[0]["c_th_high"]);
            config.c_th_low = Convert.ToInt32(dt_config.Rows[0]["c_th_low"]);
            config.v_th = Convert.ToInt32(dt_config.Rows[0]["v_th"]);
            config.version = dt_config.Rows[0]["Version"].ToString();
            config.station = Convert.ToInt32(dt_config.Rows[0]["Station"]);
            config.keycode = dt_config.Rows[0]["Keycode"].ToString();
            config.channel = Convert.ToInt32(dt_config.Rows[0]["Channel"]);
            config.auto_start = Convert.ToInt32(dt_config.Rows[0]["auto_start"]);
            if (config.keycode == "3230")
            {
                config.firmware_type = def.COSTYPE_USB_MOH_F323;
                config.dev_type = def.DEVTYPE_USB_MOH_F323;
            }    
            else
            {
                config.firmware_type = def.COSTYPE_USB_MOCH_MOH_BLD;
                config.dev_type = def.DEVTYPE_USB_MOCH_MOH_BLD;
            }

            config.comport = dt_config.Rows[0]["comport"].ToString();
            config_fs.Close();
            Console.WriteLine("config loaded !");
        }
        void open_create_log()
        {
            FileStream log_fs;
            //if csv not exsits, creat. if exsit, load it
            log_path = Application.StartupPath + @"\log-" + config.project_name + "-" + config.product_code + "-" + config.station.ToString() + ".csv";
            Console.WriteLine("log_path! "+ log_path);
            try
            {
                log_fs = new FileStream(log_path, FileMode.Open, FileAccess.Read);
                csv_log.csv2dt(log_fs, 0, dt);
                Console.WriteLine("Log loaded ");

            }
            catch (Exception err)
            {
                log_fs = new FileStream(log_path, FileMode.OpenOrCreate, FileAccess.Write);
                csv_log.gen_table(log_path, dt);
                csv_log.dt2csv(log_fs, dt);
                Console.WriteLine("Log created ");

            }
            log_fs.Close();
        }
        void log_info()
        {
            //return;

            FileStream log_fs;
            log_fs = new FileStream(log_path, FileMode.Open, FileAccess.Write);
            int dt_count = dt.Rows.Count;
            int max_id = 0;
            for(int i=0;i<dt_count;i++)
            {
                if (max_id < Convert.ToInt32(dt.Rows[i]["ID"]))
                    max_id = Convert.ToInt32(dt.Rows[i]["ID"]);
            }
            DataRow dr = dt.NewRow();
            dr["ID"] = max_id + 1;
            dr["Project"] = lb_project.Text;
            dr["Product"] = lb_product.Text;
            dr["Station"] = lb_station.Text;
            dr["Date"] = DateTime.Now.ToString("F");
            dr["Remains"] = Convert.ToInt32(lb_count.Text);
            dr["Current"] = Convert.ToInt32(lb_current.Text);
            //dr["Voltage"] = Convert.ToFloat(lb_voltage.Text);
            float v = 0;
            if (float.TryParse(lb_voltage.Text, out v))
                dr["Voltage"] = v;
            else
                dr["Voltage"] = 0.0;

            if (ssm_state >= def.stage_download)
                dr["Download"] = 1;
            else
                dr["Download"] = 0;

            if (ssm_state >= def.stage_restart)
                dr["Activate"] = 1;
            else
                dr["Activate"] = 0;
            dr["SN"] = lb_sn.Text;
            dr["Version"] = lb_version.Text;
            dr["Parameter"] = lb_parameter.Text;
            dr["Gray Level"] = Convert.ToInt32(lb_graylevel.Text);
            dr["RV"] = Convert.ToInt32(lb_rv.Text);
            dr["noise"] = float.Parse(lb_noise.Text);
            dr["snr"] = float.Parse(lb_snr.Text);
            dr["dr"] = float.Parse(lb_dr.Text);
            dr["RV"] = Convert.ToInt32(lb_rv.Text);
            dr["RV"] = Convert.ToInt32(lb_rv.Text);
            dr["Stage"] = ssm_state;
            if (ssm_state == def.stage_result_ok)
                dr["Result"] = "PASS";//good
            else
                dr["Result"] = "FAIL";//not finished
            
            dr["BIN"] = Convert.ToInt32(lb_bin.Text);
            dt.Rows.Add(dr);
            csv_log.dt2csv(log_fs, dt);
            log_fs.Close();
            Console.WriteLine("Log stored ");
            //char[] line = dt.Rows[max_id].ToString().ToCharArray();
            string strline = "";
            for (int j = 0; j < dt.Columns.Count; j++)
                strline += Convert.ToString(dt.Rows[max_id][j]);
            char[] line = strline.ToCharArray();
            device.write_flash(line,0,line.Length);
            Console.WriteLine("Log in module stored ");
        }

        private void btn_live_Click(object sender, EventArgs e)
        {
            preview_noise = 0;
            Console.WriteLine("BTN start clicked ");
            if (thread_live != null)
            {
                Console.WriteLine("thread_live != null ");
                return;
            }
               
            live_state = 1;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(1500);
            device.disconnect();
            int rtn = device.connect(config.firmware_type);
            rtn = device.get_bg(bkg_img);
            thread_live = new Thread(threadLive);
            thread_live.IsBackground = true;
            thread_live.Start();
            Console.WriteLine("Working thread started ");

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("BTN stop clicked ");
            live_state = 0;
            Thread.Sleep(100);
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            thread_live = null;        
        }

        private void btn_pressagain_Click(object sender, EventArgs e)
        {
            if (thread_press != null)
                return;
            set_process(def.stage_start, def.RTN_OK);
            init_tips();
            init_lbs();
            img_preview.Image = null;
            

            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(3000);

            set_process(def.stage_power_up, def.RTN_OK);
            set_process(def.stage_download, def.RTN_OK);
            set_process(def.stage_restart, def.RTN_OK);
            set_process(def.stage_calibrate, def.RTN_OK);

            device.disconnect();
            int rtn = device.connect(config.firmware_type);
            if(rtn==def.RTN_FAIL)
            {
                Console.WriteLine("thread press connect failed");
                set_process(def.stage_press, def.RTN_FAIL);
                call_fail(def.BIN_CODE_01);
                thread_press = null;
                return;
            }

            rtn = device.calibrate();
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread calibrate after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_04);
                thread_press = null;
                return;
            }

            rtn = device.get_bg(bkg_img);
            if (rtn == def.RTN_FAIL)
            {
                Console.WriteLine("thread press get_bg failed");
                set_process(def.stage_press, def.RTN_FAIL);
                call_fail(def.BIN_CODE_01);
                thread_press = null;
                return;
            }

            thread_press = new Thread(threadPress);
            thread_press.IsBackground = true;
            thread_press.Start();
        }

        private void btn_poweron_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
        }

        private void btn_poweroff_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
        }

       
        private void btn_usb1_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
        }

        private void btn_usb0_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.USB, (int)control.MODE.DOWN);
        }

        private void btn_s0_1_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);
        }

        private void btn_s0_0_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.S0, (int)control.MODE.DOWN);
        }

        private void btn_led_1_Click(object sender, EventArgs e)
        {

        }

        private void btn_led_0_Click(object sender, EventArgs e)
        {

        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(1500);
            device.disconnect();
            int rtn = device.connect(config.firmware_type);
            if (rtn == def.RTN_FAIL)
            {
                Console.WriteLine(" btn_read_Click connect failed");
                return;
            }
            char[] buf = new char[129];
            int len = 128;
            rtn = device.read_flash(buf, 0, ref len);
            if (rtn == def.RTN_FAIL)
            {
                Console.WriteLine(" btn_read_Click read_flash failed");
                return;
            }
            string s = new string(buf);
            tb_read.Text = s;
            rtn = get_all_info(1);
            if (rtn == def.RTN_FAIL)
            {
                Console.WriteLine(" btn_read_Click get_all_info failed");
                return;
            }
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);

        }

        private void cb_enhance_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_enhance.Checked == true)
                tb_gain.Enabled = false;
            else
                tb_gain.Enabled = true;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tb_read.Text = "";
        }

        private void btn_get_cv_Click(object sender, EventArgs e)
        {
             get_all_info(0);
        }

        private void cb_mp_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_mp.Checked == true)
            {
                btn_start.Text = "Click Button to Start";
                btn_start.BackColor = Color.White;
            }  
            else
            {
                btn_start.Text = "Start and Click";
                btn_start.BackColor = Color.White;
                con.should_leave = 1;
                
            }
                
        }
    }
}
