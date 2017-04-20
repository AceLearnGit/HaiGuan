using DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UICommon.Controls;

namespace Pad.Controls
{
    /// <summary>
    /// SetDataCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class SetDataCtrl : UserControl
    {

        private int deviceCount = 1;
        public SetDataCtrl()
        {
            InitializeComponent();
            this.Loaded += SetDataCtrl_Loaded;
        }
        /// <summary>
        /// 初始化本控件需要的参数
        /// 这里为大屏硬件数量，决定了示意区域被分割为几部分
        /// </summary>
        /// <param name="deviceCount">数量</param>
        public void Initialize(int deviceCount)
        {
            this.deviceCount = deviceCount;
            cb_Device.ItemsSource = DataManager.DeviceList;
            //划分示意区域
            for (int i = 0; i < deviceCount; i++)
            {
                ColumnDefinition c = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                regionGrid.ColumnDefinitions.Add(c);
                DeviceControl device = new DeviceControl { Width = regionGrid.Width / deviceCount, Height = regionGrid.Height };
                device.Tag = WCFServiceManager.DeviceClientDic.Keys.ToList()[i];
                device.DeviceSelected += device_DeviceSelected;
                Grid.SetColumn(device, i);
                regionGrid.Children.Add(device);
            }
        }

        private void SetDataCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            //从本地文件中读取后台录入的数据源，物理屏幕等信息
            if (File.Exists(DataManager.DataSourcesFile))
            {
                tb_Column.Text = DataManager.SourcesInfo.Width.ToString();
                tb_Row.Text = DataManager.SourcesInfo.Height.ToString();
            }
            //加载模板列表数据源
            cb_Template.ItemsSource = DataManager.TemplateFileNameList;
            //加载数据源缩略图
            lb_DailySourcesImg.ItemsSource = DataManager.SourcesInfo.SourcesItems;
            cb_Sources.ItemsSource = DataManager.SourcesInfo.SourcesItems;
        }



        #region New
        /// <summary>
        /// 确定按钮事件，使用行列数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            SetLayoutWithRowsandColumns();
        }
        /// <summary>
        /// 使用用户输入的行列数进行布局
        /// </summary>
        private async void SetLayoutWithRowsandColumns()
        {
            //设置行列
            int rows = int.Parse(tb_Row.Text);
            int columns = int.Parse(tb_Column.Text);
            //划分示意区域
            foreach (DeviceControl item in regionGrid.Children)
            {
                if (item.Tag.ToString() == deviceId)
                {
                    await item.SetLayout(deviceId, rows, columns);
                    break;
                }
            }
        }

        /// <summary>
        /// 日常模式源数据列表当前现在在屏幕上的第一个元素的索引 翻页用
        /// </summary>
        private int dailyStartIndex = 0;
        /// <summary>
        /// 日常模式源数据列表当前现在在屏幕上的最后一个元素的索引 翻页用
        /// </summary>
        private int dailyEndIndex = 3;

        /// <summary>
        /// 日常模式上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DailyLast_Click(object sender, RoutedEventArgs e)
        {
            //目前列表里一次能完全显示4个
            //往上翻时使用起始索引控制，起始索引-4
            dailyStartIndex = dailyStartIndex - 4;
            if (dailyStartIndex < 0)
            {
                dailyStartIndex = 0;
            }
            dailyEndIndex = dailyStartIndex + 3;
            lb_DailySourcesImg.ScrollIntoView(lb_DailySourcesImg.Items.GetItemAt(dailyStartIndex));
        }

        /// <summary>
        /// 日常模式下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DailyNext_Click(object sender, RoutedEventArgs e)
        {
            //往下翻时使用结束索引控制，结束索引+4
            dailyEndIndex = dailyEndIndex + 4;
            if (dailyEndIndex > lb_DailySourcesImg.Items.Count - 1)
            {
                dailyEndIndex = lb_DailySourcesImg.Items.Count - 1;
            }
            dailyStartIndex = dailyEndIndex - 3;
            lb_DailySourcesImg.ScrollIntoView(lb_DailySourcesImg.Items.GetItemAt(dailyEndIndex));
        }

        /// <summary>
        /// 源数据缩略图列表数据选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void img_Source_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var info = (SingleSourceInfo)(sender as Image).DataContext;
            foreach (DeviceControl item in regionGrid.Children)
            {
                if (item.Tag.ToString() == deviceId)
                {
                    item.SetSource(info.SourceUrl);
                    break;
                }
            }
        }

        /// <summary>
        /// 显示按钮
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            //挨个调用每个控件的show方法，向设备发送消息
            foreach (DeviceControl item in regionGrid.Children)
            {
                item.Show((bool)cb_isWebkit.IsChecked);
            }

        }
        /// <summary>
        /// 模板下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string templeteName = e.AddedItems[0].ToString();
            UseTemplate(templeteName);
        }
        /// <summary>
        /// 使用用户选中的模板进行布局，填充数据
        /// </summary>
        private async void UseTemplate(string templeteName)
        {
            string fileName = System.IO.Path.Combine(DataManager.TemplateFolder, templeteName);
            if (File.Exists(fileName))
            {
                var templateInfo = Common.SerializationHelper.DeserializeXmlFile2Obj<Devices>(fileName);
                if (templateInfo != null)
                {
                    if (templateInfo.DeviceList.Count > 0)
                    {
                        for (int i = 0; i < templateInfo.DeviceList.Count; i++)
                        {
                            DeviceControl c = null;
                            foreach (DeviceControl item in regionGrid.Children)
                            {
                                if (item.Tag.ToString() == templateInfo.DeviceList[i].DeviceID)
                                {
                                    c = item;
                                    break;
                                }
                            }
                            await c.SetLayout(templateInfo.DeviceList[i].DeviceID, templateInfo.DeviceList[i].Row, templateInfo.DeviceList[i].Column);
                            c.AddBorder(templateInfo.DeviceList[i].DeviceDataList);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 删除模板按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string fileName = btn?.DataContext.ToString();
            //删除模板文件
            string fullFileName = System.IO.Path.Combine(DataManager.TemplateFolder, fileName);
            try
            {
                //这里或许有坑
                //如果先用File.Exists(string path)判断文件在不在，可能文件即使存在也会返回false，详见F12方法说明
                File.Delete(fullFileName);
            }
            catch (Exception ex)
            {
                //貌似遇到过点删除按钮时崩溃
                Common.LogManager.LogError(ex, "btn_DeleteTemplate_Click");
            }
            //从下拉框移除
            DataManager.TemplateFileNameList.Remove(fileName);
        }


        /// <summary>
        /// 存为模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsTemplete_Click(object sender, RoutedEventArgs e)
        {
            Devices devices = new Devices() { DeviceList = new List<Device>() };
            foreach (DeviceControl item in regionGrid.Children)
            {
                Device device = new Device()
                {
                    Column = item.Columns,
                    Row = item.Rows,
                    DeviceID = item.DeviceId,
                    DeviceDataList = item.BorderDic.Values.ToList()
                };
                devices.DeviceList.Add(device);
            }
            DataManager.SaveAsTemplate(devices);
        }
        #endregion

        int deviceIndex = -1;

        string deviceId = string.Empty;
        /// <summary>
        /// 设备列表选中项改变时，记录当前正在操作哪台设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string device = (sender as ComboBox).SelectedValue.ToString();
            deviceId = device.Replace("机器", "");
            deviceIndex = (sender as ComboBox).SelectedIndex;
        }

        /// <summary>
        /// 示意区域中小区域被选中时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">该小区所在设备的id</param>
        private void device_DeviceSelected(object sender, object e)
        {
            deviceId = e.ToString();
        }

        /// <summary>
        /// 单机刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index;
            if (int.TryParse(btn.Tag.ToString(), out index))
            {
                (regionGrid.Children[index - 1] as DeviceControl)?.Show((bool)cb_isWebkit.IsChecked);
            }
        }

        /// <summary>
        /// 数据源下拉框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var info = (SingleSourceInfo)(sender as TextBlock).DataContext;
            if (info != null)
            {
                foreach (DeviceControl item in regionGrid.Children)
                {
                    if (item.Tag.ToString() == deviceId)
                    {
                        item.SetSource(info.SourceUrl);
                        break;
                    }
                }
            }
        }
    }
}
