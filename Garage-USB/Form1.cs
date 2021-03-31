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
        public System.Timers.Timer ui_tr = new System.Timers.Timer();
        public ThreadStart threadStart = null;
        public ThreadStart threadLive = null;
        public ThreadStart threadMP = null;
        public Thread thread = null;
        public Thread thread_live = null;
        public Thread thread_mp = null;
        public Label[] label_list = new Label[def.lable_count];
        public fang g;
        Enkidu ek;



        void Thread_Init()
        {
            threadStart = new ThreadStart(th_prime);
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
            g.BIN = new err();
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
            csv_log.open_create_log(g);

        }
        void project_ui_init(int index)
        {
            int dft = 0;
            lb_project.Text = config.project_name;
            lb_product.Text = config.comm_type.ToString() + "-" + config.firmware_type.ToString() + "-" + config.dev_type.ToString();
            cmb_project.Items.Clear();
            cmb_project.Items.Add("Select Project");
            for (int i = 0; i < g.dt_configs.Rows.Count; i++)
            {
                cmb_project.Items.Add(g.dt_configs.Rows[i]["Project"].ToString());
                if (Convert.ToInt32(g.dt_configs.Rows[i]["Default"]) == 1)
                    dft = i;
            }
            if (index != -1)
                cmb_project.SelectedIndex = index;
            else
                cmb_project.SelectedIndex = dft + 1;

            if (config.locked == 1)
            {
                cmb_project.Enabled = false;
                btn_reload.Visible = false;
                cmb_channel.Visible = false;
            }


        }
        void station_ui_init()
        {
            lb_station.Text = config.station.ToString();
            lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
            btn_tip_blink.BackColor = Color.White;
            set_process(def.stage_start);
            tb_com.Text = config.comport;
            tb_working.Text = config.working_port;
            btn_start.BackColor = Color.White;
            lb_err_message.Text = "";
            if (config.test_only == 3)
                lb_station_type.Text = "CHECK";
            else
                lb_station_type.Text = "MP";

        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            int i = config.init_projects(g);
            if (cmb_project.SelectedIndex != 0)
            {
                config.load_config(g, cmb_project.SelectedIndex - 1);
                project_ui_init(cmb_project.SelectedIndex);
            }
            else
            {
                config.load_config(g, i);
                project_ui_init(i);
            }
            config.init_station(g);

            station_ui_init();
            image_view_init();
            g.ram_counter_bad = 0;
            g.ram_counter_good = 0;
            csv_log.open_create_log(g);

        }
        void image_view_init()
        {
            bmp_helper.image_view_init(img_preview);
            bmp_helper.init(g);
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
                btn_start.Text = "推-開始按鈕";
            }
        }

        void init_ui_list()
        {

            label_list[def.LABLE_PROJECT] = lb_project; label_list[def.LABLE_PRODUCT] = lb_product; label_list[def.LABLE_SN] = lb_sn; label_list[def.LABLE_VOLTAGE] = lb_voltage;
            label_list[def.LABLE_CURRENT] = lb_current; label_list[def.LABLE_PARAMETER] = lb_parameter; label_list[def.LABLE_VERSION] = lb_version; label_list[def.LABLE_RV] = lb_rv;
            label_list[def.LABLE_GRAYLEVEL] = lb_graylevel; label_list[def.LABLE_NOISE] = lb_noise; label_list[def.LABLE_BIN] = lb_bin; label_list[def.LABLE_STATION] = lb_station;
            g.init_ui_list(label_list);
            g.btn_result = btn_result;
            g.tb_com = tb_com;
            g.tb_working = tb_working;
            g.pb_process = pb_process;
            g.img_preview = img_preview;
            g.btn_tip_blink = btn_tip_blink;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init_ui_list();
            g.control_init();
            project_ui_init(-1);
            station_ui_init();
            //for debug
            cmb_channel.Items.Add("1");
            cmb_channel.Items.Add("2");
            cmb_channel.Items.Add("3");

            image_view_init();
            mp_handling();
            g.working_com_init();
            ek = new Enkidu(this);

        }
        public void init_lbs()
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                g.clear_ui_list();
                lb_project.Text = config.project_name;
                lb_product.Text = config.product_code;
                lb_err_message.Text = "";
            }));
        }
        public void init_tips()
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
            if (cb_mp.Checked == false)
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
                btn_start.Text = "推-開始按鈕";
                thread_mp = new Thread(threadMP);
                thread_mp.IsBackground = true;
                thread_mp.Start();
            }
        }
        public void do_mp()
        {
            int rtn = 0;
            g.con.should_leave = 0;
            g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            g.con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
            g.con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);

            while (cb_mp.Checked == true)
            {
                this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                {
                    btn_start.BackColor = Color.Purple;
                }));
                if (g.working == 1)
                    continue;

                rtn = g.con.wait_start_key(-1);
                if (rtn == def.RTN_FAIL)
                {
                    call_fail(g.BIN.BIN_CODE[15]);
                    return;
                }
                set_process(def.stage_start);
                init_tips();
                init_lbs();
                img_preview.Image = null;

                Console.WriteLine("Start do prime!");
                rtn = do_prime();
            }
            thread_mp = null;
        }

        int prime_dispatch(int func, int p1)
        {
            int rtn = def.RTN_FAIL;
            g.await = 1;
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                switch (func)
                {
                    case def.FUNC_POWER_UP:
                        rtn = g.prime_power_up(p1);
                        break;
                    case def.FUNC_DOWNLOAD:
                        rtn = g.prime_download();
                        break;
                    case def.FUNC_ACTIVE:
                        rtn = g.prime_active();
                        break;
                    case def.FUNC_GETINFO:
                        rtn = g.prime_test_getinfo();
                        break;
                    case def.FUNC_CALIBRATE:
                        rtn = g.prime_calibrate(p1);
                        break;
                    case def.FUNC_NOISE:
                        rtn = g.prime_check_noise(p1);
                        break;
                    case def.FUNC_PREVIEW:
                        rtn = g.preview_image(p1);
                        break;
                    case def.FUNC_BUTTON:
                        rtn = g.prime_check_button(p1);
                        break;
                    case def.FUNC_FRAME:
                        rtn = g.prime_get_average_frames(p1);
                        break;
                    case def.FUNC_SINGAL:
                        rtn = g.prime_signal(p1);
                        break;
                    case def.FUNC_FINAL:
                        rtn = g.prime_final();
                        break;
                    case def.FUNC_POWER_DOWN:
                        rtn = g.prime_power_down();
                        break;
                    default:
                        break;
                }
            }));
            while (g.await == 1)
            {
                Thread.Sleep(10);
            }
            if (rtn != def.RTN_OK)
                call_fail(g.BIN.BIN_CODE[rtn]);
            return rtn;
        }
        public void init_prime_work()
        {
            g.button_down = 0;
            g.working = 1;
            g.active_opt_done = 0;
            g.download_opt_done = 0;
            set_process(def.stage_start);
            init_tips();
            init_lbs();
            img_preview.Image = null;
            g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.DOWN);
        }

        public void final_prime_work()
        {
            set_process(def.stage_result);
            set_process(def.stage_result_ok);
            prime_dispatch(def.FUNC_FINAL, 0);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
                btn_start.BackColor = Color.White;
            }));
            thread = null;
        }

        public void set_process(int stage)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                g.set_process(stage);
            }));
        }

        public void th_prime()
        {
            do_prime();
        }

        public int do_prime()
        {
            int rtn = def.RTN_FAIL;
            //int download_calibrate = 0;

            if (config.keycode == "2887"|| config.keycode == "2888")
            {
                ek.work();
                thread = null;
                return def.RTN_OK;
            }

            init_prime_work();

            rtn = prime_dispatch(def.FUNC_POWER_UP, def.FIRST_POWER_UP);
            if (rtn != def.RTN_OK)
                return rtn;
            if(config.comm_type==0)//USB
            {
                if(config.long_wait==1)
                    Thread.Sleep(3000);
                g.prime_wait_device(2);
            }
            else
            {
                Thread.Sleep(500);
            }

            if(config.led==1)
            {
                g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.UP);
            }

            if (config.test_only == 1)
                goto ACTIVE;
            else if (config.test_only == 2)
                goto TEST;

            rtn = g.prime_detect();
            if (rtn == def.ACTIVE)
                goto ACTIVE;
            else if (rtn == def.TEST)
                goto TEST;

            if (config.test_only == 3)//check no download
            {
                call_fail(g.BIN.BIN_CODE[1]);
                return g.BIN.BIN_CODE[1];
            }

            rtn = prime_dispatch(def.FUNC_DOWNLOAD, 0);
            if (rtn != def.RTN_OK)
            {
                return rtn;
            }
            rtn = prime_dispatch(def.FUNC_POWER_DOWN, 0);
            if (rtn != def.RTN_OK)
                return rtn;
            Thread.Sleep(3000);// powerup need time to calirate otherwise current will go high
            //download_calibrate = 1;
            rtn = prime_dispatch(def.FUNC_POWER_UP, def.SECOND_POWER_UP);
            if (rtn != def.RTN_OK)
            {
                return rtn;
            }

            g.prime_wait_device(3);

        ACTIVE:
            if (config.test_only == 3)
            {
                call_fail(g.BIN.BIN_CODE[2]);
                return g.BIN.BIN_CODE[2];
            }
            rtn = prime_dispatch(def.FUNC_ACTIVE, 0);
            if (rtn != def.RTN_OK)
            {
                return rtn;
            }
        TEST:
            rtn = prime_dispatch(def.FUNC_GETINFO,0);
            if (rtn != def.RTN_OK)
            {
                return rtn;
            }
            if (config.test_only == 3)
            {
                final_prime_work();
                return def.RTN_OK;
            }


            if (config.simple_test==0)
            {

                if (cb_calibrate.Checked == true)//&& download_calibrate == 0)
                    rtn = prime_dispatch(def.FUNC_CALIBRATE, 1);
                else
                    rtn = prime_dispatch(def.FUNC_CALIBRATE, 0);
                if (rtn != def.RTN_OK)
                        return rtn;

                if (cb_ns.Checked)
                {
                    rtn = prime_dispatch(def.FUNC_NOISE, 0);//SHOULD EMPTY IMAGE FOR BTL MODULE?
                    if (rtn != def.RTN_OK)
                        return rtn;
                }
            }
            set_process(def.stage_calibrate);

            Console.Beep(2766, 200);
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                img_preview.Image = null;
                btn_tip_blink.BackColor = Color.Blue;
                btn_result.Text = "拉-橡膠手柄-確保到位";
            }));

            g.press_counter = config.counter;

            int gain = 0;
            if (cb_enhance.Checked)
                gain = Convert.ToInt32(tb_gain.Text);
            ui_tr.Start();

            g.button_down = 0;

            int btn_pull_started = 0;
            int btn_pull_stopped = 0;
            
            while (g.press_counter > 0)
            {
                if (config.simple_test == 0)
                {
                    rtn = prime_dispatch(def.FUNC_PREVIEW, gain);
                    if (rtn != def.RTN_OK)
                        goto LB_TR_END;
                }
                else
                    Thread.Sleep(25);

                if(config.btn_check == 1&& btn_pull_started==0)
                {
                    g.con.stop_pull_button();
                    g.con.check_button_short();
                    btn_pull_started = 1;
                }
                if(config.btn_check == 1 && g.button_down == 0)
                {
                    rtn = prime_dispatch(def.FUNC_BUTTON, 2);
                    if (rtn != def.RTN_OK)
                        goto LB_TR_END; ;
                }
                else if(config.btn_check == 1 && g.button_down == 1 && btn_pull_stopped == 0)
                {
                    g.con.stop_pull_button();
                    btn_pull_stopped = 1;
                }
                g.press_counter--;
            }// preview finish
            if (g.button_down == 0&&config.btn_check==1)
            {
                Console.WriteLine("button down check failed");
                set_process(def.stage_press);
                call_fail(g.BIN.BIN_CODE[17]);
                ui_tr.Stop();
                return g.BIN.BIN_CODE[17];
            }
            if(config.simple_test == 0)
                rtn = prime_dispatch(def.FUNC_FRAME, 10);
            else
                rtn = prime_dispatch(def.FUNC_FRAME, 1);
            if (rtn != def.RTN_OK)
                goto LB_TR_END;

            LB_TR_END:
            ui_tr.Stop();
            if (rtn != def.RTN_OK)
                return rtn;

            rtn = prime_dispatch(def.FUNC_SINGAL, gain);
            if (rtn != def.RTN_OK)
              return rtn;

            final_prime_work();
            return def.RTN_OK;
        }
      

        public int get_all_info(int mode)
        {
            return def.RTN_FAIL;
        }

        public void do_preview()
        {
            byte[] image = new byte[config.sensor_width * config.sensor_height];
           
            while (true)
            {
                if (g.live_state == 0)
                {
                    Console.WriteLine("do preview abort!");
                    thread_live = null; 
                    return;
                }
                int gain = 0;
                if (cb_enhance.Checked)
                    gain = Convert.ToInt32(tb_gain.Text);
                if (prime_dispatch(def.FUNC_PREVIEW, gain)!=def.RTN_OK)
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

        public void call_fail(int code)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                g.call_fail(code);
                lb_err_message.Text = g.BIN.message(code);
                g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.DOWN);
                if (thread != null)
                {
                    g.ram_counter_bad++;
                    lb_pf.Text = g.ram_counter_good.ToString() + "/" + g.ram_counter_bad.ToString();
                }
                btn_start.BackColor = Color.White;
                btn_tip_blink.BackColor = Color.Red;
                thread = null;
                g.working = 0;
                ui_tr.Stop();   
            }));
        }

        void log_info()
        {
            g.log_info();            
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
            g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(1500);
            device.disconnect();
            int rtn = device.connect(config.firmware_type, config.working_port);
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
            g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            thread_live = null;        
        }

        private void btn_poweron_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
        }

        private void btn_poweroff_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
        }

       
        private void btn_usb1_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
        }

        private void btn_usb0_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.USB, (int)control.MODE.DOWN);
        }

        private void btn_s0_1_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);
        }

        private void btn_s0_0_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.S0, (int)control.MODE.DOWN);
        }

        private void btn_led_1_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.UP);
        }

        private void btn_led_0_Click(object sender, EventArgs e)
        {
            g.con.switch_control((int)control.COMMAND.LED, (int)control.MODE.DOWN);
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
                btn_start.Text = "點擊開始 Start";
                btn_start.BackColor = Color.White;
            }  
            else
            {
                btn_start.Text = "Start and Click";
                btn_start.BackColor = Color.White;
                g.con.should_leave = 1;
                
            }
                
        }

        private void cmb_channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.channel = Convert.ToInt32(cmb_channel.SelectedItem.ToString());
            g.con.set_channel();
        }
    }
}
