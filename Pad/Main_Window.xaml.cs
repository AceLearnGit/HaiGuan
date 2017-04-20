using DataModel;
using Pad.Controls;
using Pad.ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pad
{
    /// <summary>
    /// Main_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Main_Window : Window
    {
        public Main_Window()
        {
            InitializeComponent();
            this.Loaded += Main_Window_Loaded;
        }
        /// <summary>
        /// MainWindow的Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Main_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化WCFServiceManager，从Manager获取数据
            await WCFServiceManager.Instance.InitializeDeviceClientDic();
            rb_SetData.IsChecked = true;
            ScreenRadioButton.IsChecked = true;
            setDataCtrl.Initialize(WCFServiceManager.DeviceClientDic.Count);
        }

        /// <summary>
        /// 选择模式
        /// 显示SetDataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_SetData_Checked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(LayoutRoot, "SetDataState", true);
        }

        /// <summary>
        /// 日常模式
        /// 显示DailyOperateGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_DailyOperare_Checked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(LayoutRoot, "DailyOperateState", true);
            //初始化对应Grid中的数据
            dailyOpetateCtrl.SetDailyWindow();
        }
        /// <summary>
        /// 退出
        /// 关闭应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_Exit_Checked(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        /// <summary>
        /// 会议室、大屏切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (ScreenRadioButton.IsChecked == true)
            {
                VisualStateManager.GoToElementState(LayoutRoot, "ScreenModeState", true);
            }
            else
            {
                VisualStateManager.GoToElementState(LayoutRoot, "MeetingRoomState", true);
            }

        }
    }
}
