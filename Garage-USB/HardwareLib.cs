using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace Garage_USB
{


        public class HardWareLib
        {
            /// <summary>
            /// 设备的各项属性，注意有些属性是不通用的，例如SPDRP_FRIENDLYNAME只适用于端口设备
            /// </summary>
            public enum SPDRP_
            {
                SPDRP_DEVICEDESC = (0x00000000),  // DeviceDesc (R/W)
                SPDRP_HARDWAREID = (0x00000001),  // HardwareID (R/W)
                SPDRP_SERVICE = (0x00000004), // Service (R/W)
                SPDRP_CLASS = (0x00000007),  // Class (R--tied to ClassGUID)
                SPDRP_CLASSGUID = (0x00000008),  // ClassGUID (R/W)
                SPDRP_DRIVER = (0x00000009),  // Driver (R/W)
                SPDRP_CONFIGFLAGS = (0x0000000A), // ConfigFlags (R/W)
                SPDRP_MFG = (0x0000000B), // Mfg (R/W)
                SPDRP_FRIENDLYNAME = (0x0000000C),  // FriendlyName (R/W)
                SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = (0x0000000E),  // PhysicalDeviceObjectName (R)
                SPDRP_CAPABILITIES = (0x0000000F), // Capabilities (R)
                SPDRP_REMOVAL_POLICY_HW_DEFAULT = (0x00000020),  // Hardware Removal Policy (R)
                SPDRP_INSTALL_STATE = (0x00000022), // Device Install State (R)
            }
            public const int DIGCF_ALLCLASSES = (0x00000004);
            public const int DIGCF_DEVICEINTERFACE = 0x00000010;
            public const int DIGCF_PRESENT = (0x00000002);
            public const int INVALID_HANDLE_VALUE = -1;
            public const int MAX_DEV_LEN = 1000;

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);

        [DllImport(@"setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, SP_DEVICE_INTERFACE_DATA deviceInterfaceData, SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, SP_DEVINFO_DATA deviceInfoData);
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        // <summary>
        /// 获取指定设备的属性
        /// </summary>
        /// <param name="lpInfoSet">设备信息集合</param>
        /// <param name="DeviceInfoData">表示该设备</param>
        /// <param name="Property">表示要获取哪项属性</param>
        /// <param name="PropertyRegDataType">注册类型</param>
        /// <param name="PropertyBuffer">属性（输出）</param>
        /// <param name="PropertyBufferSize">存储属性的字节大小</param>
        /// <param name="RequiredSize">需要的字节大小</param>
        /// <returns>是否成功</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
            public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr lpInfoSet,
                                        SP_DEVINFO_DATA DeviceInfoData,
                                        UInt32 Property,
                                        UInt32 PropertyRegDataType,
                                        StringBuilder PropertyBuffer,
                                        UInt32 PropertyBufferSize,
                                        IntPtr RequiredSize);

            /// <summary>
            /// 销毁一个设备信息集合，并且释放所有关联的内存
            /// </summary>
            /// <param name="lpInfoSet">设备信息集合</param>
            /// <returns></returns>
            [DllImport("setupapi.dll", SetLastError = true)]
            public static extern bool SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);
            /// <summary>
            /// 设备信息数据
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public class SP_DEVINFO_DATA
            {
                public int cbSize;//本结构的大小（字节表示）
                public Guid classGuid;//本结构所表示的设备的GUID
                public int devInst;//设备句柄
                public ulong reserved;//没用
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

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo,SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData,uint deviceInterfaceDetailDataSize,out uint requiredSize,SP_DEVINFO_DATA deviceInfoData
);
    }
    
}
