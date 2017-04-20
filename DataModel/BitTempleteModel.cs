using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataModel
{
    /// <summary>
    /// 大模板数据模型
    /// </summary>
    [XmlRoot("Devices")]
    public class Devices
    {
        [XmlArray("DeviceList"), XmlArrayItem("Device")]
        public List<Device> DeviceList { get; set; }
    }

    /// <summary>
    /// 每台硬件设备的数据模型
    /// </summary>
    [XmlRoot("Device")]
    public class Device
    {
        /// <summary>
        /// 被分为几行
        /// </summary>
        [XmlAttribute("Row")]
        public int Row { set; get; }

        /// <summary>
        /// <summary>
        /// 被分为几列
        /// </summary>
        [XmlAttribute("Column")]
        public int Column { set; get; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [XmlAttribute("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 每块区域的信息
        /// </summary>
        [XmlArray("DeviceDataList"), XmlArrayItem("SingleScreenItemInfo")]
        public List<SingleScreenItemInfo> DeviceDataList { get; set; }

    }
}
