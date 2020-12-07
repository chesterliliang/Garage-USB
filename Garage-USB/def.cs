using System;
using System.Collections.Generic;
using System.Text;

namespace Garage_USB
{
    public class def
    {
        public static int WM_DEVICECHANGE = 0x219;
        public static int DBT_DEVICEARRIVAL = 0x8000;
        public static int DBT_CONFIGCHANGECANCELED = 0x0019;
        public static int DBT_CONFIGCHANGED = 0x0018;
        public static int DBT_CUSTOMEVENT = 0x8006;
        public static int DBT_DEVICEQUERYREMOVE = 0x8001;
        public static int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public static int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public static int DBT_DEVICEREMOVEPENDING = 0x8003;
        public static int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public static int DBT_QUERYCHANGECONFIG = 0x0017;
        public static int DBT_USERDEFINED = 0xFFFF;

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

        public static int BIN_CODE_01 = 1;
        public static int BIN_CODE_02 = 2;
        public static int BIN_CODE_03 = 3;
        public static int BIN_CODE_04 = 4;
        public static int BIN_CODE_05 = 5;
        public static int BIN_CODE_06 = 6;
        public static int BIN_CODE_07 = 7;
        public static int BIN_CODE_08 = 8;
        public static int BIN_CODE_09 = 9;
        public static int BIN_CODE_10 = 10;
        public static int BIN_CODE_21 = 21;
        public static int BIN_CODE_22 = 22;
        public static string firmware_file = "_pxat_fm.bin";
    }
}
