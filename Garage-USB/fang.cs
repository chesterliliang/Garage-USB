using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Garage_USB
{
    public class fang
    {
        public byte[] bkg_img = null;

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

        public fang(string app_path)
        {
            ref_path = app_path;
            firmware_path = app_path + @"\data\" + config.keycode + def.firmware_file;
            Console.WriteLine("firmware path = " + firmware_path);
            csv_log.path = app_path + @"\img\";
            Console.WriteLine("img storage path = " + csv_log.path);
            configs_path = app_path + @"\files\configs.csv";
            Console.WriteLine("configs_path = " + configs_path);
            station_path = app_path + @"\files\station.csv"; ;
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

    }
}
