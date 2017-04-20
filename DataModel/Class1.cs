using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataModel
{
    [XmlRoot("ScreentItemsInfo")]
    public class ScreenItemsInfo
    {
        /// <summary>
        /// 屏幕上有多少行
        /// </summary>
        [XmlAttribute("rows")]
        public int Rows { get; set; }

        /// <summary>
        /// 屏幕上有多少列
        /// </summary>
        [XmlAttribute("columns")]
        public int Columns { get; set; }

        /// <summary>
        /// 屏幕上各小屏幕的信息
        /// </summary>
        [XmlArray("ScreenItems"), XmlArrayItem("ScreenItem")]
        public List<SingleScreenItemInfo> ScreenItems { get; set; }
    }

    /// <summary>
    /// 每块小屏的数据模型
    /// </summary>
    [XmlRoot("ScreenItem")]
    public partial class SingleScreenItemInfo
    {
        /// <summary>
        /// Source
        /// </summary>
        [XmlAttribute("TargetUrl")]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 行位置
        /// </summary>
        [XmlElement("Row")]
        public int Row { set; get; }

        /// <summary>
        /// 占行数
        /// </summary
        [XmlElement("RowSpan")]
        public int RowSpan { set; get; } = 1;

        /// <summary>
        /// 列位置
        /// </summary>
        [XmlElement("Column")]
        public int Column { set; get; }

        /// <summary>
        /// 占列数
        /// </summary>
        [XmlElement("ColumnSpan")]
        public int ColumnSpan { set; get; } = 1;

        /// <summary>
        /// 索引,方便平板更新某块屏幕时根据索引找到对应对象
        /// </summary>
        [XmlElement("ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// 是否使用webkit内核
        /// </summary>
        [XmlElement("IsWebKit")]
        public bool IsWebKit { get; set; }
    }

    [XmlRoot("SourcesInfo")]
    public class SourcesInfo
    {
        /// <summary>
        /// 横向有几块物理屏
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// 纵向有几块物理屏
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }


        [XmlArray("SourcesItems"), XmlArrayItem("SingleSourceInfo")]
        public List<SingleSourceInfo> SourcesItems { get; set; }
    }

    /// <summary>
    /// 单个数据源的数据模型
    /// </summary>
    [XmlRoot("SingleSourceInfo")]
    public class SingleSourceInfo
    {
        /// <summary>
        /// 行位置
        /// </summary>
        [XmlAttribute("KeyName")]
        public string KeyName { set; get; }

        /// <summary>
        /// 占行数
        /// </summary
        [XmlAttribute("SourceUrl")]
        public string SourceUrl { set; get; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [XmlAttribute("TargetImg")]
        public string TargetImg { get; set; }

        public override string ToString()
        {
            return KeyName;
        }
    }
}
