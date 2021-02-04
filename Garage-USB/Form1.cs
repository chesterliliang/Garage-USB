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
        System.Timers.Timer ui_tr = new System.Timers.Timer();   
        ThreadStart threadStart = null;
        ThreadStart threadLive = null;
        ThreadStart threadMP = null;
        Thread thread = null;
        Thread thread_live = null;
        Thread thread_mp = null;
        control con;
        fang g;     
       void Thread_Init()
        {
            threadStart = new ThreadStart(do_prime);
            threadLive = new ThreadStart(do_preview);
            threadMP = new ThreadStart(do_mp);
        }
        void Init_Timer()
        {
            ui_tr.AutoReset = true;
            ui_tr.Elapsed += new ElapsedEventHandler(ui_tr_event);
            ui_tr.Interval = 200;
        }
        void Data_Init()
        {
            g = new fang(Application.StartupPath);
            int i = config.init_projects(g);
            config.load_config(g, i);
            config.init_station(g);
        }
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("App start up!");
            Data_Init();
            Thread_Init();
            Init_Timer();
            open_create_log();
            
           
        }
        void project_ui_init(int index)
        {
            int dft = 0;
            lb_project.Text = config.project_name;
            lb_product.Text = config.comm_type.ToString()+"-"+config.firmware_type.ToString()+"-"+config.dev_type.ToString();
            cmb_project.Items.Clear();
            cmb_project.Items.Add("Select Project");
            for (int i = 0; i < g.dt_configs.Rows.Count; i++)
            {
                cmb_project.Items.Add(g.dt_configs.Rows[i]["Project"].ToString());
                if (Convert.ToInt32(g.dt_configs.Rows[i]["Default"]) == 1)
                    dft = i;
            }
            if(index!=-1)
                cmb_project.SelectedIndex = index;
            else
                cmb_project.SelectedIndex = dft+1;
        }
        void station_ui_init()
        {
            lb_station.Text = config.station.ToString();
            lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
            btn_tip_blink.BackColor = Color.White;
            set_process(def.stage_start, def.RTN_OK);
            tb_com.Text = config.comport;
            btn_start.BackColor = Color.White;

        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            int i = config.init_projects(g);
            if(cmb_project.SelectedIndex!=0)
            {
                config.load_config(g,cmb_project.SelectedIndex - 1);
                project_ui_init(cmb_project.SelectedIndex);
            } 
            else
            {
                config.load_config(g,i);
                project_ui_init(i);
            }    
            config.init_station(g);
            
            station_ui_init();
            image_view_init();
            g.ram_counter_bad = 0;
            g.ram_counter_good = 0;

        }
        void image_view_init()
        {
            bmp_helper.image_view_init(img_preview);
            bmp_helper.init(g);
        }
        void control_init()
        {
            con = new control();
            if (config.single_module != 1)
            {
                if (con.open_port(config.comport) == def.RTN_OK)
                {
                    tb_com.BackColor = Color.Green;
                    con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                    con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
                    con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);
                }
                   
                else
                    tb_com.BackColor = Color.Red;
            }

        }

        void mp_handling()
        {
            if (config.auto_start == 1)
            {
                Console.WriteLine("auto started!");
                thread_mp = new Thread(threadMP);
                thread_mp.IsBackground = true;
                thread_mp.Start();
                cb_mp.Checked = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            control_init();
            project_ui_init(-1);
            station_ui_init();
            image_view_init();
            mp_handling();
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
                {
                    Console.WriteLine("Thread exists!");
                    return;
                }
                    
                thread = new Thread(threadStart);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                if (thread_mp != null)
                {
                    Console.WriteLine("Thread_mp exists!");
                    return;
                }
                    

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
                if (g.working == 1)
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

                /*con.stop_pull_button();
                Console.WriteLine("stop_pull_button!");
                rtn = con.check_button_short();
                Console.WriteLine("check_button_short!");
                if (rtn == def.RTN_FAIL)
                {
                    Console.WriteLine("Button Short!");
                    call_fail(def.BIN_CODE_12);
                    return;
                }*/
                Console.WriteLine("Start do prime!");
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
            g.button_down = 0;
            g.working = 1;
            set_process(def.stage_start, def.RTN_OK);
            init_tips();
            init_lbs();
            img_preview.Image = null;

            if (config.test_only == 1)
                goto ACTIVE;

            if (config.test_only == 2)
                goto TEST;

            g.event_device_changed = 0;
            Console.WriteLine("Power up!");
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
            while (g.event_device_changed<2)
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
            rtn = device.download_firmware(g.firmware_path);
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


            g.event_device_changed = 0;
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
                if (config.c_check == 1)
                {
                    if (cv2[1] < config.c_th_low)
                    {
                        Console.WriteLine("Current below limited!");
                        call_fail(def.BIN_CODE_F2);
                        return;
                    }
                    if (cv2[1] > config.c_th_high)//<30
                    {
                        Console.WriteLine("Current above limited!");
                        call_fail(def.BIN_CODE_F4);
                        return;
                    }
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
            while (g.event_device_changed < 2)
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

            //goto TEST;
            rtn = device.disconnect();
            rtn = device.connect(config.firmware_type);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread connect after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_01);
                thread = null;
                return;
            }
            rtn = device.disconnect();


            rtn = device.set_sn_activiate(device.gen_sn(config.station,config.keycode),config.dev_type);
            /*if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread set_sn_activiate after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_10);
                thread = null;
                return;
            }*/


        TEST:
            rtn = device.disconnect();
            rtn = device.connect(config.firmware_type);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread connect after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_01);
                thread = null;
                return;
            }

            byte[] info = new byte[64];
            int len_info = 64;
            rtn = device.devinfo(info, ref len_info);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread devinfo after restart failed");
                set_process(def.stage_calibrate, def.RTN_FAIL);
                call_fail(def.BIN_CODE_02);
                thread = null;
                return;
            }
            string strsn = "";
            if (config.keycode != "7060")
                strsn = System.Text.Encoding.UTF8.GetString(info.Skip(6).Take(16).ToArray());
            else
                strsn = System.Text.Encoding.UTF8.GetString(info.Take(16).ToArray());

            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_sn.Text = strsn;
            }));

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
            string strversion = "";
            if (config.keycode!="7060")
                 strversion = System.Text.Encoding.UTF8.GetString(version);
            else
                strversion = device.bytesToHexString(version, 4);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_version.Text = strversion;
            }));

           
            if(config.simple_test!=1)
            {
                if (cb_calibrate.Checked == true || need_calirate == 1)
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


                byte[] list = new byte[64];
                int config_len = 0;
                rtn = device.config(0, list, ref config_len);
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
                rtn = device.get_bg(g.bkg_img);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("thread get_bg after restart failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_05);
                    return;
                }


                rtn = check_noise();
                if (rtn != def.RTN_OK)
                {
                    return;
                }
            }

            set_process(def.stage_calibrate, def.RTN_OK);
            Console.Beep(2766, 200);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                img_preview.Image = null;
                //btn_tip_press.BackColor = Color.Blue;
                btn_tip_blink.BackColor = Color.Blue;
            }));
            //restart and calbrate
            //======================================================//

            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                btn_result.Text = "拉-橡膠手柄-確保到位";
            }));
            g.press_counter = 80;
            ui_tr.Start();
            byte[] frame_data = new byte[4*config.sensor_width * config.sensor_height];

            if (config.simple_test == 0)
            {
                //preview image
                while (g.press_counter > 0)
                {
                    rtn = preview_image(frame_data);
                    if (rtn != device.ERR_OK)
                    {
                        Console.WriteLine("capture_frame failed");
                        set_process(def.stage_calibrate, def.RTN_FAIL);
                        call_fail(def.BIN_CODE_06);
                        return;
                    }
                    g.press_counter--;
                }
                if (g.button_down == 0&&config.btn_check==1)
                {
                    Console.WriteLine("button down check failed");
                    set_process(def.stage_press, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_16);
                    return;
                }
            }
            else
            {
                int frame_len = 4 * config.sensor_width * config.sensor_height;
                rtn = device.capture_frame(1, frame_data,ref frame_len);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("capture_frame failed");
                    set_process(def.stage_press, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_06);
                    return;
                }
            }
            if(config.simple_test==0)
            {
                if (average_check(frame_data) == def.RTN_FAIL)
                {
                    g.ram_counter_bad++;
                    this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                    {
                        lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
                    }));
                    Thread.Sleep(500);
                    btn_start.BackColor = Color.White;
                    thread = null;
                    g.working = 0;
                    device.disconnect();
                    return;
                }
            }
            else
            {
                if(inline_check(frame_data)==def.RTN_FAIL)
                {
                    g.ram_counter_bad++;
                    this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                    {
                        lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
                    }));
                    Thread.Sleep(500);
                    btn_start.BackColor = Color.White;
                    thread = null;
                    g.working = 0;
                    device.disconnect();
                    return;
                }
            }
            

            if (check_result())
            {
                set_process(def.stage_result, def.RTN_OK);
                set_process(def.stage_result_ok, def.RTN_OK);
                log_info();
                if (config.single_module == 0)
                {
                    con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                }
            }
            g.ram_counter_good++;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
            }));
            Thread.Sleep(500);
            btn_start.BackColor = Color.White;
            device.disconnect();
            thread = null;
            g.working = 0;
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
                int frame_len = 4 * config.sensor_width * config.sensor_height;
                rtn = device.capture_frame(0, empty_frame,ref frame_len);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 emptet capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_07);
                    return def.RTN_FAIL;
                }
                fpimage.bmp_frame(empty_frame, g.bkg_img);
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
            byte[] list = new byte[64];
            int config_len = 0;
            rtn = device.config(0, list,ref config_len);
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
            if (g.preview_noise == 0) 
            {
                rtn = check_noise();
                if (rtn != def.RTN_OK)
                {
                    Console.WriteLine("preview_image check noise failed");
                    return def.RTN_FAIL; ;
                }
                g.preview_noise = 1;
            }

            int frame_len = 4 * config.sensor_width * config.sensor_height;
            rtn = device.capture_frame(0, frame_data, ref frame_len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("capture_frame failed");
                return def.RTN_FAIL;
            }
            fpimage.bmp_frame(frame_data, g.bkg_img);
            

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
            if(config.btn_check==1)
            {
                if (g.button_down == 0)
                {
                    con.stop_pull_button();
                    con.check_button_short();
                    rtn = con.wait_button(2);
                    if (rtn == def.RTN_OK)
                    {
                        g.button_down = 1;
                    }
                    else
                    {
                       g.button_down = 0;
                    }
                }
            }

            return def.RTN_OK;
        }

        public int inline_check(byte[] avg_frame)
        {
            int[] para = fpimage.get_otsu(avg_frame);
            int rv = para[3] - para[2];
            int avg = 0;
            Image avg_img;
            if (cb_enhance.Checked)
            {
                int sw_gain = Convert.ToInt32(tb_gain.Text);
                avg_img = bmp_helper.format_from_bytes(avg_frame, sw_gain);
                fpimage.gain_frame(avg_frame, avg_frame, sw_gain);   
            }
            else
                avg_img = bmp_helper.format_from_bytes(avg_frame, 0);

            avg = fpimage.average_frame(avg_frame);

            int signal = 255 - para[2];
            double f_noise = float.Parse(lb_noise.Text);
            if (config.simple_test == 1)
                f_noise = 1.5;
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
                int frame_len = 4 * config.sensor_width * config.sensor_height;
                rtn = device.capture_frame(0, frame_data, ref frame_len);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 capture_frame failed");
                    set_process(def.stage_calibrate, def.RTN_FAIL);
                    call_fail(def.BIN_CODE_07);
                    return def.RTN_FAIL;
                }
                fpimage.bmp_frame(frame_data, g.bkg_img);
                Buffer.BlockCopy(frame_data, 0, frame10, i * config.sensor_width * config.sensor_height, config.sensor_width * config.sensor_height);
            }
            avg = fpimage.merge_frames(frame10, 10, avg_frame);
            ui_tr.Stop();
            // do report
            rtn = inline_check(avg_frame);
            if (rtn != def.RTN_OK)
            {
                return rtn;
            }
            set_process(def.stage_press, def.RTN_OK);
            return def.RTN_OK;
        }

        public void do_preview()
        {
            byte[] image = new byte[config.sensor_width * config.sensor_height];
            while(true)
            {
                if (g.live_state == 0)
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
        private void ui_tr_event(object source, System.Timers.ElapsedEventArgs e)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                if (g.ssm_state == def.stage_calibrate)
                {
                    if (btn_tip_blink.BackColor == Color.White)
                    {
                        if (g.button_down == 0)
                            btn_tip_blink.BackColor = Color.Blue;
                        else
                            btn_tip_blink.BackColor = Color.Green;
                    }
                    else if (btn_tip_blink.BackColor == Color.Blue)
                    {
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
                            g.event_device_changed += 1;
                            Console.WriteLine("device changed! "+ g.event_device_changed.ToString());
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
                if(config.single_module!=1)
                    con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                Console.WriteLine("call fail! " + code.ToString() + " stage=" + g.ssm_state);
                if (thread != null)
                {
                    g.ram_counter_bad++;
                    this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                    {
                        lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
                    }));
                }
                btn_start.BackColor = Color.White;
                thread = null;
                g.working = 0;
                ui_tr.Stop();
            }));
            Thread.Sleep(1500);
        }
            private void set_process(int stage,int code)
        {
            g.ssm_state = stage;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                pb_process.Value = stage;
               /* if (stage==def.stage_start)
                {
                    pb_process.Value = 1;
                    return;
                }
                if(stage==def.stage_power_up)
                {
                    pb_process.Value = 17;
                        
                }
                if (stage == def.stage_download)
                {
                    pb_process.Value = 35;
                }
                if (stage == def.stage_restart)
                {
                    pb_process.Value = 53;
                }
                if (stage == def.stage_calibrate)
                {
                    pb_process.Value = 70;
                }
                if (stage == def.stage_press)
                {
                    pb_process.Value = 85;
                }
                if (stage == def.stage_result)
                {
                    pb_process.Value = 100;
                }*/
            }));
        }

       
        void open_create_log()
        {
            FileStream log_fs;
            //if csv not exsits, creat. if exsit, load it
            g.log_path = Application.StartupPath + @"\log-" + config.project_name + "-" + config.product_code + "-" + config.station.ToString() + ".csv";
            Console.WriteLine("log_path! "+ g.log_path);
            try
            {
                log_fs = new FileStream(g.log_path, FileMode.Open, FileAccess.Read);
                csv_log.csv2dt(log_fs, 0, g.dt);
                Console.WriteLine("Log loaded ");

            }
            catch (Exception err)
            {
                log_fs = new FileStream(g.log_path, FileMode.OpenOrCreate, FileAccess.Write);
                csv_log.gen_table(g.log_path, g.dt);
                csv_log.dt2csv(log_fs, g.dt);
                Console.WriteLine("Log created ");

            }
            log_fs.Close();
            
        }
        void log_info()
        {
            //return;

            FileStream log_fs;
            log_fs = new FileStream(g.log_path, FileMode.Open, FileAccess.Write);
            int dt_count = g.dt.Rows.Count;
            int max_id = 0;
            for(int i=0;i<dt_count;i++)
            {
                if (max_id < Convert.ToInt32(g.dt.Rows[i]["ID"]))
                    max_id = Convert.ToInt32(g.dt.Rows[i]["ID"]);
            }
            DataRow dr = g.dt.NewRow();
            dr["ID"] = max_id + 1;
            dr["Project"] = lb_project.Text;
            dr["Product"] = lb_product.Text;
            dr["Station"] = lb_station.Text;
            dr["Date"] = DateTime.Now.ToString("F");
            dr["Current"] = Convert.ToInt32(lb_current.Text);
            //dr["Voltage"] = Convert.ToFloat(lb_voltage.Text);
            float v = 0;
            if (float.TryParse(lb_voltage.Text, out v))
                dr["Voltage"] = v;
            else
                dr["Voltage"] = 0.0;

            if (g.ssm_state >= def.stage_download)
                dr["Download"] = 1;
            else
                dr["Download"] = 0;

            if (g.ssm_state >= def.stage_restart)
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
            dr["Stage"] = g.ssm_state;
            if (g.ssm_state == def.stage_result_ok)
                dr["Result"] = "PASS";//good
            else
                dr["Result"] = "FAIL";//not finished
            
            dr["BIN"] = Convert.ToInt32(lb_bin.Text);
            g.dt.Rows.Add(dr);
            csv_log.dt2csv(log_fs, g.dt);
            log_fs.Close();
            Console.WriteLine("Log stored ");

            if (config.simple_test == 1 && config.simple_test == 0)//close it for now
            {
                string strline = "";
                for (int j = 0; j < g.dt.Columns.Count; j++)
                    strline += Convert.ToString(g.dt.Rows[max_id][j]);
                char[] line = strline.ToCharArray();
                device.write_flash(line, 0, line.Length);
                Console.WriteLine("Log in module stored ");
            }
            
        }

        private void btn_live_Click(object sender, EventArgs e)
        {
            g.preview_noise = 0;
            Console.WriteLine("BTN start clicked ");
            if (thread_live != null)
            {
                Console.WriteLine("thread_live != null ");
                return;
            }
               
            g.live_state = 1;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(1500);
            device.disconnect();
            int rtn = device.connect(config.firmware_type);
            rtn = device.get_bg(g.bkg_img);
            thread_live = new Thread(threadLive);
            thread_live.IsBackground = true;
            thread_live.Start();
            Console.WriteLine("Working thread started ");

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("BTN stop clicked ");
            g.live_state = 0;
            Thread.Sleep(100);
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            thread_live = null;        
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
