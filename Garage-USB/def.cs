using System;
using System.Collections.Generic;
using System.Text;

namespace Garage_USB
{
    public class def
    {
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public static int WM_DEVICECHANGE = 0x219;
        public static string template_hearder = "key";
        public static string template_tail = ".bin";
        public static int stage_start = 0;
        public static int stage_power_up = 17;
        public static int stage_download = 35;
        public static int stage_restart = 52;
        public static int stage_calibrate = 70;
        public static int stage_press = 85;
        public static int stage_result= 99;
        public static int stage_result_ok = 100;
        public static int stage_result_fail = 0;
        public static int RTN_OK = 0;
        public static int RTN_FAIL = -1;
        public static int RTN_ON =2;
        public static int RTN_TIMEOUT = 3;

        public static string firmware_file = "_pxat_fm.bin";

        public static int COSTYPE_USB_MOH_F323 = 64;
        public static int DEVTYPE_USB_MOH_F323 = 23;

        public static int COSTYPE_SERIAL_F225 = 20;
        public static int DEVTYPE_SERIAL_F225 = 12;

        public static int COSTYPE_USB_MOCH_MOH_BLD = 54;
        public static int DEVTYPE_USB_MOCH_MOH_BLD = 29;

        public static int DOWNLOAD = 1000;
        public static int ACTIVE = 1001;
        public static int TEST = 1002;

        public static int FIRST_POWER_UP = 1;
        public static int SECOND_POWER_UP = 2;

        public static int ENKIDU_POWER_UP_MCU = 3;
        public static int ENKIDU_POWER_UP_SENSOR = 4;

        public static int ENKIDU_POWER_OFF_MCU = 1;
        public static int ENKIDU_POWER_OFF_SENSOR = 2;

        public static int LABLE_PROJECT = 0;
        public static int LABLE_PRODUCT = 1;
        public static int LABLE_SN = 2;
        public static int LABLE_VOLTAGE = 3;
        public static int LABLE_CURRENT = 4;
        public static int LABLE_PARAMETER = 5;
        public static int LABLE_VERSION = 6;
        public static int LABLE_RV = 7;
        public static int LABLE_GRAYLEVEL = 8;
        public static int LABLE_NOISE = 9;
        public static int LABLE_BIN = 10;
        public static int LABLE_STATION = 11;
        public static int lable_count = 12;

        public const int FUNC_POWER_UP = 1;
        public const int FUNC_DOWNLOAD = 2;
        public const int FUNC_ACTIVE = 3;
        public const int FUNC_GETINFO = 4;
        public const int FUNC_CALIBRATE = 5;
        public const int FUNC_NOISE = 6;
        public const int FUNC_PREVIEW = 7;
        public const int FUNC_BUTTON = 8;
        public const int FUNC_FRAME = 9;
        public const int FUNC_SINGAL = 10 ;
        public const int FUNC_FINAL = 11;
        public const int FUNC_POWER_DOWN = 12;
        public const int FUNC_IMAGE= 13;

    }
}
