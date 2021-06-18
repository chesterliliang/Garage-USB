using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Linq;

namespace Garage_USB
{
    public class fang
    {
        public byte[] bkg_img = null;
        public byte[] avg_frame = null;
        Form1 ui_form;

        public DataTable dt_configs;
        public DataTable dt_station;
        public DataTable dt = new DataTable();

        public string ref_path;
        public string configs_path;
        public string station_path;

        public int press_counter = 0;

        public int ssm_state = 0;
        public string log_path = "";
        public int ram_counter_good = 0;
        public int ram_counter_bad = 0;

        public int event_device_changed = 0;
        public string firmware_path = "";
        public int button_down = 0;
        public int preview_noise = 0;

        public int working = 0;
        public int live_state = 0;
        public Label[] label_list;
        public Button btn_result;
        public Button btn_tip_blink;
        public control con;
        public TextBox tb_com;
        public TextBox tb_working;

        public ProgressBar pb_process;
        public PictureBox img_preview;
        public int await = 0;

        public int active_opt_done = 0;
        public int download_opt_done = 0;

        public err BIN;
        public com c;

        public int control_init(string com_name)
        {
            int version = 0;
            con = new control();

            if (con.open_port(com_name) == def.RTN_OK)
            {
                //tb_com.BackColor = Color.Green;
                int rtn = con.get_vr(ref version);
                if (rtn == def.RTN_OK)
                {
                    con.close_port();
                    config.comport = com_name;
                    ui_form.BeginInvoke(new ThreadStart(delegate ()
                    {
                        ui_form.lb_vr.Text = version.ToString();
                    }));

                    return def.RTN_OK;
                }
                else
                {
                    con.close_port();
                    return def.RTN_FAIL;
                }
                    
            }
            return def.RTN_FAIL;


        }
        public void control_init()
        {
            con = new control();
            if (config.single_module != 1)
            {
                if (con.open_port(config.comport) == def.RTN_OK)
                {
                    tb_com.BackColor = Color.Green;
                    //con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                    //con.switch_control((int)control.COMMAND.USB, (int)control.MODE.UP);
                    //con.switch_control((int)control.COMMAND.S0, (int)control.MODE.UP);
                }

                else
                    tb_com.BackColor = Color.Red;
            }

        }

        public void working_com_init()
        {
            c = new com();
            if(config.comm_type==1)
            {
                if(c.open_port(config.working_port)==def.RTN_OK)
                {
                    tb_working.BackColor = Color.Green;
                }
                else
                {
                    tb_working.BackColor = Color.Red; 
                }
            }
        }

        public fang(string app_path, Form ui)
        {
            ui_form = (Form1)ui;
            ref_path = app_path; 
            csv_log.path = app_path + @"img\";
            Console.WriteLine("img storage path = " + csv_log.path);
            configs_path = app_path + @"files\configs.csv";
            Console.WriteLine("configs_path = " + configs_path);
            station_path = app_path + @"files\station.csv"; ;
            Console.WriteLine("station_path = " + station_path);
        }

        public void init_config()
        {
            dt_configs = new DataTable();
        }

        public void init_station()
        {
            dt_station = new DataTable();
        }

        public void init_ui_list(Label[] list)
        {
            label_list = new Label[def.lable_count];
            for (int i = 0; i < def.lable_count; i++)
                label_list[i] = list[i];
        }

        public void clear_ui_list()
        {
            for (int i = 0; i < def.lable_count; i++)
                label_list[i].Text = "-1";
        }
        public void set_process(int stage)
        {
            ssm_state = stage;
            pb_process.Value = stage;
        }

        public void log_info()
        {
            FileStream log_fs;
            log_fs = new FileStream(log_path, FileMode.Open, FileAccess.Write);
            int dt_count = dt.Rows.Count;
            int max_id = 0;
            for (int i = 0; i < dt_count; i++)
            {
                if (max_id < Convert.ToInt32(dt.Rows[i]["ID"].ToString()))
                    max_id = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
            }
            DataRow dr = dt.NewRow();
            dr["ID"] = (max_id + 1).ToString();
            dr["Project"] = label_list[def.LABLE_PROJECT].Text;
            dr["Product"] = label_list[def.LABLE_PRODUCT].Text;
            dr["Station"] = label_list[def.LABLE_STATION].Text;
            dr["Date"] = DateTime.Now.ToString("F");
            dr["Current"] = Convert.ToInt32(label_list[def.LABLE_CURRENT].Text);
            //dr["Voltage"] = Convert.ToFloat(lb_voltage.Text);
            /* float v = 0;
             if (float.TryParse(label_list[def.LABLE_VOLTAGE].Text, out v))
                 dr["Voltage"] = v;
             else
                 dr["Voltage"] = 0.0;*/
            dr["Voltage"] = label_list[def.LABLE_CURRENT].Text;

            if (ssm_state >= def.stage_download)
                dr["Download"] = "1";
            else
                dr["Download"] = "0";


            dr["Activate"] = "1";

            dr["SN"] = label_list[def.LABLE_SN].Text;
            dr["Version"] = label_list[def.LABLE_VERSION].Text;
            dr["Parameter"] = label_list[def.LABLE_PARAMETER].Text;
            dr["Gray Level"] = label_list[def.LABLE_GRAYLEVEL].Text;
            dr["RV"] = label_list[def.LABLE_RV].Text;
            dr["noise"] = label_list[def.LABLE_NOISE].Text;
            dr["Stage"] = ssm_state.ToString();
            if (ssm_state == def.stage_result_ok)
                dr["Result"] = "PASS";//good
            else
                dr["Result"] = "FAIL";//not finished

            dr["BIN"] = label_list[def.LABLE_BIN].Text;
            dt.Rows.Add(dr);
            csv_log.dt2csv(log_fs, dt);
            log_fs.Close();
            Console.WriteLine("Log stored ");

            /*if (config.simple_test == 1 && config.simple_test == 0)//close it for now
            {
                string strline = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                    strline += Convert.ToString(dt.Rows[max_id][j]);
                char[] line = strline.ToCharArray();
                device.write_flash(line, 0, line.Length);
                Console.WriteLine("Log in module stored ");
            }*/

        }

        public void call_fail(int code)
        {

                Console.Beep(3766, 500);
                label_list[def.LABLE_BIN].Text = code.ToString();
                btn_result.BackColor = Color.Red;
                btn_result.Text = "Fail";
                if (code != -2 && code != 19)
                {
                    log_info();
                }
                //if (config.single_module != 1)
                //    con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                Console.WriteLine("call fail! " + code.ToString() + " stage=" + ssm_state);
        }
        public int prime_power_up(int mode)
        {
            int rtn = 0;
            event_device_changed = 0;
            Console.WriteLine("Power up!");

            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.UP);
            Thread.Sleep(50);
            if(config.c_check == 1)
            {
                rtn = con.fn_check_current();
                if (rtn == def.RTN_FAIL)
                {
                    Console.WriteLine("Power up fn_check_current fail!");
                    rtn = BIN.BIN_CODE[3];
                    goto POWER_UP_END;
                }
           
                float[] cv = con.fn_get_cv(1,0);
                if (cv != null)
                {
                    label_list[def.LABLE_VOLTAGE].Text = cv[0].ToString();
                    label_list[def.LABLE_CURRENT].Text = cv[1].ToString();
                    if (cv[1] < 1) //pin connect fail
                    {
                        Console.WriteLine("Power up but no current!");
                        rtn = BIN.BIN_CODE[6];
                        goto POWER_UP_END;
                    }
                    if (cv[1] > 100)//<50
                    {
                        Console.WriteLine("too much current!");
                        rtn = BIN.BIN_CODE[3];
                        goto POWER_UP_END;
                    }
                    if (config.c_check == 1 && mode == def.SECOND_POWER_UP)
                    {
                        if (cv[1] < config.c_th_low)
                        {
                            Console.WriteLine("Current below limited!");
                            rtn = BIN.BIN_CODE[3];
                            goto POWER_UP_END;
                        }
                        if (cv[1] > config.c_th_high)
                        {
                            Console.WriteLine("Current above limited!");
                            rtn = BIN.BIN_CODE[3];
                            goto POWER_UP_END;
                        }
                    }
                }
                
            }
            rtn = def.RTN_OK;
            if (mode == def.FIRST_POWER_UP)
                set_process(def.stage_power_up);
            else if (mode == def.SECOND_POWER_UP)
                set_process(def.stage_restart);
            POWER_UP_END:
            await = 0;
            return rtn;
        }

        public int prime_power_down()
        {
            Console.WriteLine("enter prime_power_down");
            event_device_changed = 0;
            con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
            await = 0;
            return def.RTN_OK;
        }

        public int prime_wait_device(int time_out_s)
        {
            int wait_first_connect_counter = 0;
            while (event_device_changed < 2)
            {
                Thread.Sleep(1000);
                wait_first_connect_counter++;
                if (wait_first_connect_counter > time_out_s)
                {
                    Console.WriteLine("wait_first_connect_counter timeout");
                    break;
                }
            }
            Console.WriteLine("leave waiting device while!");
            return def.RTN_OK;
        }

        public int prime_detect()
        {
            Console.WriteLine("enter prime_detect");
            int rtn = device.disconnect();
            rtn = device.connect(config.firmware_type, config.working_port);
            if (rtn == device.ERR_OK)//to check if downloaded
            {
                rtn = device.abort();//check if activated
                if (rtn == device.ERR_OK)
                    return def.TEST;
                else
                    return def.ACTIVE;
            }
            return def.DOWNLOAD;
        }

        public int prime_download()
        {
            Console.WriteLine("enter prime_download");
            int rtn = device.download_firmware(firmware_path);
            if (rtn == def.RTN_OK)
            {
                download_opt_done = 1;
                set_process(def.stage_download);
            }
            else
            {
                rtn = BIN.BIN_CODE[5];
                goto DOWNLOAD_END;
            }
        DOWNLOAD_END:
            await = 0;
            return rtn;
        }
        public int prime_active()
        {
            Console.WriteLine("enter prime_active");
            int rtn = device.disconnect();
            rtn = device.connect(config.firmware_type, config.working_port);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread connect after restart failed");
                set_process(def.stage_calibrate);
                rtn = BIN.BIN_CODE[7];
                goto ACTIVE_END;
            }
            rtn = device.disconnect();
            rtn = device.set_sn_activiate(device.gen_sn(config.station, config.keycode), config.dev_type,null);
            if (rtn == def.RTN_OK)
            {
                active_opt_done = 1;
            }
            else
            {
                rtn = BIN.BIN_CODE[2];
                goto ACTIVE_END;
            }
            rtn = def.RTN_OK;
        ACTIVE_END:
            await = 0;
            return rtn;
        }

        public int prime_test_getinfo()
        {
            int rtn = device.disconnect();
            rtn = device.connect(config.firmware_type, config.working_port);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread connect after restart failed");
                rtn = BIN.BIN_CODE[7];
                goto INFO_END;
            }

            byte[] info = new byte[64];
            int len_info = 64;
            rtn = device.devinfo(info, ref len_info);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread devinfo after restart failed");
                rtn = BIN.BIN_CODE[8];
                goto INFO_END;
            }
            string strsn = "";
            if (config.keycode != "7060")
                strsn = System.Text.Encoding.UTF8.GetString(info.Skip(6).Take(16).ToArray());
            else
                strsn = System.Text.Encoding.UTF8.GetString(info.Take(16).ToArray());

            
            label_list[def.LABLE_SN].Text = strsn;
            

            byte[] version = new byte[64];
            int len = 64;
            rtn = device.version(version, ref len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread version after restart failed");
                rtn = BIN.BIN_CODE[8];
                goto INFO_END;
            }
            string strversion = "";
            if (config.keycode != "7060")
                strversion = System.Text.Encoding.UTF8.GetString(version);
            else
                strversion = device.bytesToHexString(version, 4);

            label_list[def.LABLE_VERSION].Text = strversion;
            rtn = def.RTN_OK;
        INFO_END:
            await = 0;
            return rtn;
        }
        public int prime_calibrate(int mode)
        {
            int rtn = 0;
            Console.WriteLine("enter prime_calibrate");
            if (mode==1)
            {
                Console.WriteLine("prime_calibrate mode = 1");
                rtn = device.calibrate();
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("thread calibrate after restart failed");
                    rtn = BIN.BIN_CODE[4];
                    goto CA_END;
                }
            }
            
            byte[] list = new byte[64];
            int config_len = 0;
            Console.WriteLine("prime get config");
            rtn = device.config(0, list, ref config_len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread config after restart failed");
                rtn = BIN.BIN_CODE[9];
                goto CA_END;
            }
            string strconfig = device.bytesToHexString(list, 7);
            label_list[def.LABLE_PARAMETER].Text = strconfig;
            Console.WriteLine("prime get bg");
            rtn = device.get_bg(bkg_img);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("thread get_bg after restart failed");
                rtn = BIN.BIN_CODE[9];
                goto CA_END;
            }
            rtn = def.RTN_OK;
        CA_END:
            await = 0;
            return rtn;
        }

        public int prime_check_noise(int gain)
        {
            int rtn = def.RTN_FAIL;
            Console.WriteLine("enter prime_check_noise");
            //do bg and empty image check
            byte[] empty_frames = new byte[4*config.sensor_width * config.sensor_height * 10];
            byte[] empty_frame = new byte[4*config.sensor_width * config.sensor_height];
            //get 10 frames
            for (int i = 0; i < 10; i++)
            {
                int frame_len = 4 * config.sensor_width * config.sensor_height;
                rtn = device.capture_frame(0, empty_frame, ref frame_len);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 emptet capture_frame failed");
                    rtn = BIN.BIN_CODE[10];
                    goto NOISE_END;
                }
                fpimage.bmp_frame(empty_frame, bkg_img);
                Buffer.BlockCopy(empty_frame, 0, empty_frames, i * config.sensor_width * config.sensor_height, config.sensor_width * config.sensor_height);
            }
            fpimage.merge_frames(empty_frames, 10, empty_frame);

            float empty_avg = 255;
            if (gain!=0)
            {
                fpimage.gain_frame(empty_frame, empty_frame, gain);
            }

            empty_avg = fpimage.average_noise(empty_frame);
            int[] para = fpimage.get_otsu(empty_frame);
            int rv = para[3] - para[2];
            if (empty_avg < 245 || rv > 10)
            {
                Console.WriteLine("too much noise");
                Image img = bmp_helper.format_from_bytes(empty_frame, 0);
                img_preview.Image = img;
                label_list[def.LABLE_RV].Text = rv.ToString(); ;
                int nAvg = (int)empty_avg;
                label_list[def.LABLE_GRAYLEVEL].Text = nAvg.ToString();
                rtn = BIN.BIN_CODE[16];
                goto NOISE_END;
            }
            float noise = 255 - empty_avg;
            label_list[def.LABLE_NOISE].Text = noise.ToString("f2");
            rtn = def.RTN_OK;
        NOISE_END:
            await = 0;
            return rtn;
        }

        public int preview_image(int gain)
        {
            byte[] frame_data = new byte[4*config.sensor_width * config.sensor_height];
            int rtn = def.RTN_FAIL;
            Console.WriteLine("enter preview_image");
            int frame_len = 4*config.sensor_width * config.sensor_height;
            rtn = device.capture_frame(0, frame_data, ref frame_len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("capture_frame failed");
                rtn = BIN.BIN_CODE[10];
                goto PRE_END;
            }
            fpimage.bmp_frame(frame_data, bkg_img);
            Image img;
            if (gain>0)
            {
                int sw_gain = gain;
                img = bmp_helper.format_from_bytes(frame_data, sw_gain);
                fpimage.gain_frame(frame_data, frame_data, sw_gain);
            }
            else
                img = bmp_helper.format_from_bytes(frame_data, 0);

            int[] para = fpimage.get_otsu(frame_data);
            int rv = para[3] - para[2];
            int avg = fpimage.average_frame(frame_data);

            img_preview.Image = img;
            label_list[def.LABLE_RV].Text = rv.ToString(); ;
            label_list[def.LABLE_GRAYLEVEL].Text = avg.ToString();

        PRE_END:
            await = 0;
            return rtn;
        }

        public int prime_check_button(int wait_time)
        {
            int rtn = def.RTN_FAIL;
            Console.WriteLine("enter prime_check_button");
            //con.stop_pull_button();
            //con.check_button_short();
            rtn = con.wait_button(wait_time);
            if (rtn == def.RTN_OK)
                button_down = 1;
            else
                button_down = 0;
            await = 0;

            return def.RTN_OK;
        }

        public int prime_get_average_frames(int count)
        {
            int rtn = def.RTN_FAIL;
            Console.WriteLine("enter prime_get_average_frames");
            byte[] frame_data = new byte[4*config.sensor_width * config.sensor_height];
            byte[] frame10 = new byte[4*10 * config.sensor_width * config.sensor_height];
            avg_frame = new byte[4*config.sensor_width * config.sensor_height];
            int avg = 0;

            //get 10 frames
            for (int i = 0; i < count; i++)
            {
                int frame_len = 4*config.sensor_width * config.sensor_height;
                rtn = device.capture_frame(0, frame_data, ref frame_len);
                if (rtn != device.ERR_OK)
                {
                    Console.WriteLine("10 capture_frame failed");
                    await = 0;
                    return BIN.BIN_CODE[10];
                }
                if(config.simple_test==0)
                    fpimage.bmp_frame(frame_data, bkg_img);
                Buffer.BlockCopy(frame_data, 0, frame10, i * config.sensor_width * config.sensor_height, config.sensor_width * config.sensor_height);
            }
            avg = fpimage.merge_frames(frame10, count, avg_frame);
            await = 0;
            return def.RTN_OK;   
        }

        public int prime_get_image()
        {
            int rtn = def.RTN_FAIL;
            int len = 0;
            Console.WriteLine("enter prime_get_image");
            avg_frame = new byte[config.sensor_width * config.sensor_height];

            rtn = device.get_img(avg_frame,ref len);
            if (rtn != device.ERR_OK)
            {
                Console.WriteLine("capture_frame failed");
                await = 0;
                return BIN.BIN_CODE[10];
             }
            await = 0;
            return def.RTN_OK;
        }
        public int prime_signal(int gain)
        {
            Console.WriteLine("enter prime_signal");
            int[] para = fpimage.get_otsu(avg_frame);
            int rv = para[3] - para[2];
            int avg = 0;
            int rtn = def.RTN_FAIL;
            Image avg_img;
            if (gain>0)
            {
                int sw_gain = gain;
                avg_img = bmp_helper.format_from_bytes(avg_frame, sw_gain);
                fpimage.gain_frame(avg_frame, avg_frame, sw_gain);
            }
            else
                avg_img = bmp_helper.format_from_bytes(avg_frame, 0);

            avg = fpimage.average_frame(avg_frame);

            avg_img.Save(csv_log.path + label_list[def.LABLE_SN].Text + ".bmp");

            img_preview.Image = avg_img;
            label_list[def.LABLE_RV].Text = rv.ToString(); ;
            label_list[def.LABLE_GRAYLEVEL].Text = avg.ToString();

            if (avg > config.avg_th)
            {
                Console.WriteLine("press failed avg=" + avg.ToString());
                rtn = BIN.BIN_CODE[13];
                goto SIGNAL_END;                                                                                                   
            }
            /*if (avg <200)
            {
                Console.WriteLine("press failed avg=" + avg.ToString());
                rtn = BIN.BIN_CODE[16];
                goto SIGNAL_END;
            }*/
            if (rv < config.rv_th)
            {;
                Console.WriteLine("press failed rv=" + rv.ToString());
                rtn = BIN.BIN_CODE[14];
                goto SIGNAL_END;
            }
            rtn = def.RTN_OK;
        SIGNAL_END:
            await = 0;
            return rtn;
        }
        public int prime_final()
        {
            Console.WriteLine("enter prime_final");
            con.switch_control((int)control.COMMAND.LED, (int)control.MODE.DOWN);
            btn_result.Text = "PASS";
            btn_result.BackColor = Color.Green;
            ssm_state = def.stage_result_ok;
            btn_tip_blink.BackColor = Color.Green;
            Console.Beep(2766, 100);
            Thread.Sleep(50);
            Console.Beep(2766, 100);
            log_info();
            if (config.single_module == 0)
            {
                con.switch_control((int)control.COMMAND.POWER, (int)control.MODE.DOWN);
                if(config.keycode=="2887"|| config.keycode == "2888")
                    con.switch_control((int)control.COMMAND.POWER_MCU, (int)control.MODE.DOWN);
            }
            ram_counter_good++;
            //Thread.Sleep(1500);
            if(config.comm_type==0)
                device.disconnect();   
            working = 0;
            await = 0;
            return def.RTN_OK;
        }
    }
}
