using System;
using System.Collections.Generic;
using System.Text;

namespace Garage_USB
{
    public class def
    {
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public static int WM_DEVICECHANGE = 0x219;
        public static string template_name = "key.bin";
        public static int stage_start = 0;
        public static int stage_power_up = 1;
        public static int stage_download = 2;
        public static int stage_restart = 3;
        public static int stage_calibrate = 4;
        public static int stage_press = 5;
        public static int stage_result= 6;
        public static int stage_result_ok = 7;
        public static int stage_result_fail = -1;
        public static int RTN_OK = 0;
        public static int RTN_FAIL = -1;

        public static int BIN_CODE_F4 = -4;//too much current
        public static int BIN_CODE_F3 = -3;//already downloaded or download fail
        public static int BIN_CODE_F2 = -2;//no current fail
        public static int BIN_CODE_01 = 1;//connect fail
        public static int BIN_CODE_02 = 2;//get info opts fail
        public static int BIN_CODE_03 = 3;//get config fail
        public static int BIN_CODE_04 = 4;//calirate fail
        public static int BIN_CODE_05 = 5;//get background bmp fail
        public static int BIN_CODE_06 = 6;//pull image fail in preview
        public static int BIN_CODE_07 = 7;//pull image fail when check
        public static int BIN_CODE_08 = 8;
        public static int BIN_CODE_09 = 9;
        public static int BIN_CODE_10 = 10;//write sn and activiate fail
        public static int BIN_CODE_11 = 11;
        public static int BIN_CODE_12 = 12;
        public static int BIN_CODE_13 = 13;
        public static int BIN_CODE_14 = 14;
        public static int BIN_CODE_15 = 15;
        public static int BIN_CODE_16 = 16;
        public static int BIN_CODE_17 = 17;
        public static int BIN_CODE_18 = 18;
        public static int BIN_CODE_19 = 19;
        public static int BIN_CODE_20 = 20;
        public static int BIN_CODE_21 = 21;//AVG fail
        public static int BIN_CODE_22 = 22;//RV FAIL
        public static string firmware_file = "_pxat_fm.bin";

        public static int COSTYPE_USB_MOH_F323 = 64;
        public static int DEVTYPE_USB_MOH_F323 = 23;
        public static int COSTYPE_USB_MOCH_MOH_BLD = 54;
        public static int DEVTYPE_USB_MOCH_MOH_BLD = 29;
    }
}
