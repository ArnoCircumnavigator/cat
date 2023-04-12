using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Cat.WindowsSystemInformations
{
    public partial class WindowsSystemInformation
    {
        /// <summary>
        /// 设备级别
        /// </summary>
        public enum DeviceLevel
        {
            LowLevel = 1,
            MidLevel = 2,
            HighLevel = 3
        }
        /// <summary>
        /// Interl HD Graphics 显卡,这一系列一堆堆都是集显
        /// </summary>
        const int VendorID = 32902;
        public static DeviceLevel GetDeviceLevel()
        {
            if (SystemInfo.graphicsDeviceVendorID == VendorID)
            {
                //集显，emmmm
                return DeviceLevel.LowLevel;
            }
            else // N卡，A卡都还不错
            {
                //显存
                int G_MemorySize = SystemInfo.graphicsMemorySize;
                //内存
                int S_MemorySize = SystemInfo.systemMemorySize;
                if (G_MemorySize > 6000 && S_MemorySize >= 8000)
                    return DeviceLevel.HighLevel;
                else if
                    (
                    (G_MemorySize >= 2000 && G_MemorySize < 6000 && S_MemorySize >= 4000)
                    ||
                    (S_MemorySize >= 4000 && S_MemorySize < 8000 && G_MemorySize >= 2000)
                    )
                    return DeviceLevel.MidLevel;
                return DeviceLevel.LowLevel;
            }
        }
    }
}