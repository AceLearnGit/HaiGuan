using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    /// <summary>
    /// 每台硬件设备的WCF配置数据模型
    /// </summary>
    public class MachineConfigModel
    {
        public string DeciveID { get; set; }
        public string DeviceEndPointAddress { get; set; }
    }
    /// <summary>
    /// “后台”用来保存所有硬件设备WCF配置的数据模型
    /// </summary>
    public class DeviceInfoConfigModel
    {
        /// <summary>
        /// 大屏的设备
        /// </summary>
        public List<MachineConfigModel> ScreenDeviceList { get; set; }
        /// <summary>
        /// 会议室设备
        /// </summary>
        public MachineConfigModel MettingDevice { get; set; }
    }
}
