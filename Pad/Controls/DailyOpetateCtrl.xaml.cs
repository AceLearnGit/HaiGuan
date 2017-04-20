using Common;
using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Pad.Controls
{
    /// <summary>
    /// DailyOpetateCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class DailyOpetateCtrl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// 当前操作的机器的id
        /// </summary>
        string deviceId = string.Empty;
        /// <summary>
        /// 甩动区域所在位置 example:sendArea=3 即控件的上部三分之一的面积内可以通过拖动触发选择数据源
        /// </summary>
        private static double sendAreaRatio = 2;
        /// <summary>
        /// 日常模式区域列表选中项的索引
        /// </summary>
        private Guid dailySelectedIndex = default(Guid);

        public DailyOpetateCtrl()
        {
            InitializeComponent();
            this.Loaded += DailyOpetateCtrl_Loaded;
        }

        private void DailyOpetateCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            //加载数据源缩略图
            lb_DailySourcesImg.ItemsSource = DataManager.SourcesInfo.SourcesItems;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var info = (SingleSourceInfo)(sender as Button).DataContext;
            if (info != null)
            {
                SelectSingleSource(info);
            }
        }
        /// <summary>
        /// 选择数据源
        /// </summary>
        /// <param name="info"></param>
        private async void SelectSingleSource(SingleSourceInfo info)
        {
            if (dailySelectedIndex == default(Guid))
                return;
            if (string.IsNullOrEmpty(deviceId))
                return;
            //获取缩略图对应的源地址
            string sourceUrl = info.SourceUrl;
            //根据索引改数据
            if (DevicesScreenItemsInfoDic.ContainsKey(deviceId))
            {
                foreach (var item in DevicesScreenItemsInfoDic[deviceId].ScreenItems)
                {
                    if (item.ID == dailySelectedIndex)
                    {
                        item.TargetUrl = sourceUrl;
                        await WCFServiceManager.Instance.UpdateWindowInfo(deviceId, item);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 大屏各设备显示的信息
        /// </summary>
        Dictionary<string, ScreenItemsInfo> DevicesScreenItemsInfoDic = new Dictionary<string, ScreenItemsInfo>();

        /// <summary>
        /// 设置日常模式数据（ 布局、区域列表等）
        /// </summary>
        public async void SetDailyWindow()
        {
            //多机还是每次都获取算了
            DevicesScreenItemsInfoDic.Clear();
            foreach (KeyValuePair<string, ServiceReference1.Service1Client> KeyValuePair in WCFServiceManager.DeviceClientDic)
            {
                DevicesScreenItemsInfoDic.Add(KeyValuePair.Key, await WCFServiceManager.Instance.GetCurrentWindowInfo(KeyValuePair.Key));
            }
            DailyBoard.ColumnDefinitions.Clear();
            DailyBoard.Children.Clear();
            //根据设备数量分割示意区域
            for (int i = 0; i < DevicesScreenItemsInfoDic.Count; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                DailyBoard.ColumnDefinitions.Add(columnDefinition);
            }
            //向每个区域添加Border
            foreach (var item in DevicesScreenItemsInfoDic)
            {
                if (item.Value == null)
                    continue;
                Grid board = new Grid();
                board.Tag = item.Key;
                DataManager.SetBoardLayout(item.Value.Rows, item.Value.Columns, board);
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    try
                    {
                        board.Children.Clear();
                        foreach (var singleItem in item.Value.ScreenItems)
                        {
                            Border border = new Border()
                            {
                                Background = FindResource("RegionBackground") as SolidColorBrush,
                                BorderBrush = new SolidColorBrush(Colors.Black),
                                BorderThickness = new Thickness(1),
                                Tag = singleItem.ID
                            };
                            border.MouseDown += Border_MouseDown;
                            border.TouchDown += Border_TouchDown;
                            Grid.SetRow(border, singleItem.Row);
                            Grid.SetRowSpan(border, singleItem.RowSpan);
                            Grid.SetColumn(border, singleItem.Column);
                            Grid.SetColumnSpan(border, singleItem.ColumnSpan);
                            //在Border里添加Text Block把区域显示在Border上
                            TextBlock textBlock = new TextBlock()
                            {
                                Text = "区域" + (item.Value.ScreenItems.IndexOf(singleItem) + 1),
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };
                            border.Child = textBlock;
                            board.Children.Add(border);
                            Grid.SetColumn(board, DevicesScreenItemsInfoDic.ToList().IndexOf(item));

                        }
                        DailyBoard.Children.Add(board);
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                    }

                }));
            }
        }

        /// <summary>
        /// 点击示意区域中某区域时（Border）touchdown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_TouchDown(object sender, TouchEventArgs e)
        {
            //选中区域红边，其他黑边
            Border border = sender as Border;
            if (border == null)
                return;
            border.BorderBrush = new SolidColorBrush(Colors.Red);
            dailySelectedIndex = (Guid)border.Tag;
            foreach (Grid grid in ((border.Parent as Grid).Parent as Grid).Children)
            {
                foreach (Border item in grid.Children)
                {
                    if ((Guid)item.Tag == dailySelectedIndex)
                    {
                        deviceId = grid.Tag.ToString();
                        continue;
                    }
                    item.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

        /// <summary>
        /// 点击示意区域中某区域时，鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //选中区域红边，其他黑边
            Border border = sender as Border;
            if (border == null)
                return;
            border.BorderBrush = new SolidColorBrush(Colors.Red);
            dailySelectedIndex = (Guid)border.Tag;
            foreach (Grid grid in ((border.Parent as Grid).Parent as Grid).Children)
            {
                foreach (Border item in grid.Children)
                {
                    if ((Guid)item.Tag == dailySelectedIndex)
                    {
                        deviceId = grid.Tag.ToString();
                        continue;
                    }
                    item.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

        #region 拖拽相关
        //是否处于拖拽图片状态
        private bool IsDragging = false;
        //当前拖拽状态 0为普通拖拽 1为拖拽至发送区 3为完成发送
        private int dragStatus = 0;
        //当前拖拽状态 0为普通拖拽 1为拖拽至发送区 3为完成发送
        public int DragStatus
        {
            get { return dragStatus; }
            set
            {
                if (value != dragStatus)
                {
                    dragStatus = value;
                    this.RaisePropertyChanged("DragStatus");
                }
            }
        }
        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {

            e.Handled = true;

            var image = (e.OriginalSource as FrameworkElement);
            //检查对象是否是listbox中的item
            if (
                    image != null
                 && image.Tag != null
                 && image.Tag.GetType() == typeof(string)
                 && (string)image.Tag == "listboxitem"
                )
            {
                //解除动画对属性的绑定
                drageImage.BeginAnimation(Canvas.TopProperty, null);
                IsDragging = true;
                //获取拖拽对象的datacontext并存在跟随拖拽点的图片框中
                drageImage.DataContext = image.DataContext as SingleSourceInfo;
                TouchPoint touchPoint = e.GetTouchPoint(this);
                DraggingImage(touchPoint);
                //显示跟随触点的图片
                drageImage.BeginAnimation(VisibilityProperty, null);
                Dispatcher.Invoke(new Action(() =>
                 {
                     drageImage.Visibility = Visibility.Visible;
                 }));
            }
        }

        private void UIElement_OnTouchMove(object sender, TouchEventArgs e)
        {
            e.Handled = true;
            if (IsDragging)
            {
                TouchPoint touchPoint = e.GetTouchPoint(this);
                DraggingImage(touchPoint);
            }

        }
        /// <summary>
        /// 跟随触点刷新图片框的位置
        /// </summary>
        /// <param name="touchPoint"></param>
        private void DraggingImage(TouchPoint touchPoint)
        {   //使图片框中点与触点对齐
            Canvas.SetLeft(drageImage, touchPoint.Bounds.Left - drageImage.ActualWidth / 2);
            Canvas.SetTop(drageImage, touchPoint.Bounds.Top - drageImage.ActualHeight / 2);
            if (touchPoint.Bounds.Top < canvas.ActualHeight / sendAreaRatio)
            {
                //进入发送区域
                DragStatus = 1;
            }
            else
            {
                //未进入发送区域
                DragStatus = 0;
            }
        }

        private void Canvas_OnTouchUp(object sender, TouchEventArgs e)
        {
            e.Handled = true;
            if (IsDragging)
            {
                DragRelease(e.GetTouchPoint(canvas));
            }
            //操作完成，标记拖拽结束
            IsDragging = false;
        }




        /// <summary>
        /// 释放拖拽图片时，计算释放点的位置
        /// </summary>
        /// <param name="touchPoint"></param>
        private void DragRelease(TouchPoint touchPoint)
        {
            //操作完成，重置图片框状态
            DragStatus = 0;
            if (touchPoint.Bounds.Top < canvas.ActualHeight / sendAreaRatio)
            {
                //操作完成，发送信号源，播放动画
                DragStatus = 2;
                //TODO DO SOMETHING
                //跟随触点的图片框中的datacontext就是我们要的数据
                var info = (drageImage.DataContext as SingleSourceInfo);
                SelectSingleSource(info);
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    drageImage.Visibility = Visibility.Collapsed;
                }));
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
