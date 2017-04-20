using DataModel;
using Pad.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pad
{
    internal class DataManager
    {
        static DataManager()
        {
            //加载数据源
            SourcesInfo = Common.SerializationHelper.DeserializeXmlFile2Obj<SourcesInfo>(DataSourcesFile);
            TemplateFileNameList = new ObservableCollection<string>();
            DeviceList = new ObservableCollection<string>();
            //创建存放模板的文件夹
            if (!Directory.Exists(TemplateFolder))
            {
                Directory.CreateDirectory(TemplateFolder);
            }
            string[] fileList = Directory.GetFiles(TemplateFolder);
            for (int i = 0; i < fileList.Length; i++)
            {
                TemplateFileNameList.Add(Path.GetFileName(fileList[i]));
            }
        }

        /// <summary>
        /// 用户输入的行列数
        /// </summary>
        public static int Rows = 1;
        public static int Columns = 1;
        /// <summary>
        /// 通过后台保存的信息，包括物理屏大小，每条数据源的名称、链接、缩略图路径
        /// </summary>
        public static SourcesInfo SourcesInfo { get; set; }
        /// <summary>
        /// 已保存模板列表
        /// </summary>
        public static ObservableCollection<string> TemplateFileNameList { get; set; }
        /// <summary>
        /// 存放模板的路径
        /// </summary>
        public static readonly string TemplateFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        /// <summary>
        /// 后台程序录入的各种数据的存放目录
        /// </summary>
        public static readonly string DataSourcesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GSDM");
        /// <summary>
        /// 后台程序录入的各种数据的文件名
        /// </summary>
        public static readonly string DataSourcesFile = Path.Combine(DataSourcesFolder, "testConfig.xml");
        /// <summary>
        /// 当前大屏上的信息
        /// 为了持久化数据，方便选择模式页面和日常操作页面都能使用
        /// </summary>
        public static ScreenItemsInfo ScreenItemsInfoOnScreen = new ScreenItemsInfo() { ScreenItems = new List<SingleScreenItemInfo>() };
        /// <summary>
        /// 组成大屏的设备的ID
        /// </summary>
        public static ObservableCollection<string> DeviceList
        {
            get;
            set;
        }


        /// <summary>
        /// 把某个Grid分为几行几列
        /// </summary>
        /// <param name="Rows">行数</param>
        /// <param name="Columns">列数</param>
        /// <param name="board">要划分的Grid</param>
        public static void SetBoardLayout(int Rows, int Columns, Grid board)
        {
            if (Rows != 0 && Columns != 0)
            {
                board.Children.Clear();
                board.RowDefinitions.Clear();
                board.ColumnDefinitions.Clear();
                for (int j = 0; j < Rows; j++)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(1, GridUnitType.Star);
                    board.RowDefinitions.Add(row);
                }
                for (int k = 0; k < Columns; k++)
                {
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(1, GridUnitType.Star);
                    board.ColumnDefinitions.Add(column);
                }
                //每行
                for (int j = 0; j < Rows; j++)
                {
                    //每列
                    for (int k = 0; k < Columns; k++)
                    {
                        //在每行每列放一个Border占位，视觉效果
                        Border screenItem = new Border();
                        screenItem.BorderBrush = new SolidColorBrush(Colors.Black);
                        screenItem.BorderThickness = new Thickness(1);
                        Grid.SetColumn(screenItem, k);
                        Grid.SetRow(screenItem, j);
                        board.Children.Add(screenItem);
                    }
                }
            }
        }

        /// <summary>
        /// 把一个保存了大屏的信息对象序列化为模板文件
        /// </summary>
        /// <param name="itemsInfo"></param>
        public static void SaveAsTemplate(Devices itemsInfo)
        {
            ConfirmDialog dialog = new ConfirmDialog();
            if ((bool)dialog.ShowDialog())
            {
                string fileName = Path.Combine(TemplateFolder, dialog.Result + ".Xml");
                Common.SerializationHelper.SerializerObj2XmlFile(itemsInfo, fileName);
                //获取文件名，添加到模板列表中
                string fileShortName = Path.GetFileName(fileName);
                //更新模板列表
                if (!TemplateFileNameList.Contains(fileShortName))
                {
                    TemplateFileNameList.Add(fileShortName);
                }
            }
        }
    }
}
