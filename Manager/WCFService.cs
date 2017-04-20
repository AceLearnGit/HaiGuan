using DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Manager
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“WCFService”。
    public class WCFService : IWCFService
    {

        private static string ConfigFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"DevicesConfig.xml");
        public void DoWork()
        {
        }
        /// <summary>
        /// 获取大屏各设备的WCF配置
        /// </summary>
        /// <returns></returns>
        public List<MachineConfigModel> GetAllMachineConfig()
        {
            DeviceInfoConfigModel model = Common.SerializationHelper.DeserializeXmlFile2Obj<DeviceInfoConfigModel>(ConfigFileName);
            return model.ScreenDeviceList;
        }
        /// <summary>
        /// 获取会议室设备的WCF配置
        /// </summary>
        /// <returns></returns>
        public MachineConfigModel GetMettingMachineConfig()
        {
            DeviceInfoConfigModel model = Common.SerializationHelper.DeserializeXmlFile2Obj<DeviceInfoConfigModel>(ConfigFileName);
            return model.MettingDevice;
        }
    }
}
