using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Garage_USB
{
        [StructLayout(LayoutKind.Sequential)]
        public class SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid classGuid;
            public uint devInst;
            public IntPtr reserved;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            public byte[] devicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        public class Win32
        {
            public static uint ANYSIZE_ARRAY = 1000;

            [DllImport("setupapi.dll", SetLastError = true)]
            public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

            [DllImport("setupapi.dll", SetLastError = true)]
            public static extern Boolean SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);

            [DllImport(@"setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

            [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, SP_DEVICE_INTERFACE_DATA deviceInterfaceData, SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, SP_DEVINFO_DATA deviceInfoData);

            public const int DIGCF_PRESENT = 0x02;
            public const int DIGCF_DEVICEINTERFACE = 0x10;
            public const int SPDRP_DEVICEDESC = (0x00000000);
            public const long ERROR_NO_MORE_ITEMS = 259L;
        }
    
}
