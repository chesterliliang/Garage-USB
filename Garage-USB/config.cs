using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace Garage_USB
{
    public class config
    {

        public static string project_name = "";
        public static string product_code = "";
        public static int sensor_width = -1;
        public static int sensor_height= -1;
        public static int avg_th = -1;
        public static int rv_th = -1;
        public static int v_th = -1;
        public static int c_th_high = -1;
        public static int c_th_low = -1;
        public static int station = -1;
        public static string version = "";
        public static string keycode = "";
        public static string comport = "";
        public static int sw_gain = 48;
        public static int firmware_type = 0;
        public static int dev_type = 0;
        public static int channel = 0;
        public static int auto_start = 0;
        public static int comm_type = 0;
        public static int single_module = 0;
        public static int test_only = 0;
        public static int simple_test = 0;
        public static int c_check = 0;
        public static int v_check = 0;
        public static int btn_check = 0;

        public static void init_station(fang g)
        {
            string station_path = g.station_path;
            Console.WriteLine("station_path !" + station_path);
            FileStream station_fs = new FileStream(station_path, FileMode.Open, FileAccess.Read);
            g.init_station();
            csv_log.csv2dt(station_fs, 0, g.dt_station);

            config.station = Convert.ToInt32(g.dt_station.Rows[0]["Station"]);
            config.channel = Convert.ToInt32(g.dt_station.Rows[0]["Channel"]);
            config.auto_start = Convert.ToInt32(g.dt_station.Rows[0]["auto_start"]);
            config.single_module = Convert.ToInt32(g.dt_station.Rows[0]["single_module"]);
            config.test_only = Convert.ToInt32(g.dt_station.Rows[0]["test_only"]);
            config.simple_test = Convert.ToInt32(g.dt_station.Rows[0]["simple_test"]);
            config.comport = g.dt_station.Rows[0]["comport"].ToString();
            config.c_check = Convert.ToInt32(g.dt_station.Rows[0]["c_check"]);
            config.v_check = Convert.ToInt32(g.dt_station.Rows[0]["v_check"]);

            station_fs.Close();
            station_fs.Dispose();
            Console.WriteLine("station loaded !");
        }

        public static void load_config(fang g,int i)
        {
            config.project_name = g.dt_configs.Rows[i]["Project"].ToString();
            config.product_code = g.dt_configs.Rows[i]["Product"].ToString();
            config.sensor_width = Convert.ToInt32(g.dt_configs.Rows[i]["sensor_width"]);
            config.sensor_height = Convert.ToInt32(g.dt_configs.Rows[i]["sensor_height"]);
            config.avg_th = Convert.ToInt32(g.dt_configs.Rows[i]["avg_th"]);
            config.rv_th = Convert.ToInt32(g.dt_configs.Rows[i]["rv_th"]);
            config.c_th_high = Convert.ToInt32(g.dt_configs.Rows[i]["c_th_high"]);
            config.c_th_low = Convert.ToInt32(g.dt_configs.Rows[i]["c_th_low"]);
            config.v_th = Convert.ToInt32(g.dt_configs.Rows[i]["v_th"]);
            config.version = g.dt_configs.Rows[i]["Version"].ToString();
            config.comm_type = Convert.ToInt32(g.dt_configs.Rows[i]["comm_type"]);
            config.keycode = g.dt_configs.Rows[i]["Keycode"].ToString();
            config.firmware_type = Convert.ToInt32(g.dt_configs.Rows[i]["COSTYPE"]);
            config.dev_type = Convert.ToInt32(g.dt_configs.Rows[i]["DEVTYPE"]);
            config.btn_check = Convert.ToInt32(g.dt_configs.Rows[i]["btn_check"]);
            g.firmware_path = g.ref_path + @"data\" + config.keycode + def.firmware_file;
            Console.WriteLine("firmware path = " + g.firmware_path);
        }

        public static int init_projects(fang g)
        {
            string configs_path = g.configs_path;
            Console.WriteLine("configs_path !" + configs_path);
            g.init_config();
            FileStream configs_fs = new FileStream(configs_path, FileMode.Open, FileAccess.Read);
            int selected = 0;
            csv_log.csv2dt(configs_fs, 0, g.dt_configs);
            for (int i = 0; i < g.dt_configs.Rows.Count; i++)
            {
               
                if (Convert.ToInt32(g.dt_configs.Rows[i]["Default"]) == 1)
                {
                    selected = i;
                }
            }
            configs_fs.Close();
            Console.WriteLine("configs loaded !");
            return selected;


        }
    }
}
