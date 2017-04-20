using DataModel;
using Pad.ManagerService;
using Pad.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pad
{
    /// <summary>
    /// WCF通信帮助类
    /// </summary>
    public class WCFServiceManager
    {
        /// <summary>
        /// Manger的Client
        /// </summary>
        private static WCFServiceClient managerClient;
        /// <summary>
        /// 某台设备的Client
        /// </summary>
        private Service1Client activeDeviceClient;
        /// <summary>
        /// 会议室的Client
        /// </summary>
        public Service1Client mettingClient;

        //单例
        private static WCFServiceManager _Instance;
        public static WCFServiceManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new WCFServiceManager();
                    managerClient = new WCFServiceClient();
                }
                return _Instance;
            }

        }

        private WCFServiceManager()
        {
        }
        /// <summary>
        /// 大屏上设备的Client字典
        /// </summary>
        public static Dictionary<string, Service1Client> DeviceClientDic = new Dictionary<string, Service1Client>();
        /// <summary>
        /// 初始化各设备的WCFClient
        /// </summary>
        /// <returns></returns>
        public async Task InitializeDeviceClientDic()
        {
            MachineConfigModel[] modelList = await managerClient.GetAllMachineConfigAsync();
            for (int i = 0; i < modelList.Length; i++)
            {
                DeviceClientDic.Add(modelList[i].DeciveID, new Service1Client(new BasicHttpBinding("BasicHttpBinding_IWCFService"), new EndpointAddress(modelList[i].DeviceEndPointAddress)));
                DataManager.DeviceList.Add(modelList[i].DeciveID);
            }
            mettingClient = new Service1Client(new BasicHttpBinding("BasicHttpBinding_IWCFService"), new EndpointAddress((await managerClient.GetMettingMachineConfigAsync()).DeviceEndPointAddress));
        }
        /// <summary>
        /// 通过deviceid选择当前活动的WCFClient
        /// </summary>
        /// <param name="deviceId"></param>
        private void SelectWCFClient(string deviceId)
        {
            if (deviceId == "Metting")
            {
                activeDeviceClient = mettingClient;
            }
            else
            {
                activeDeviceClient = DeviceClientDic[deviceId];
            }
        }

        /// <summary>
        /// 将某个设备划分
        /// </summary>
        /// <param name="deviceId">设备id</param>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <returns></returns>
        public async Task SetLayout(string deviceId, int rows, int columns)
        {
            try
            {
                SelectWCFClient(deviceId);
                await activeDeviceClient.SetLayoutAsync(rows, columns);
            }
            catch (Exception ex)
            {
                Common.LogManager.LogError(ex, "WCFServiceManager.SetLayout");
            }
        }
        /// <summary>
        /// 给某设备添加数据
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public async Task AddContentToScreen(string deviceId, SingleScreenItemInfo[] array)
        {
            try
            {
                SelectWCFClient(deviceId);
                await activeDeviceClient.AddContentToScreenAsync(array);
            }
            catch (Exception ex)
            {
                Common.LogManager.LogError(ex, "WCFServiceManager.AddContentToScreen");
            }
        }
        /// <summary>
        /// 更新某设备的一个数据
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateWindowInfo(string deviceId, SingleScreenItemInfo item)
        {
            try
            {
                SelectWCFClient(deviceId);
                await activeDeviceClient.UpdateWindowInfoAsync(item);
            }
            catch (Exception ex)
            {
                Common.LogManager.LogError(ex, "WCFServiceManager.UpdateWindowInfo");
            }
        }
        /// <summary>
        /// 选中大屏上某块小屏
        /// </summary>
        /// <param name="index">小屏的Index</param>
        public async Task SelectWindow(string deviceId, Guid guid)
        {
            try
            {
                SelectWCFClient(deviceId);
                await activeDeviceClient.SelectWindowAsync(guid);
            }
            catch (Exception ex)
            {
                Common.LogManager.LogError(ex, "WCFServiceManager.SelectWindow");
            }
        }
        /// <summary>
        /// 获取某设备显示的内容
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<ScreenItemsInfo> GetCurrentWindowInfo(string deviceId)
        {
            ScreenItemsInfo ret = null;
            try
            {
                SelectWCFClient(deviceId);
                ret = await activeDeviceClient.GetCurrentWindowInfoAsync();
            }
            catch (Exception ex)
            {
                Common.LogManager.LogError(ex, "WCFServiceManager.GetCurrentWindowInfo");
            }
            return ret;
        }
    }
}
